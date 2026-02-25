using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Transport.Targets;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;

namespace DotNetSnmp.Client
{
    /// <summary>
    /// The message processing model for SNMPv1.
    /// </summary>
    public class V1MessageProcessingModel : IMessageProcessingModel
    {
        public bool IsProtocolVersionSupported(VersionCode version)
        {
            return version == VersionCode.V1;
        }

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
            result = MessageProcessingResult.Success;
            sendPduHandle = -1;
            securityLevel = 0;
            securityModel = 0;
            securityName = string.Empty;
            message = null;

            try
            {
                var reader = new AsnReader(incomingMessage, AsnEncodingRules.BER);
                message = SnmpV1Message.ReadFrom(reader);
                sendPduHandle = message.Scope!.RequestId;
                securityName = ((SnmpV1Message)message).Community;
                securityModel = 0;
                return true;
            }
            catch (Exception)
            {
                result = MessageProcessingResult.InternalError;
                return false;
            }
        }

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

            result = MessageProcessingResult.Success;
            sendPduHandle = -1;
            outgoingMessage = null;

            ArgumentNullException.ThrowIfNull(scope);

            if (target.ProtocolVersion != VersionCode.V1)
            {
                result = MessageProcessingResult.UnsupportedSecurityModel;
                return false;
            }

            if (scope is Scope)
            {
                throw new ArgumentException(
                    $"{nameof(scope)} of type ScopedPdu is not supported by V1MessageProcessingModel");
            }

            var pdu = (Pdu)scope;
            if (pdu.RequestId <= 0)
            {
                pdu.RequestId = Random.Shared.Next();
            }

            outgoingMessage = new SnmpV1Message
            {
                Community = target.SecurityName,
                Scope = pdu
            };

            sendPduHandle = pdu.RequestId;

            return true;
        }
    }
}
