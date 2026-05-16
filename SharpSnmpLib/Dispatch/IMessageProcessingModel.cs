using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Defines the contract for IMessageProcessingModel.
    /// </summary>
    public interface IMessageProcessingModel
    {
        /// <summary>
        /// Determines whether this processing model supports the specified SNMP protocol version.
        /// </summary>
        bool IsProtocolVersionSupported(VersionCode version);

        /// <summary>
        /// Builds an outgoing SNMP message from a target and PDU scope.
        /// </summary>
        bool TryPrepareOutgoingMessage(
            in ISnmpTarget target,
            in IScope scope,
            in ReadOnlyMemory<byte> secEngineId,
            out int sendPduHandle,
            [NotNullWhen(true)] out ISnmpMessage? outgoingMessage,
            out MessageProcessingResult result,
            Memory<byte> digestBuffer,
            bool expectResponse = true);

        /// <summary>
        /// Parses an incoming SNMP message into dispatch data elements.
        /// </summary>
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
