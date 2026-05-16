using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
/// </summary>
public static class MessageFactory
{
    internal readonly struct V3SecurityState
    {
        public V3SecurityState(bool authenticationFailed, bool decryptionFailed, bool unknownUser)
        {
            AuthenticationFailed = authenticationFailed;
            DecryptionFailed = decryptionFailed;
            UnknownUser = unknownUser;
        }

        public bool AuthenticationFailed { get; }

        public bool DecryptionFailed { get; }

        public bool UnknownUser { get; }

        public bool HasFailures => AuthenticationFailed || DecryptionFailed || UnknownUser;
    }

    private sealed class V3SecurityStateHolder
    {
        public V3SecurityState State { get; set; }
    }

    private static readonly ConditionalWeakTable<ISnmpMessage, V3SecurityStateHolder> V3SecurityStates = new();

    /// <summary>
    /// Parses one or more SNMP messages from hexadecimal text.
    /// </summary>
    public static IList<ISnmpMessage> ParseMessages(IEnumerable<char> bytes, UserRegistry registry)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }
#pragma warning disable CS0618 // Compatibility overload intentionally targets legacy char-sequence parsing semantics.
        return ParseMessages(ByteTool.Convert(bytes), registry);
#pragma warning restore CS0618
    }

    /// <summary>
    /// Parses one or more SNMP messages from a byte array.
    /// </summary>
    public static IList<ISnmpMessage> ParseMessages(byte[] buffer, UserRegistry registry)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        return ParseMessages(buffer, 0, buffer.Length, registry, throwOnV3SecurityError: false);
    }

    /// <summary>
    /// Parses one or more SNMP messages from a segment of a byte array.
    /// </summary>
    public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int length, UserRegistry registry)
    {
        return ParseMessages(buffer, index, length, registry, throwOnV3SecurityError: false);
    }

    /// <summary>
    /// Parses one or more SNMP messages from a segment of a byte array.
    /// </summary>
    public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int length, UserRegistry registry, bool throwOnV3SecurityError)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        if (index < 0 || index > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (length < 0 || length > buffer.Length - index)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        return ParseMessages(new ReadOnlyMemory<byte>(buffer, index, length), registry, throwOnV3SecurityError);
    }

    /// <summary>
    /// Parses one or more SNMP messages from BER-encoded bytes.
    /// </summary>
    /// <param name="bytes">Byte string.</param>
    /// <param name="registry">The registry.</param>
    /// <returns></returns>
    public static IList<ISnmpMessage> ParseMessages(ReadOnlyMemory<byte> bytes, UserRegistry registry)
    {
        return ParseMessages(bytes, registry, throwOnV3SecurityError: false);
    }

    /// <summary>
    /// Parses one or more SNMP messages from BER-encoded bytes.
    /// </summary>
    /// <param name="bytes">Byte string.</param>
    /// <param name="registry">The registry.</param>
    /// <param name="throwOnV3SecurityError">
    /// If <c>true</c>, throws on v3 security failures (manager/client behavior).
    /// If <c>false</c>, keeps parsing and records failures for agent-side report generation.
    /// </param>
    /// <returns></returns>
    public static IList<ISnmpMessage> ParseMessages(ReadOnlyMemory<byte> bytes, UserRegistry registry, bool throwOnV3SecurityError)
    {
        if (registry == null)
        {
            throw new ArgumentNullException(nameof(registry));
        }

        var result = new List<ISnmpMessage>();

        try
        {
            // Create an AsnReader for the entire byte array
            var reader = new AsnReader(bytes, AsnEncodingRules.BER);

            // Continue parsing messages until we've consumed all data
            while (reader.HasData)
            {
                // Read the full encoded message directly to avoid extra copy/allocation.
                ReadOnlyMemory<byte> messageData = reader.ReadEncodedValue();

                // Use a temporary reader to peek at the version
                var versionReader = new AsnReader(messageData, AsnEncodingRules.BER);
                var rootSeq = versionReader.ReadSequence();
                if (!rootSeq.TryReadInt32(out var messageVersion))
                {
                    throw new SnmpDecodeException("Failed to read SNMP version from message");
                }

                // Convert version number to enum
                var version = (VersionCode)messageVersion;

                // Parse the message based on its version, passing the full message data
                // to the appropriate version-specific parser
                ISnmpMessage message;

                switch (version)
                {
                    case VersionCode.V1:
                        message = Lextm.SharpSnmpLib.SnmpV1Message.ReadFrom(
                            new AsnReader(messageData, AsnEncodingRules.BER));
                        break;

                    case VersionCode.V2:
                        var v2Message = Lextm.SharpSnmpLib.SnmpV2Message.ReadFrom(
                            new AsnReader(messageData, AsnEncodingRules.BER));
                        message = v2Message.Scope?.Pdu is TrapV2Pdu
                            ? new TrapV2Message(v2Message)
                            : v2Message.Scope?.Pdu is InformRequestPdu
                                ? new InformRequestMessage(v2Message)
                            : v2Message;
                        break;

                    case VersionCode.V3:
                        var v3Message = Lextm.SharpSnmpLib.SnmpV3Message.ReadFrom(
                            new AsnReader(messageData, AsnEncodingRules.BER));

                        // For V3, handle security operations if message has security flags
                        var securityState = ProcessV3Security(v3Message, registry, throwOnV3SecurityError);

                        message = v3Message.Scope?.Pdu is TrapV2Pdu
                            ? new TrapV2Message(v3Message)
                            : v3Message.Scope?.Pdu is InformRequestPdu
                                ? new InformRequestMessage(v3Message)
                            : v3Message;
                        if (securityState.HasFailures)
                        {
                            SetV3SecurityState(message, securityState);
                        }

                        break;

                    default:
                        throw new SnmpDecodeException($"Unsupported SNMP version: {version}");
                }

                result.Add(message);
            }
        }
        catch (AsnContentException ex)
        {
            throw new SnmpDecodeException($"Error parsing SNMP message: {ex.Message}");
        }
        catch (SnmpDecodeException)
        {
            throw; // Just rethrow already formatted SNMP-specific exceptions
        }
        catch (Exception ex)
        {
            throw new SnmpDecodeException($"Error processing SNMP message: {ex.Message}");
        }

        return result;
    }

    private static V3SecurityState ProcessV3Security(
        Lextm.SharpSnmpLib.SnmpV3Message v3Message,
        UserRegistry registry,
        bool throwOnV3SecurityError)
    {
        var authenticationFailed = false;
        var decryptionFailed = false;
        var unknownUser = false;

        // Only process security if the message contains security flags
        var msgFlags = v3Message.Header.MsgFlags;

        // Skip security processing for discovery messages (which have empty security name)
        if (string.IsNullOrEmpty(v3Message.SecurityParameters.SecurityName))
        {
            return new V3SecurityState(authenticationFailed, decryptionFailed, unknownUser);
        }

        // Find the appropriate security parameters from registry
        var userName = v3Message.SecurityParameters.SecurityName;
        var privacy = registry.Find(userName);

        if (privacy == null)
        {
            unknownUser = true;
            if (throwOnV3SecurityError)
            {
                throw new SnmpDecodeException($"User '{userName}' not found in registry");
            }

            return new V3SecurityState(authenticationFailed, decryptionFailed, unknownUser);
        }

        var auth = privacy.AuthenticationProvider;

        // Process authentication if needed
        if ((msgFlags & Lextm.SharpSnmpLib.Security.MsgFlag.Auth) != 0)
        {
            bool authenticated = auth.AuthenticateIncomingMsg(v3Message);
            if (!authenticated)
            {
                authenticationFailed = true;
                if (throwOnV3SecurityError)
                {
                    throw new SnmpDecodeException("Authentication failed for incoming message");
                }

                return new V3SecurityState(authenticationFailed, decryptionFailed, unknownUser);
            }
        }

        // Process privacy (decryption) if needed
        if ((msgFlags & Lextm.SharpSnmpLib.Security.MsgFlag.Priv) != 0)
        {
            try
            {
                privacy.DecryptMessage(v3Message);
            }
            catch when (!throwOnV3SecurityError)
            {
                decryptionFailed = true;
            }
        }

        return new V3SecurityState(authenticationFailed, decryptionFailed, unknownUser);
    }

    private static void SetV3SecurityState(ISnmpMessage message, V3SecurityState state)
    {
        if (message == null || !state.HasFailures)
        {
            return;
        }

        V3SecurityStates.Remove(message);
        V3SecurityStates.Add(message, new V3SecurityStateHolder { State = state });
    }

    internal static bool TryGetV3SecurityState(ISnmpMessage message, out V3SecurityState state)
    {
        state = default;
        if (message == null)
        {
            return false;
        }

        if (V3SecurityStates.TryGetValue(message, out var holder))
        {
            state = holder.State;
            return true;
        }

        return false;
    }
}
