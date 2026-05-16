using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy PDU facade exposed by compatibility APIs.
/// </summary>
public sealed class LegacyPdu : ISnmpPdu
{
    private readonly Pdu _pdu;

    internal LegacyPdu(Pdu pdu)
    {
        _pdu = pdu ?? throw new ArgumentNullException(nameof(pdu));
        Variables = pdu.VariableBindings?.ToList() ?? new List<Variable>();

        if (TryGetBulkRequestParameters(pdu, out var nonRepeaters, out var maxRepetitions))
        {
            ErrorStatus = new Integer32(nonRepeaters);
            ErrorIndex = new Integer32(maxRepetitions);
        }
        else
        {
            ErrorStatus = new Integer32((int)pdu.ErrorStatus);
            ErrorIndex = new Integer32(pdu.ErrorIndex);
        }
    }

    private static bool TryGetBulkRequestParameters(Pdu pdu, out int nonRepeaters, out int maxRepetitions)
    {
        nonRepeaters = 0;
        maxRepetitions = 0;

        if (pdu.TypeCode != SnmpType.GetBulkRequestPdu)
        {
            return false;
        }

        if (pdu is GetBulkRequestPdu bulk)
        {
            nonRepeaters = bulk.NonRepeaters;
            maxRepetitions = bulk.MaxRepetitions;
            return true;
        }

        // Some compatibility builds can surface a different CLR type for bulk PDUs.
        // Read by property name to preserve v12 semantics in those mixed-type scenarios.
        var type = pdu.GetType();
        var nonRepeatersProperty = type.GetProperty("NonRepeaters");
        var maxRepetitionsProperty = type.GetProperty("MaxRepetitions");
        if (nonRepeatersProperty?.PropertyType == typeof(int) && maxRepetitionsProperty?.PropertyType == typeof(int))
        {
            nonRepeaters = (int)(nonRepeatersProperty.GetValue(pdu) ?? 0);
            maxRepetitions = (int)(maxRepetitionsProperty.GetValue(pdu) ?? 0);
            return true;
        }

        return false;
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

    /// <summary>
    /// Gets request id.
    /// </summary>
    public Integer32 RequestId => new(_pdu.RequestId);

    /// <summary>
    /// Gets PDU type code.
    /// </summary>
    public SnmpType TypeCode => _pdu switch
    {
        GetRequestPdu => SnmpType.GetRequestPdu,
        GetNextRequestPdu => SnmpType.GetNextRequestPdu,
        GetBulkRequestPdu => SnmpType.GetBulkRequestPdu,
        SetRequestPdu => SnmpType.SetRequestPdu,
        ResponsePdu => SnmpType.ResponsePdu,
        TrapPdu => SnmpType.TrapV1Pdu,
        TrapV2Pdu => SnmpType.TrapV2Pdu,
        InformRequestPdu => SnmpType.InformRequestPdu,
        ReportPdu => SnmpType.ReportPdu,
        _ => SnmpType.Unknown
    };

    /// <summary>
    /// Casts to <see cref="TrapV2Pdu"/> when possible.
    /// </summary>
    public static explicit operator TrapV2Pdu(LegacyPdu pdu)
    {
        if (pdu == null)
        {
            throw new ArgumentNullException(nameof(pdu));
        }

        if (pdu._pdu is TrapV2Pdu trap)
        {
            return trap;
        }

        throw new InvalidCastException("Legacy PDU is not a TrapV2Pdu.");
    }

    /// <summary>
    /// Casts to <see cref="InformRequestPdu"/> when possible.
    /// </summary>
    public static explicit operator InformRequestPdu(LegacyPdu pdu)
    {
        if (pdu == null)
        {
            throw new ArgumentNullException(nameof(pdu));
        }

        if (pdu._pdu is InformRequestPdu inform)
        {
            return inform;
        }

        throw new InvalidCastException("Legacy PDU is not an InformRequestPdu.");
    }
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
            if (response.Header.MsgFlags.HasFlag(MsgFlag.Auth))
            {
                var authenticated = v3Request.Privacy.AuthenticationProvider.AuthenticateIncomingMsg(response);
                if (!authenticated)
                {
                    throw new SnmpException("Authentication failed for incoming response.");
                }
            }

            if (response.Header.MsgFlags.HasFlag(MsgFlag.Priv))
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
