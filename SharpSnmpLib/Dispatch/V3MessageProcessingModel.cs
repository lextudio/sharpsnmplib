using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Represents the V3MessageProcessingModel type.
    /// </summary>
    public class V3MessageProcessingModel : IMessageProcessingModel
    {


        /// <inheritdoc/>
        public bool IsProtocolVersionSupported(VersionCode version)
        {
            return version == VersionCode.V3;
        }

        /// <inheritdoc/>
        public bool TryPrepareDataElements(
            in ReadOnlyMemory<byte> incomingMessage,
            in ISnmpTarget target,
            out string securityName,
            out Levels securityLevel,
            out SecurityModel securityModel,
            [NotNullWhen(true)] out ISnmpMessage? message,
            out int sendPduHandle,
            out MessageProcessingResult result)
        {
            securityName = string.Empty;
            securityLevel = 0;
            securityModel = SecurityModel.Usm;
            message = null;
            sendPduHandle = -1;
            result = MessageProcessingResult.Success;

            UserTarget userTarget = (UserTarget)target;

            try
            {
                var reader = new AsnReader(incomingMessage, AsnEncodingRules.BER);
                var v3Message = SnmpV3Message.ReadFrom(reader);

                // Extract basic security information
                securityModel = v3Message.Header.MsgSecurityModel;
                securityName = v3Message.SecurityParameters.SecurityName;

                // Determine security level from flags
                var msgFlags = v3Message.Header.MsgFlags;
                securityLevel = 0;
                var isAuth = msgFlags.HasFlag(MsgFlag.Auth);
                var isPriv = msgFlags.HasFlag(MsgFlag.Priv);
                if (isAuth)
                {
                    securityLevel |= Levels.Authentication;
                }
                if (isPriv)
                {
                    securityLevel |= Levels.Privacy;
                }

                // Process authentication if required
                if (isAuth)
                {
                    var authenticationService = userTarget.AuthenticationService;

                    // Authentication processing would go here
                    // This would require user credentials/key to verify
                    // Since we don't have that context in this method,
                    // we would likely need to pass this to another
                    var valid = authenticationService.AuthenticateIncomingMsg(
                        v3Message
                    );
                    if (!valid)
                    {
                        result = MessageProcessingResult.AuthenticationError;
                        return false;
                    }

                    // Process encrypted PDU if present
                    if (isPriv)
                    {
                        var privacyService = userTarget.PrivacyService;
                        // We would need privacy keys to decrypt
                        // Again, this might need to be delegated to a security module
                        // that has access to the necessary keys
                        privacyService.DecryptMessage(v3Message);
                    }
                }

                // Extract PDU
                if (v3Message.Scope == null)
                {
                    result = MessageProcessingResult.DecryptionError;
                    return false;
                }

                // Record message ID as PDU handle for response correlation
                sendPduHandle = v3Message.Header.MsgId;
                message = v3Message;
                return true;
            }
            catch (AsnContentException)
            {
                result = MessageProcessingResult.BadEncoding;
                return false;
            }
            catch (Exception)
            {
                result = MessageProcessingResult.InternalError;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryPrepareOutgoingMessage(
            in ISnmpTarget target,
            in IScope scope,
            in ReadOnlyMemory<byte> secEngineId,
            out int sendPduHandle,
            [NotNullWhen(true)] out ISnmpMessage? outgoingMessage,
            out MessageProcessingResult result,
            Memory<byte> digestBuffer,
            bool expectResponse = true)
        {
            sendPduHandle = -1;
            outgoingMessage = null;
            result = MessageProcessingResult.Success;
            if (target.ProtocolVersion != VersionCode.V3)
            {
                result = MessageProcessingResult.UnsupportedSecurityModel;
                return false;
            }

            var userTarget = (UserTarget)target;

            // Use the cached engine ID if provided and secEngineId is empty
            var engineId = secEngineId.IsEmpty ? userTarget.EngineId : secEngineId;

            if (userTarget?.SecurityModel != SecurityModel.Usm)
            {
                result = MessageProcessingResult.UnsupportedSecurityModel;
                return false;
            }

            var securityLevel = userTarget.SecurityLevel;

            if (scope is Pdu)
            {
                throw new ArgumentException(
                    $"{nameof(scope)} must be ScopedPdu for V3MessageProcessingModel");
            }

            var scopedPdu = (Scope)scope;

            scopedPdu.ContextEngineId = engineId;

            if (scopedPdu.Pdu.RequestId <= 0)
            {
                scopedPdu.Pdu.RequestId = Random.Shared.Next();
            }

            var msgFlags = securityLevel switch
            {
                Levels.Authentication => MsgFlag.Auth,
                Levels.Privacy => MsgFlag.Priv,
                Levels.Authentication | Levels.Privacy => MsgFlag.Auth | MsgFlag.Priv,
                0 => MsgFlag.NoAuthNoPriv,
                _ => MsgFlag.NoAuthNoPriv
            };

            if (scopedPdu.Pdu.IsConfirmed())
            {
                msgFlags |= MsgFlag.Reportable;
            }

            var message = new SnmpV3Message
            {
                Header = new()
                {
                    MsgId = scopedPdu.Pdu.RequestId > 0 ? scopedPdu.Pdu.RequestId : Random.Shared.Next(),
                    MsgFlags = msgFlags,
                    MsgMaxSize = target.MaxMessageSize,
                    MsgSecurityModel = userTarget.SecurityModel
                },
                SecurityParameters = new()
                {
                    SecurityName = target.SecurityName,
                    EngineId = !engineId.IsEmpty ? engineId.ToArray() : null,
                    EngineBoots = userTarget.EngineBoots,
                    EngineTime = userTarget.EngineTime,
                },
                Scope = scopedPdu,
            };

            if (msgFlags.HasFlag(MsgFlag.Priv))
            {
                userTarget.PrivacyService.EncryptMessage(message);
            }

            if (msgFlags.HasFlag(MsgFlag.Auth))
            {
                var authModel = userTarget.AuthenticationService;
                authModel.AuthenticateOutgoingMsg(message, digestBuffer);
            }

            outgoingMessage = message;
            sendPduHandle = message.Header.MsgId;
            return true;
        }
    }
}
