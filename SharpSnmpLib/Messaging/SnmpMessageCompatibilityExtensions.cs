using System.Formats.Asn1;
using System.Net;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using DotNetSnmp.Transport;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy PDU facade exposed by compatibility APIs.
/// </summary>
public sealed class LegacyPdu
{
    internal LegacyPdu(Pdu pdu)
    {
        Variables = pdu.VariableBindings?.ToList() ?? new List<Variable>();
        ErrorStatus = new Integer32((int)pdu.ErrorStatus);
        ErrorIndex = new Integer32(pdu.ErrorIndex);
    }

    /// <summary>
    /// Gets variables.
    /// </summary>
    public IList<Variable> Variables { get; }

    /// <summary>
    /// Gets error Status.
    /// </summary>
    public Integer32 ErrorStatus { get; }

    /// <summary>
    /// Gets error Index.
    /// </summary>
    public Integer32 ErrorIndex { get; }
}

/// <summary>
/// Provides helper methods for SnmpMessageCompatibilityExtensions.
/// </summary>
public static class SnmpMessageCompatibilityExtensions
{
    /// <summary>
    /// Serializes the message to a byte array.
    /// </summary>
    public static byte[] ToBytes(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message is ReportMessage report)
        {
            return report.ToBytes();
        }

        return message.Encode();
    }

    /// <summary>
    /// Returns the message PDU wrapped in the legacy compatibility facade.
    /// </summary>
    public static LegacyPdu Pdu(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var pdu = message.Scope?.Pdu;
        if (pdu == null)
        {
            throw new SnmpException("The SNMP message does not contain a scoped PDU.");
        }

        return new LegacyPdu(pdu);
    }

    /// <summary>
    /// Gets the response message.
    /// </summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        if (timeout < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        if (timeout == 0 || timeout == -1)
        {
            return GetResponseAsync(request, receiver).GetAwaiter().GetResult();
        }

        try
        {
            return GetResponseAsync(request, receiver)
                .WaitAsync(TimeSpan.FromMilliseconds(timeout))
                .GetAwaiter()
                .GetResult();
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"SNMP request timed out after {timeout} milliseconds.", ex);
        }
    }

    private static async Task<ISnmpMessage> GetResponseAsync(ISnmpMessage request, IPEndPoint receiver)
    {
        using var transport = new BasicUdpTransport(receiver);
        await transport.SendAsync(request.ToBytes(), receiver).ConfigureAwait(false);

        var incoming = await transport.ReceiveAsync(receiver, CancellationToken.None).ConfigureAwait(false);
        return ParseResponse(incoming, request);
    }

    private static ISnmpMessage ParseResponse(ReadOnlyMemory<byte> incoming, ISnmpMessage request)
    {
        var versionReader = new AsnReader(incoming, AsnEncodingRules.BER);
        var root = versionReader.ReadSequence();
        if (!root.TryReadInt32(out var versionRaw))
        {
            throw new SnmpException("Cannot parse response version.");
        }

        var version = (VersionCode)versionRaw;
        return version switch
        {
            VersionCode.V1 => SnmpV1Message.ReadFrom(new AsnReader(incoming, AsnEncodingRules.BER)),
            VersionCode.V2 => SnmpV2Message.ReadFrom(new AsnReader(incoming, AsnEncodingRules.BER)),
            VersionCode.V3 => ParseV3Response(incoming, request),
            _ => throw new SnmpException($"Unsupported SNMP version in response: {versionRaw}.")
        };
    }

    private static ISnmpMessage ParseV3Response(ReadOnlyMemory<byte> incoming, ISnmpMessage request)
    {
        var response = SnmpV3Message.ReadFrom(new AsnReader(incoming, AsnEncodingRules.BER));

        if (request is ILegacyV3Request v3Request)
        {
            if (response.Header.MsgFlags.HasFlag(MsgFlags.Auth))
            {
                var authenticated = v3Request.Privacy.AuthenticationProvider.AuthenticateIncomingMsg(response);
                if (!authenticated)
                {
                    throw new SnmpException("Authentication failed for incoming response.");
                }
            }

            if (response.Header.MsgFlags.HasFlag(MsgFlags.Priv))
            {
                v3Request.Privacy.DecryptMessage(response);
            }
        }

        if (response.Scope?.Pdu is ReportPdu)
        {
            return new ReportMessage(response);
        }

        return response;
    }
}
