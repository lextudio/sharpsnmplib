using System.Formats.Asn1;
using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Common.Definitions;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging;

public static class MessageFactory
{
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

    public static IList<ISnmpMessage> ParseMessages(byte[] buffer, UserRegistry registry)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        return ParseMessages(buffer, 0, buffer.Length, registry);
    }

    public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int length, UserRegistry registry)
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

        return ParseMessages(new ReadOnlyMemory<byte>(buffer, index, length), registry);
    }

    /// <summary>
    /// Creates <see cref="ISnmpMessage"/> instances from a string.
    /// </summary>
    /// <param name="bytes">Byte string.</param>
    /// <param name="registry">The registry.</param>
    /// <returns></returns>
    public static IList<ISnmpMessage> ParseMessages(ReadOnlyMemory<byte> bytes, UserRegistry registry)
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
                // Get the entire message data before we move on to the next one
                ReadOnlyMemory<byte> messageData = reader.PeekEncodedValue().ToArray();

                // Skip over this message in the main reader so we can continue with the next message
                reader.ReadEncodedValue();

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
                        message = DotNetSnmp.Protocol.V1.SnmpV1Message.ReadFrom(
                            new AsnReader(messageData, AsnEncodingRules.BER));
                        break;

                    case VersionCode.V2:
                        message = DotNetSnmp.Protocol.V2.SnmpV2Message.ReadFrom(
                            new AsnReader(messageData, AsnEncodingRules.BER));
                        break;

                    case VersionCode.V3:
                        var v3Message = DotNetSnmp.Protocol.V3.SnmpV3Message.ReadFrom(
                            new AsnReader(messageData, AsnEncodingRules.BER));

                        // For V3, handle security operations if message has security flags
                        ProcessV3Security(v3Message, registry);

                        message = v3Message;
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

    private static void ProcessV3Security(DotNetSnmp.Protocol.V3.SnmpV3Message v3Message, UserRegistry registry)
    {
        // Only process security if the message contains security flags
        var msgFlags = v3Message.Header.MsgFlags;

        // Skip security processing for discovery messages (which have empty security name)
        if (string.IsNullOrEmpty(v3Message.SecurityParameters.SecurityName))
        {
            return;
        }

        // Find the appropriate security parameters from registry
        var userName = v3Message.SecurityParameters.SecurityName;
        var privacy = registry.Find(userName);

        if (privacy == null)
        {
            throw new SnmpDecodeException($"User '{userName}' not found in registry");
        }

        var auth = privacy.AuthenticationProvider;

        // Process authentication if needed
        if (msgFlags.HasFlag(DotNetSnmp.Protocol.V3.Security.MsgFlags.Auth))
        {
            bool authenticated = auth.AuthenticateIncomingMsg(v3Message);
            if (!authenticated)
            {
                throw new SnmpDecodeException("Authentication failed for incoming message");
            }
        }

        // Process privacy (decryption) if needed
        if (msgFlags.HasFlag(DotNetSnmp.Protocol.V3.Security.MsgFlags.Priv))
        {
            privacy.DecryptMessage(v3Message);
        }
    }
}
