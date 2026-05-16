using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using System.Formats.Asn1;
using System.Buffers;
using Microsoft.Extensions.Logging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Coordinates SNMP Protocol Data Unit (PDU) transmission and reception across all SNMP versions (v1, v2c, v3).
    /// </summary>
    /// <remarks>
    /// <para>
    /// SnmpDispatcher is the central component for SNMP communication, handling:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Protocol version detection and routing (SNMPv1, SNMPv2c, SNMPv3)</description></item>
    /// <item><description>Message preparation and encoding via version-specific models</description></item>
    /// <item><description>Transport abstraction for UDP communication</description></item>
    /// <item><description>Timeout management and response correlation by PDU handle</description></item>
    /// <item><description>SNMPv3 engine discovery and parameter caching (RFC 3414)</description></item>
    /// </list>
    /// <para>
    /// The dispatcher lazily loads and caches protocol models to avoid repeated instantiation.
    /// It enforces target-specific timeouts by creating linked cancellation tokens.
    /// </para>
    /// </remarks>
    public class SnmpDispatcher : ISnmpDispatcher
    {
        private readonly ILogger<SnmpDispatcher>? _logger;
        private IMessageProcessingModel? _v1MsgProcModel;
        private IMessageProcessingModel? _v2MsgProcModel;
        private IMessageProcessingModel? _v3UsmMsgProcModel;

        /// <summary>
        /// Initializes a new instance of <see cref="SnmpDispatcher"/>.
        /// </summary>
        /// <param name="logger">Optional logger for diagnostic output. If <c>null</c>, logging is disabled.</param>
        /// <remarks>
        /// Protocol models (v1, v2c, v3) are created on-demand to minimize startup overhead.
        /// </remarks>
        public SnmpDispatcher(ILogger<SnmpDispatcher>? logger = null)
        {
            _logger = logger;
            _logger?.LogDebug("SnmpDispatcher initialized");
        }

        /// <summary>
        /// Gets or creates the message processing model for the specified SNMP protocol version.
        /// </summary>
        /// <param name="version">The SNMP protocol version (v1, v2c, or v3).</param>
        /// <returns>The lazy-initialized message processing model for the specified version.</returns>
        /// <exception cref="NotImplementedException">Thrown when <paramref name="version"/> is not supported.</exception>
        /// <remarks>
        /// Models are cached to avoid repeated instantiation across multiple PDU operations.
        /// </remarks>
        private IMessageProcessingModel GetMessageProcessingModel(VersionCode version)
        {
            return version switch
            {
                VersionCode.V1 => GetOrCreateV1Model(),
                VersionCode.V2 => GetOrCreateV2Model(),
                VersionCode.V3 => GetOrCreateV3Model(),
                _ => throw new NotImplementedException(),
            };
        }

        private IMessageProcessingModel GetOrCreateV1Model()
        {
            _v1MsgProcModel ??= new V1MessageProcessingModel();
            return _v1MsgProcModel;
        }

        private IMessageProcessingModel GetOrCreateV2Model()
        {
            _v2MsgProcModel ??= new V2MessageProcessingModel();
            return _v2MsgProcModel;
        }

        private IMessageProcessingModel GetOrCreateV3Model()
        {
            _v3UsmMsgProcModel ??= new V3MessageProcessingModel();
            return _v3UsmMsgProcModel;
        }

        /// <summary>
        /// Sends an SNMP PDU to the target and optionally waits for the response.
        /// </summary>
        /// <param name="transport">The network transport layer (e.g., UDP) used to send/receive data.</param>
        /// <param name="target">The SNMP target containing protocol version, security, and timeout parameters.</param>
        /// <param name="targetAddress">The network address (IP:port) of the SNMP agent.</param>
        /// <param name="scope">The PDU wrapped in a scope object containing version-specific data.</param>
        /// <param name="expectResponse">If <c>true</c>, waits for and validates the response; if <c>false</c>, returns immediately.</param>
        /// <param name="cancellationToken">A cancellation token to abort the operation. The dispatcher also enforces the target's timeout.</param>
        /// <returns>The response scope containing the SNMP agent's reply, or the original scope if no response expected.</returns>
        /// <exception cref="SnmpTimeoutException">Thrown when the response is not received within the target's timeout period.</exception>
        /// <exception cref="SnmpMessageProcessingException">Thrown when message preparation, encoding, or processing fails.</exception>
        /// <remarks>
        /// <para>
        /// This method handles all protocol versions by delegating to version-specific processors.
        /// For SNMPv3, it performs engine discovery if the target lacks cached engine parameters.
        /// </para>
        /// <para>
        /// A linked cancellation token is created from both the target's timeout and the caller's cancellation token,
        /// ensuring the operation respects both time constraints.
        /// </para>
        /// </remarks>
        public ValueTask<IScope> SendPdu(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            IScope scope)
            => SendPdu(transport, target, targetAddress, scope, true, CancellationToken.None);

        public ValueTask<IScope> SendPdu(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            IScope scope,
            bool expectResponse)
            => SendPdu(transport, target, targetAddress, scope, expectResponse, CancellationToken.None);

        public async ValueTask<IScope> SendPdu(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            IScope scope,
            bool expectResponse,
            CancellationToken cancellationToken)
        {
            _logger?.LogDebug("Sending PDU to target {TargetAddress} with protocol version {ProtocolVersion}", targetAddress, target.ProtocolVersion);

            // IMPORTANT: enforce the target's timeout value
            using var timeoutCts = new CancellationTokenSource(target.Timeout);
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
                            out var _,
                            out var _,
                            out var _,
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
            catch (SnmpMessageProcessingException ex)
            {
                // Already logged with appropriate context in throw site, rethrow as-is
                _logger?.LogDebug(ex, "Rethrowing SnmpMessageProcessingException from SendPdu");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error occurred while sending PDU to {TargetAddress}", targetAddress);
                throw;
            }
        }

        /// <summary>
        /// Sends an SNMPv3 PDU with security parameters, performing engine discovery if needed.
        /// </summary>
        /// <param name="transport">The network transport layer for sending/receiving data.</param>
        /// <param name="target">The SNMPv3 target containing user security parameters.</param>
        /// <param name="targetAddress">The network address (IP:port) of the SNMPv3 agent.</param>
        /// <param name="scope">The PDU to send.</param>
        /// <param name="expectResponse">If <c>true</c>, waits for and validates the response.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The response scope or original scope if no response expected.</returns>
        /// <remarks>
        /// <para>
        /// This method implements RFC 3414 SNMPv3 engine discovery:
        /// </para>
        /// <list type="number">
        /// <item><description>Checks if the target has cached engine parameters (EngineId, EngineBoots, EngineTime)</description></item>
        /// <item><description>If parameters are missing, discovers them from the agent</description></item>
        /// <item><description>Caches discovered parameters in the target for future PDUs</description></item>
        /// <item><description>Prepares and sends the actual request with security parameters</description></item>
        /// </list>
        /// <para>
        /// If the response indicates engine time has changed (RFC 3414), it updates the cached parameters
        /// and recursively sends the PDU again with updated timing.
        /// </para>
        /// </remarks>
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
                                out var _,
                                out var _,
                                out var _,
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
            catch (SnmpMessageProcessingException ex)
            {
                // Already logged with appropriate context in throw site, rethrow as-is
                _logger?.LogDebug(ex, "Rethrowing SnmpMessageProcessingException from SendPduV3");
                throw;
            }
            catch (SnmpTimeoutException ex)
            {
                // Already logged with appropriate context in throw site, rethrow as-is
                _logger?.LogDebug(ex, "Rethrowing SnmpTimeoutException from SendPduV3");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error occurred during SNMPv3 operation with {TargetAddress}", targetAddress);
                throw;
            }
        }

        /// <summary>
        /// Discovers the SNMPv3 engine ID and timing parameters from the target agent.
        /// </summary>
        /// <param name="transport">The network transport layer.</param>
        /// <param name="target">The SNMPv3 target.</param>
        /// <param name="targetAddress">The agent's network address.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <param name="originalScope">Optional original PDU scope to use for discovery context; if null, a generic probe is sent.</param>
        /// <returns>An <see cref="EngineDiscoveryResult"/> containing the EngineId, EngineBoots, and EngineTime.</returns>
        /// <remarks>
        /// <para>
        /// Engine discovery is performed as per RFC 3414 Section 4.3:
        /// </para>
        /// <list type="number">
        /// <item><description>Sends an unauthenticated message to the agent's discovery port</description></item>
        /// <item><description>Receives the agent's engine parameters in the response</description></item>
        /// <item><description>Extracts and validates the engine ID, boots counter, and current time</description></item>
        /// <item><description>Caches these parameters for subsequent authenticated operations</description></item>
        /// </list>
        /// <para>
        /// If <paramref name="originalScope"/> is provided, it's used to discover with the actual operation context;
        /// otherwise, a generic probe request is sent. This helps align engine discovery with the operation type.
        /// </para>
        /// </remarks>
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
                out int _,
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

        private sealed record EngineDiscoveryResult(
            ReadOnlyMemory<byte> EngineId,
            int EngineBoots,
            int EngineTime);
    }
}
