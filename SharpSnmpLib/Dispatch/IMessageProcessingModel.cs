using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Transport.Targets;
using System.Diagnostics.CodeAnalysis;

namespace DotNetSnmp.Client
{
    public interface IMessageProcessingModel
    {
        bool IsProtocolVersionSupported(VersionCode version);

        bool TryPrepareOutgoingMessage(
            in ISnmpTarget target,
            in IScope scope,
            in ReadOnlyMemory<byte> secEngineId,
            out int sendPduHandle,
            [NotNullWhen(true)] out ISnmpMessage? outgoingMessage,
            out MessageProcessingResult result,
            Memory<byte> digestBuffer,
            bool expectResponse = true);

        bool TryPrepareDataElements(
            in ReadOnlyMemory<byte> incomingMessage,
            in ISnmpTarget target,
            out string securityName,
            out Levels securityLevel,
            out SecurityModel securityModel,
            [NotNullWhen(true)] out ISnmpMessage? message,
            out int sendPduHandle,
            out MessageProcessingResult result);
    }
}
