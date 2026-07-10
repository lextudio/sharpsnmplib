using System.Formats.Asn1;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Test;

public sealed class MockTransport : ISnmpTransport
{
    private readonly Queue<byte[]> _responseQueue = new();
    public byte[]? LastSentMessage { get; private set; }
    public int SendCount { get; private set; }

    public Func<byte[], byte[]>? ResponseBuilder { get; set; }

    public MockTransport()
    {
    }

    public void EnqueueResponse(byte[] responseBytes)
    {
        _responseQueue.Enqueue(responseBytes);
    }

    public ValueTask<int> SendAsync(
        ReadOnlyMemory<byte> message,
        IPEndPoint targetEndPoint,
        CancellationToken cancellationToken = default)
    {
        LastSentMessage = message.ToArray();
        SendCount++;
        return ValueTask.FromResult(message.Length);
    }

    public ValueTask<ReadOnlyMemory<byte>> ReceiveAsync(
        IPEndPoint targetEndPoint,
        CancellationToken cancellationToken)
    {
        if (ResponseBuilder != null && LastSentMessage != null)
        {
            return ValueTask.FromResult(
                new ReadOnlyMemory<byte>(ResponseBuilder(LastSentMessage)));
        }

        if (_responseQueue.Count == 0)
            throw new InvalidOperationException("No response queued");

        return ValueTask.FromResult(
            new ReadOnlyMemory<byte>(_responseQueue.Dequeue()));
    }

    public void Dispose()
    {
        _responseQueue.Clear();
    }

    public static byte[] BuildGetResponse(
        byte[] requestBytes,
        VersionCode version,
        OctetString community,
        IList<Variable> variables)
    {
        var requestId = ExtractRequestId(requestBytes, version);

        var responseMsg = new ResponseMessage(
            requestId, version, community,
            ErrorCode.NoError, 0, variables);

        return responseMsg.ToBytes();
    }

    public static byte[] BuildGetResponse(
        byte[] requestBytes,
        VersionCode version,
        OctetString community,
        params Variable[] variables)
    {
        return BuildGetResponse(requestBytes, version, community, (IList<Variable>)variables);
    }

    private static int ExtractRequestId(byte[] requestBytes, VersionCode version)
    {
        if (requestBytes.Length == 0)
            return Random.Shared.Next(1, int.MaxValue);

        var reader = new AsnReader(requestBytes, AsnEncodingRules.BER);
        var requestId = version == VersionCode.V1
            ? SnmpV1Message.ReadFrom(reader).Scope!.RequestId
            : SnmpV2Message.ReadFrom(reader).Scope!.RequestId;

        return requestId > 0 ? requestId : Random.Shared.Next(1, int.MaxValue);
    }
}
