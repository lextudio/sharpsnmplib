using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Transport;
using DotNetSnmp.Transport.Targets;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Asn1;
using System.Net;
using System.Formats.Asn1;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Asn1.SyntaxObjects;
using System.Buffers;
using Microsoft.Extensions.Logging;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace DotNetSnmp.Client
{
    /// <summary>
    /// Represents the SnmpDispatcher type.
    /// </summary>
    public class SnmpDispatcher : ISnmpDispatcher
    {
        private readonly ILogger<SnmpDispatcher>? _logger;
        private IMessageProcessingModel? _v1MsgProcModel;
        private IMessageProcessingModel? _v2MsgProcModel;
        private IMessageProcessingModel? _v3UsmMsgProcModel;

        /// <summary>
        /// Initializes a new instance of SnmpDispatcher.
        /// </summary>
        public SnmpDispatcher(ILogger<SnmpDispatcher>? logger = null)
        {
            _logger = logger;
            _logger?.LogDebug("SnmpDispatcher initialized");
        }

        private IMessageProcessingModel GetMessageProcessingModel(VersionCode version) => version switch
        {
            VersionCode.V1 => _v1MsgProcModel ??= new V1MessageProcessingModel(),
            VersionCode.V2 => _v2MsgProcModel ??= new V2MessageProcessingModel(),
            VersionCode.V3 => _v3UsmMsgProcModel ??= new V3MessageProcessingModel(),
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Sends pdu.
        /// </summary>
        public async ValueTask<IScope> SendPdu(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            IScope scope,
            bool expectResponse = true,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Sending PDU to target {TargetAddress} with protocol version {ProtocolVersion}", targetAddress, target.ProtocolVersion);

            // IMPORTANT: enforce the target's timeout value
            var timeoutCts = new CancellationTokenSource(target.Timeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);

            var mpModel = GetMessageProcessingModel(
                target.ProtocolVersion);

            if (target.ProtocolVersion == VersionCode.V3)
            {
                return await SendPduV3(transport, target, targetAddress, scope, expectResponse, linkedCts.Token);
            }

            try
            {
                // Continue with standard V1/V2 processing
                if (mpModel.TryPrepareOutgoingMessage(
                    target,
                    scope,
                    ReadOnlyMemory<byte>.Empty,
                    out int pduHandle,
                    out ISnmpMessage? outgoingMessage,
                    out MessageProcessingResult result,
                    Memory<byte>.Empty,
                    expectResponse))
                {
                    _logger?.LogDebug("Outgoing message prepared successfully");

                    _ = await transport.SendAsync(
                        outgoingMessage!.Encode(),
                        targetAddress,
                        linkedCts.Token);

                    if (!expectResponse)
                    {
                        _logger?.LogDebug("No response expected, returning original PDU");
                        return scope;
                    }

                    try
                    {
                        var incomingData = await transport.ReceiveAsync(
                            targetAddress,
                            linkedCts.Token);

                        if (mpModel.TryPrepareDataElements(
                            incomingData,
                            target,
                            out var resSecurityName,
                            out var resSecurityLevel,
                            out var resSecurityModel,
                            out ISnmpMessage? message,
                            out int resPduHandle,
                            out var resProcessingResult))
                        {
                            if (resPduHandle != pduHandle)
                            {
                                _logger?.LogError("PDU handle mismatch: expected {ExpectedHandle}, got {ReceivedHandle}", pduHandle, resPduHandle);
                                throw new SnmpMessageProcessingException($"PDU handle mismatch: expected {pduHandle}, got {resPduHandle}");
                            }

                            _logger?.LogDebug("Response PDU prepared successfully");
                            return message.Scope!.Pdu;
                        }
                        else
                        {
                            _logger?.LogError("Message processing error: {ProcessingResult}", resProcessingResult);
                            throw new SnmpMessageProcessingException($"Failed to process the response from {targetAddress}", resProcessingResult);
                        }
                    }
                    catch (OperationCanceledException ex) when (timeoutCts.IsCancellationRequested)
                    {
                        _logger?.LogWarning(ex, "Timeout occurred while waiting for response from {TargetAddress} after {Timeout}ms", targetAddress, target.Timeout);
                        throw new SnmpTimeoutException($"Timeout waiting for response from {targetAddress}", targetAddress, target.Timeout);
                    }
                }
                else
                {
                    _logger?.LogError("Message processing error: {ProcessingResult}", result);
                    throw new SnmpMessageProcessingException($"Failed to prepare outgoing message for {targetAddress}", result);
                }
            }
            catch (OperationCanceledException ex) when (timeoutCts.IsCancellationRequested)
            {
                _logger?.LogWarning(ex, "Timeout occurred while communicating with {TargetAddress} after {Timeout}ms", targetAddress, target.Timeout);
                throw new SnmpTimeoutException($"Operation timed out while communicating with {targetAddress}", targetAddress, target.Timeout);
            }
            catch (SnmpMessageProcessingException)
            {
                // Already logged and contains the right information, just rethrow
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error occurred while sending PDU to {TargetAddress}", targetAddress);
                throw;
            }
        }

        private async ValueTask<IScope> SendPduV3(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            IScope scope,
            bool expectResponse,
            CancellationToken cancellationToken)
        {
            _logger?.LogDebug("Sending PDU using SNMPv3");

            try
            {
                // Get the V3 message processing model
                var mpModel = GetMessageProcessingModel(VersionCode.V3) as V3MessageProcessingModel
                    ?? throw new InvalidOperationException("Failed to get V3 message processing model");

                // Step 1: Engine ID Discovery with time information
                UserTarget userTarget = (UserTarget)target;

                // Check if we already have engine parameters for this target
                if (userTarget.EngineId.IsEmpty)
                {
                    try
                    {
                        // Discover engine parameters and store them in the target
                        // Pass the current PDU for discovery to match the actual operation type
                        EngineDiscoveryResult discoveryResult = await DiscoverEngineIdAndTime(transport, userTarget, targetAddress, cancellationToken, scope);

                        // Store the discovered parameters in the target
                        userTarget.EngineId = discoveryResult.EngineId;
                        userTarget.EngineBoots = discoveryResult.EngineBoots;
                        userTarget.EngineTime = discoveryResult.EngineTime;

                        _logger?.LogDebug("Stored engine parameters in target: EngineID={EngineIdLength} bytes, Boots={EngineBoots}, Time={EngineTime}",
                            userTarget.EngineId.Length, userTarget.EngineBoots, userTarget.EngineTime);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Engine discovery failed for {TargetAddress}", targetAddress);
                        throw new SnmpMessageProcessingException($"Engine discovery failed for {targetAddress}", ex);
                    }
                }
                else
                {
                    _logger?.LogDebug("Using existing engine parameters from target: EngineID={EngineIdLength} bytes, Boots={EngineBoots}, Time={EngineTime}",
                        userTarget.EngineId.Length, userTarget.EngineBoots, userTarget.EngineTime);
                }

                byte[] digestArray = ArrayPool<byte>.Shared.Rent(userTarget.AuthenticationService.TruncatedDigestSize);
                try
                {
                    Memory<byte> digestBuffer = digestArray.AsMemory(0, userTarget.AuthenticationService.TruncatedDigestSize);

                    // Step 3: Prepare and send the actual message
                    if (mpModel.TryPrepareOutgoingMessage(
                        target,
                        scope,
                        userTarget.EngineId,
                        out int pduHandle,
                        out ISnmpMessage? outgoingMessage,
                        out MessageProcessingResult result,
                        digestBuffer,
                        expectResponse))
                    {
                        _logger?.LogDebug("Outgoing SNMPv3 message prepared successfully");

                        _ = await transport.SendAsync(
                            outgoingMessage!.Encode(),
                            targetAddress,
                            cancellationToken);

                        if (!expectResponse)
                        {
                            _logger?.LogDebug("No response expected, returning original PDU");
                            return scope;
                        }

                        try
                        {
                            var incomingData = await transport.ReceiveAsync(
                                targetAddress,
                                cancellationToken);

                            if (mpModel.TryPrepareDataElements(
                                incomingData,
                                target,
                                out var resSecurityName,
                                out var resSecurityLevel,
                                out var resSecurityModel,
                                out ISnmpMessage? message,
                                out int resPduHandle,
                                out var resProcessingResult))
                            {
                                if (resPduHandle != pduHandle)
                                {
                                    _logger?.LogError("PDU handle mismatch: expected {ExpectedHandle}, got {ReceivedHandle}", pduHandle, resPduHandle);
                                    throw new SnmpMessageProcessingException($"PDU handle mismatch: expected {pduHandle}, got {resPduHandle}");
                                }

                                var resPdu = message.Scope?.Pdu;
                                if (resPdu == null)
                                {
                                    _logger?.LogError("Received null PDU from {TargetAddress}", targetAddress);
                                    throw new SnmpMessageProcessingException($"Received null PDU from {targetAddress}");
                                }

                                if (!resPdu.IsResponse())
                                {
                                    _logger?.LogError("Received non-response PDU: {Pdu}", resPdu);
                                    if (resPdu.VariableBindings?.Count() == 1 && resPdu.VariableBindings.First().Id.Oid == "1.3.6.1.6.3.15.1.1.2.0")
                                    {
                                        // IMPORTANT: This is a special case for engine discovery RFC3414
                                        var v3Message = (SnmpV3Message)message;
                                        userTarget.EngineTime = v3Message.SecurityParameters.EngineTime;
                                        userTarget.EngineBoots = v3Message.SecurityParameters.EngineBoots;
                                        _logger?.LogDebug("Engine time updated: Boots={EngineBoots}, Time={EngineTime}", userTarget.EngineBoots, userTarget.EngineTime);

                                        return await SendPduV3(transport, target, targetAddress, scope, expectResponse, cancellationToken).ConfigureAwait(false);
                                    }

                                    throw new SnmpMessageProcessingException($"Received non-response PDU from {targetAddress}, {resPdu.VariableBindings?.First().Id}");
                                }

                                _logger?.LogDebug("Response PDU prepared successfully");
                                return message.Scope!;
                            }
                            else
                            {
                                _logger?.LogError("Message processing error: {ProcessingResult}", resProcessingResult);
                                throw new SnmpMessageProcessingException($"Failed to process the response from {targetAddress}", resProcessingResult);
                            }
                        }
                        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
                        {
                            _logger?.LogWarning(ex, "Timeout occurred while waiting for response from {TargetAddress}", targetAddress);
                            throw new SnmpTimeoutException($"Timeout waiting for response from {targetAddress}", targetAddress, target.Timeout);
                        }
                    }
                    else
                    {
                        _logger?.LogError("Message processing error: {ProcessingResult}", result);
                        throw new SnmpMessageProcessingException($"Failed to prepare outgoing message for {targetAddress}", result);
                    }
                }
                finally
                {
                    // Return the rented buffer to the pool
                    ArrayPool<byte>.Shared.Return(digestArray);
                }
            }
            catch (SnmpMessageProcessingException)
            {
                // Already logged with appropriate context, just rethrow
                throw;
            }
            catch (SnmpTimeoutException)
            {
                // Already logged with appropriate context, just rethrow
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error occurred during SNMPv3 operation with {TargetAddress}", targetAddress);
                throw;
            }
        }

        private async ValueTask<EngineDiscoveryResult> DiscoverEngineIdAndTime(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            CancellationToken cancellationToken,
            IScope? originalScope = null)
        {
            _logger?.LogDebug("Discovering engine ID and time parameters for {TargetAddress}", targetAddress);

            // Create a discovery target with minimal security
            var discoveryTarget = new UserTarget(target.SecurityName, new DefaultPrivacyProvider(), target.MaxMessageSize);

            // Create a discovery PDU (with empty engine ID)
            // If originalPdu is provided, create a new PDU of the same type for better compatibility
            // Otherwise use a simple GetRequest
            Pdu discoveryInnerPdu;

            if (originalScope != null)
            {
                // Create an appropriate empty PDU based on the type of the original PDU
                // This maintains PDU type compatibility without any expensive cloning
                // and ensures no sensitive data is sent during discovery

                // Create direct empty PDU of same type as originalPdu
                discoveryInnerPdu = CreateEmptyPduOfSameType(originalScope);
            }
            else
            {
                discoveryInnerPdu = new GetRequestPdu
                {
                    RequestId = Random.Shared.Next(),
                    VariableBindings = new VarBindList()
                };
            }

            var discoveryPdu = new Scope
            {
                ContextEngineId = Memory<byte>.Empty,
                ContextName = string.Empty,
                Pdu = discoveryInnerPdu
            };

            var mpModel = GetMessageProcessingModel(VersionCode.V3);

            // Prepare discovery message
            if (mpModel.TryPrepareOutgoingMessage(
                discoveryTarget,
                discoveryPdu,
                ReadOnlyMemory<byte>.Empty,
                out int pduHandle,
                out ISnmpMessage? outgoingMessage,
                out MessageProcessingResult result,
                Memory<byte>.Empty,
                true))
            {
                _logger?.LogDebug("Discovery message prepared successfully");

                // Send discovery message
                _ = await transport.SendAsync(
                    outgoingMessage!.Encode(),
                    targetAddress,
                    cancellationToken);

                // Receive and process response
                var incomingData = await transport.ReceiveAsync(
                    targetAddress,
                    cancellationToken);

                // Try to decode the message to extract engine parameters
                try
                {
                    var reader = new AsnReader(incomingData, AsnEncodingRules.BER);
                    var v3Response = SnmpV3Message.ReadFrom(reader);

                    _logger?.LogDebug("Engine discovery response processed successfully: EngineID={EngineIdLength} bytes, Boots={EngineBoots}, Time={EngineTime}",
                        v3Response.SecurityParameters.EngineId.Length,
                        v3Response.SecurityParameters.EngineBoots,
                        v3Response.SecurityParameters.EngineTime);

                    return new EngineDiscoveryResult(
                        v3Response.SecurityParameters.EngineId,
                        v3Response.SecurityParameters.EngineBoots,
                        v3Response.SecurityParameters.EngineTime);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to process engine discovery response from {TargetAddress}", targetAddress);
                    throw new SnmpMessageProcessingException($"Failed to process engine discovery response from {targetAddress}", ex);
                }
            }
            else
            {
                _logger?.LogError("Failed to prepare engine discovery message: {ProcessingResult}", result);
                throw new SnmpMessageProcessingException($"Failed to prepare engine discovery message for {targetAddress}", result);
            }
        }

        /// <summary>
        /// Creates an empty PDU of the same type as the provided PDU.
        /// This avoids expensive cloning while maintaining PDU type compatibility.
        /// </summary>
        /// <param name="originalPdu">The PDU to get the type from</param>
        /// <returns>New empty PDU of the same type</returns>
        private Pdu CreateEmptyPduOfSameType(IScope originalPdu)
        {
            Pdu newPdu = originalPdu switch
            {
                GetRequestPdu => new GetRequestPdu(),
                SetRequestPdu => new SetRequestPdu(),
                GetNextRequestPdu => new GetNextRequestPdu(),
                InformRequestPdu => new InformRequestPdu(),
                // Add more PDU types as needed
                _ => new GetRequestPdu() // Default to GetRequestPdu
            };

            newPdu.RequestId = Random.Shared.Next();
            newPdu.VariableBindings = new VarBindList();
            return newPdu;
        }

        private record EngineDiscoveryResult(
            ReadOnlyMemory<byte> EngineId,
            int EngineBoots,
            int EngineTime);
    }
}
