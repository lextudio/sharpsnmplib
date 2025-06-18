// Messenger class.
// Copyright (C) 2009-2010 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/3/29
 * Time: 17:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using Lextm.SharpSnmpLib.Security;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Messenger class contains all static helper methods you need to send out SNMP messages.
    /// Static methods in Manager or Agent class will be removed in the future.
    /// </summary>
    /// <remarks>
    /// SNMP v3 is not supported in many methods of this class. Please use <see cref="ISnmpMessage" /> derived classes directly
    /// if you want to do v3 operations.
    /// </remarks>
    public static partial class Messenger
    {
        /// <summary>
        /// RFC 3416 (3.)
        /// </summary>
        private static readonly Lazy<NumberGenerator> RequestCounterFullRange = new(() => new NumberGenerator(int.MinValue, int.MaxValue));
        private static readonly Lazy<NumberGenerator> RequestCounterPositive = new(() => new NumberGenerator(0, int.MaxValue));

        private static NumberGenerator? requestCounter;

        /// <summary>
        /// The universal counter for request IDs and other IDs.
        /// </summary>
        public static NumberGenerator RequestCounter
        {
            get
            {
                return requestCounter ?? (requestCounter = UseFullRange ? RequestCounterFullRange.Value : RequestCounterPositive.Value);
            }

            set
            {
                requestCounter = value;
            }
        }

        /// <summary>
        /// A flag to control request ID range.
        /// </summary>
        /// <remarks>Default is <code>true</code>.
        /// Should be set to <code>false</code> when SNMP devices might not support negative request ID values.
        /// </remarks>
        public static bool UseFullRange { get; set; } = true;

        /// <summary>
        /// RFC 3412 (6.)
        /// </summary>
        private static readonly NumberGenerator MessageCounter = new(0, int.MaxValue);
        private static readonly ObjectIdentifier IdUnsupportedSecurityLevel = new(new uint[] { 1, 3, 6, 1, 6, 3, 15, 1, 1, 1, 0 });
        private static readonly ObjectIdentifier IdNotInTimeWindow = new(new uint[] { 1, 3, 6, 1, 6, 3, 15, 1, 1, 2, 0 });
        private static readonly ObjectIdentifier IdUnknownSecurityName = new(new uint[] { 1, 3, 6, 1, 6, 3, 15, 1, 1, 3, 0 });
        private static readonly ObjectIdentifier IdUnknownEngineId = new(new uint[] { 1, 3, 6, 1, 6, 3, 15, 1, 1, 4, 0 });
        private static readonly ObjectIdentifier IdAuthenticationFailure = new(new uint[] { 1, 3, 6, 1, 6, 3, 15, 1, 1, 5, 0 });
        private static readonly ObjectIdentifier IdDecryptionError = new(new uint[] { 1, 3, 6, 1, 6, 3, 15, 1, 1, 6, 0 });

        #region async methods

        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("GetAsync is incompatible with trimming.")]
#endif
        public static async Task<IList<Variable>> GetAsync(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            var message = new GetRequestMessage(RequestCounter.NextId, version, community, variables);
            var response = await message.GetResponseAsync(endpoint).ConfigureAwait(false);
            var pdu = response.Pdu();
            if (pdu.ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return pdu.Variables;
        }

        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("SetAsync is incompatible with trimming.")]
#endif
        public static async Task<IList<Variable>> SetAsync(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            var message = new SetRequestMessage(RequestCounter.NextId, version, community, variables);
            var response = await message.GetResponseAsync(endpoint).ConfigureAwait(false);
            var pdu = response.Pdu();
            if (pdu.ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return pdu.Variables;
        }

        /// <summary>
        /// Walks (based on GET NEXT).
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="mode">Walk mode.</param>
        /// <returns>
        /// Returns row count if the OID is a table. Otherwise this value is meaningless.
        /// </returns>
        /// <remarks>This method only supports SNMP v1 and v2c.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("WalkAsync is incompatible with trimming.")]
#endif
        public static async Task<int> WalkAsync(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, WalkMode mode)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = 0;
            var tableV = new Variable(table);
            Variable? seed;
            var next = tableV;
            var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
            var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
            Tuple<bool, Variable?> data = new(false, next);
            do
            {
                seed = data.Item2;

                if (version == VersionCode.V2 && seed?.Data.TypeCode == SnmpType.EndOfMibView)
                {
                    break;
                }

                if (seed == tableV)
                {
                    data = await HasNextAsync(version, endpoint, community, seed).ConfigureAwait(false);
                    continue;
                }

                if (seed == null)
                {
                    break;
                }

                if (mode == WalkMode.WithinSubtree && !seed.Id.ToString().StartsWith(subTreeMask, StringComparison.Ordinal))
                {
                    // not in sub tree
                    break;
                }

                list.Add(seed);
                if (seed.Id.ToString().StartsWith(rowMask, StringComparison.Ordinal))
                {
                    result++;
                }

                data = await HasNextAsync(version, endpoint, community, seed).ConfigureAwait(false);
            }
            while (data.Item1);
            return result;
        }

        /// <summary>
        /// Determines whether the specified seed has next item.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="community">The community.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>
        ///     <c>true</c> if the specified seed has next item; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method only supports SNMP v1 and v2c.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("HasNextAsync is incompatible with trimming.")]
#endif
        private static async Task<Tuple<bool, Variable?>> HasNextAsync(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed)
        {
            if (seed == null)
            {
                throw new ArgumentNullException(nameof(seed));
            }

            var variables = new List<Variable> { new(seed.Id) };
            var message = new GetNextRequestMessage(
                RequestCounter.NextId,
                version,
                community,
                variables);

            var response = await message.GetResponseAsync(endpoint).ConfigureAwait(false);
            var pdu = response.Pdu();
            var errorFound = pdu.ErrorStatus.ToErrorCode() == ErrorCode.NoSuchName;
            return new Tuple<bool, Variable?>(!errorFound, errorFound ? null : pdu.Variables[0]);
        }

        /// <summary>
        /// Walks (based on GET BULK).
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name (v2c) or user name (v3).</param>
        /// <param name="contextName">Context name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="mode">Walk mode.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        /// <returns>Returns row count if the OID is a table. Otherwise this value is meaningless.</returns>
        /// <remarks>This method only supports SNMP v2c and v3.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("BulkWalkAsync is incompatible with trimming.")]
#endif
        public static async Task<int> BulkWalkAsync(VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, ObjectIdentifier table, IList<Variable> list, int maxRepetitions, WalkMode mode, IPrivacyProvider privacy, ISnmpMessage report)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var tableV = new Variable(table);
            var seed = tableV;
            var result = 0;
            var message = report;
            var data = await BulkHasNextAsync(version, endpoint, community, contextName, seed, maxRepetitions, privacy, message).ConfigureAwait(false);
            var next = data.Item2;
            message = data.Item3;
            while (data.Item1)
            {
                var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
                var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
                foreach (var v in next)
                {
                    var id = v.Id.ToString();
                    if (v.Data.TypeCode == SnmpType.EndOfMibView)
                    {
                        goto end;
                    }

                    if (mode == WalkMode.WithinSubtree && !id.StartsWith(subTreeMask, StringComparison.Ordinal))
                    {
                        // not in sub tree
                        goto end;
                    }

                    list.Add(v);
                    if (id.StartsWith(rowMask, StringComparison.Ordinal))
                    {
                        result++;
                    }
                }

                seed = next[next.Count - 1];
                data = await BulkHasNextAsync(version, endpoint, community, contextName, seed, maxRepetitions, privacy, message).ConfigureAwait(false);
                next = data.Item2;
                message = data.Item3;
            }

        end:
            return result;
        }

        /// <summary>
        /// Sends a TRAP v1 message.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        /// <param name="agent">Agent.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="generic">Generic code.</param>
        /// <param name="specific">Specific code.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <remarks>This method only supports SNMP v1.</remarks>
        [CLSCompliant(false)]
        public static async Task SendTrapV1Async(EndPoint receiver, IPAddress agent, OctetString community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint timestamp, IList<Variable> variables)
        {
            var message = new TrapV1Message(VersionCode.V1, agent, community, enterprise, generic, specific, timestamp, variables);
            await message.SendAsync(receiver).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends TRAP v2 message.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="requestId">Request ID.</param>
        /// <remarks>This method only supports SNMP v2c.</remarks>
        [CLSCompliant(false)]
        public static async Task SendTrapV2Async(int requestId, VersionCode version, EndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
        {
            if (version != VersionCode.V2)
            {
                throw new NotSupportedException("Only SNMP v2c is supported");
            }

            var message = new TrapV2Message(requestId, version, community, enterprise, timestamp, variables);
            await message.SendAsync(receiver).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends INFORM message.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="contextName">Context name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        /// <remarks>This method supports SNMP v2c and v3.</remarks>
        [CLSCompliant(false)]
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("SendInformAsync is incompatible with trimming.")]
#endif
        public static async Task SendInformAsync(int requestId, VersionCode version, IPEndPoint receiver, OctetString community, OctetString contextName, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, IPrivacyProvider privacy, ISnmpMessage report)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            if (contextName == null)
            {
                throw new ArgumentNullException(nameof(contextName));
            }

            if (enterprise == null)
            {
                throw new ArgumentNullException(nameof(enterprise));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (version == VersionCode.V1)
            {
                throw new NotSupportedException("SNMP v1 is not supported");
            }

            if (version == VersionCode.V3 && privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            if (version == VersionCode.V3 && report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            var message = version == VersionCode.V3
                                    ? new InformRequestMessage(
                                          version,
                                          MessageCounter.NextId,
                                          requestId,
                                          community,
                                          contextName,
                                          enterprise,
                                          timestamp,
                                          variables,
                                          privacy,
                                          MaxMessageSize,
                                          report)
                                    : new InformRequestMessage(
                                          requestId,
                                          version,
                                          community,
                                          enterprise,
                                          timestamp,
                                          variables);

            var response = await message.GetResponseAsync(receiver).ConfigureAwait(false);
            if (response.Pdu().ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    receiver.Address,
                    response);
            }
        }

        /// <summary>
        /// Determines whether the specified seed has next item.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="community">The community name (v2c) or user name (v3).</param>
        /// <param name="contextName">The context name.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        /// <returns>
        /// <c>true</c> if the specified seed has next item; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method supports SNMP v2c and v3.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("BulkHasNextAsync is incompatible with trimming.")]
#endif
        private static async Task<Tuple<bool, IList<Variable>, ISnmpMessage>> BulkHasNextAsync(VersionCode version, IPEndPoint receiver, OctetString community, OctetString contextName, Variable seed, int maxRepetitions, IPrivacyProvider privacy, ISnmpMessage report)
        {
            // TODO: report should be updated with latest message from agent.
            if (version == VersionCode.V1)
            {
                throw new NotSupportedException("SNMP v1 is not supported");
            }

            var variables = new List<Variable> { new(seed.Id) };
            var request = version == VersionCode.V3
                                                ? new GetBulkRequestMessage(
                                                      version,
                                                      MessageCounter.NextId,
                                                      RequestCounter.NextId,
                                                      community,
                                                      contextName,
                                                      0,
                                                      maxRepetitions,
                                                      variables,
                                                      privacy,
                                                      MaxMessageSize,
                                                      report)
                                                : new GetBulkRequestMessage(
                                                      RequestCounter.NextId,
                                                      version,
                                                      community,
                                                      0,
                                                      maxRepetitions,
                                                      variables);
            var reply = await request.GetResponseAsync(receiver).ConfigureAwait(false);
            if (reply is ReportMessage)
            {
                if (reply.Pdu().Variables.Count == 0)
                {
                    // TODO: whether it is good to return?
                    return new Tuple<bool, IList<Variable>, ISnmpMessage>(false, new List<Variable>(0), report);
                }

                var id = reply.Pdu().Variables[0].Id;
                if (id != IdNotInTimeWindow)
                {
                    return new Tuple<bool, IList<Variable>, ISnmpMessage>(false, new List<Variable>(0), report);
                }

                // according to RFC 3414, send a second request to sync time.
                request = new GetBulkRequestMessage(
                    version,
                    MessageCounter.NextId,
                    RequestCounter.NextId,
                    community,
                    contextName,
                    0,
                    maxRepetitions,
                    variables,
                    privacy,
                    MaxMessageSize,
                    reply);
                reply = await request.GetResponseAsync(receiver).ConfigureAwait(false);
            }
            else if (reply.Pdu().ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    receiver.Address,
                    reply);
            }

            var next = reply.Pdu().Variables;
            return new Tuple<bool, IList<Variable>, ISnmpMessage>(next.Count != 0, next, request);
        }

        #endregion

        #region sync methods
        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <returns></returns>
        /// <remarks>This method supports SNMP v1 and v2c.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("Get is incompatible with trimming.")]
#endif
        public static IList<Variable> Get(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            var message = new GetRequestMessage(RequestCounter.NextId, version, community, variables);
            var response = message.GetResponse(timeout, endpoint);
            var pdu = response.Pdu();
            if (pdu.ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return pdu.Variables;
        }

        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <returns></returns>
        /// <remarks>This method supports SNMP v1 and v2c.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("Set is incompatible with trimming.")]
#endif
        public static IList<Variable> Set(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            var message = new SetRequestMessage(RequestCounter.NextId, version, community, variables);
            var response = message.GetResponse(timeout, endpoint);
            var pdu = response.Pdu();
            if (pdu.ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return pdu.Variables;
        }

        /// <summary>
        /// Walks (based on GET NEXT).
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="mode">Walk mode.</param>
        /// <returns>
        /// Returns row count if the OID is a table. Otherwise this value is meaningless.
        /// </returns>
        /// <remarks>This method supports SNMP v1 and v2c.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("Walk is incompatible with trimming.")]
#endif
        public static int Walk(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, int timeout, WalkMode mode)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = 0;
            var tableV = new Variable(table);
            Variable? seed;
            var next = tableV;
            var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
            var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
            do
            {
                seed = next;

                if (version == VersionCode.V2 && seed?.Data.TypeCode == SnmpType.EndOfMibView)
                {
                    break;
                }
                
                if (seed == tableV)
                {
                    continue;
                }

                if (seed == null)
                {
                    break;
                }

                if (mode == WalkMode.WithinSubtree && !seed.Id.ToString().StartsWith(subTreeMask, StringComparison.Ordinal))
                {
                    // not in sub tree
                    break;
                }

                list.Add(seed);
                if (seed.Id.ToString().StartsWith(rowMask, StringComparison.Ordinal))
                {
                    result++;
                }
            }
            while (HasNext(version, endpoint, community, seed, timeout, out next));
            return result;
        }

        /// <summary>
        /// Determines whether the specified seed has next item.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="community">The community.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="next">The next.</param>
        /// <returns>
        ///     <c>true</c> if the specified seed has next item; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method supports SNMP v1 and v2c.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("HasNext is incompatible with trimming.")]
#endif
        private static bool HasNext(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed, int timeout, out Variable? next)
        {
            if (seed == null)
            {
                throw new ArgumentNullException(nameof(seed));
            }

            var variables = new List<Variable> { new(seed.Id) };
            var message = new GetNextRequestMessage(
                RequestCounter.NextId,
                version,
                community,
                variables);

            var response = message.GetResponse(timeout, endpoint);
            var pdu = response.Pdu();
            var errorFound = pdu.ErrorStatus.ToErrorCode() == ErrorCode.NoSuchName;
            next = errorFound ? null : pdu.Variables[0];
            return !errorFound;
        }

        /// <summary>
        /// Walks (based on GET BULK).
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name (v2c) or user name (v3).</param>
        /// <param name="contextName">Context name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="mode">Walk mode.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        /// <returns>Returns row count if the OID is a table. Otherwise this value is meaningless.</returns>
        /// <remarks>This method supports SNMP v2c and v3.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("BulkWalk is incompatible with trimming.")]
#endif
        public static int BulkWalk(VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, ObjectIdentifier table, IList<Variable> list, int timeout, int maxRepetitions, WalkMode mode, IPrivacyProvider? privacy, ISnmpMessage? report)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (version == VersionCode.V1)
            {
                throw new NotSupportedException("SNMP v1 is not supported");
            }

            var tableV = new Variable(table);
            var seed = tableV;
            var result = 0;
            var message = report;
            while (BulkHasNext(version, endpoint, community, contextName, seed, timeout, maxRepetitions, out var next, privacy, ref message))
            {
                var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
                var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
                foreach (var v in next)
                {
                    var id = v.Id.ToString();
                    if (v.Data.TypeCode == SnmpType.EndOfMibView)
                    {
                        goto end;
                    }

                    if (mode == WalkMode.WithinSubtree && !id.StartsWith(subTreeMask, StringComparison.Ordinal))
                    {
                        // not in sub tree
                        goto end;
                    }

                    list.Add(v);
                    if (id.StartsWith(rowMask, StringComparison.Ordinal))
                    {
                        result++;
                    }
                }

                seed = next[next.Count - 1];
            }

        end:
            return result;
        }

        /// <summary>
        /// Sends a TRAP v1 message.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        /// <param name="agent">Agent.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="generic">Generic code.</param>
        /// <param name="specific">Specific code.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <remarks>This method supports SNMP v1.</remarks>
        [CLSCompliant(false)]
        public static void SendTrapV1(EndPoint receiver, IPAddress agent, OctetString community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint timestamp, IList<Variable> variables)
        {
            var message = new TrapV1Message(VersionCode.V1, agent, community, enterprise, generic, specific, timestamp, variables);
            message.Send(receiver);
        }

        /// <summary>
        /// Sends TRAP v2 message.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="requestId">Request ID.</param>
        /// <remarks>This method supports SNMP v2c.</remarks>
        [CLSCompliant(false)]
        public static void SendTrapV2(int requestId, VersionCode version, EndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
        {
            if (version != VersionCode.V2)
            {
                throw new NotSupportedException("Only SNMP v2c is supported");
            }

            var message = new TrapV2Message(requestId, version, community, enterprise, timestamp, variables);
            message.Send(receiver);
        }

        /// <summary>
        /// Sends INFORM message.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name (v2c) or user name (v3).</param>
        /// <param name="contextName">Context name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        /// <remarks>This method supports SNMP v2c and v3.</remarks>
        [CLSCompliant(false)]
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("SendInform is incompatible with trimming.")]
#endif
        public static void SendInform(int requestId, VersionCode version, IPEndPoint receiver, OctetString community, OctetString contextName, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, int timeout, IPrivacyProvider privacy, ISnmpMessage report)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            if (contextName == null)
            {
                throw new ArgumentException(nameof(contextName));
            }

            if (enterprise == null)
            {
                throw new ArgumentNullException(nameof(enterprise));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (version == VersionCode.V1)
            {
                throw new NotSupportedException("SNMP v1 is not supported");
            }

            if (version == VersionCode.V3 && privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            if (version == VersionCode.V3 && report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            var message = version == VersionCode.V3
                                    ? new InformRequestMessage(
                                          version,
                                          MessageCounter.NextId,
                                          requestId,
                                          community,
                                          contextName,
                                          enterprise,
                                          timestamp,
                                          variables,
                                          privacy,
                                          MaxMessageSize,
                                          report)
                                    : new InformRequestMessage(
                                          requestId,
                                          version,
                                          community,
                                          enterprise,
                                          timestamp,
                                          variables);

            var response = message.GetResponse(timeout, receiver);
            if (response.Pdu().ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    receiver.Address,
                    response);
            }
        }

        /// <summary>
        /// Determines whether the specified seed has next item.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="community">The community name (v2c) or user name (v3).</param>
        /// <param name="contextName">The context name.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="next">The next.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        /// <returns>
        /// <c>true</c> if the specified seed has next item; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This method supports SNMP v2c and v3.</remarks>
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("BulkHasNext is incompatible with trimming.")]
#endif
        private static bool BulkHasNext(VersionCode version, IPEndPoint receiver, OctetString community, OctetString contextName, Variable seed, int timeout, int maxRepetitions, out IList<Variable> next, IPrivacyProvider? privacy, ref ISnmpMessage? report)
        {
            if (version == VersionCode.V1)
            {
                throw new NotSupportedException("SNMP v1 is not supported");
            }

            var variables = new List<Variable> { new(seed.Id) };
            var request = version == VersionCode.V3
                                                ? new GetBulkRequestMessage(
                                                      version,
                                                      MessageCounter.NextId,
                                                      RequestCounter.NextId,
                                                      community,
                                                      contextName,
                                                      0,
                                                      maxRepetitions,
                                                      variables,
                                                      privacy!,
                                                      MaxMessageSize,
                                                      report!)
                                                : new GetBulkRequestMessage(
                                                      RequestCounter.NextId,
                                                      version,
                                                      community,
                                                      0,
                                                      maxRepetitions,
                                                      variables);
            var reply = request.GetResponse(timeout, receiver);
            if (reply is ReportMessage)
            {
                if (reply.Pdu().Variables.Count == 0)
                {
                    // TODO: whether it is good to return?
                    next = new List<Variable>(0);
                    return false;
                }

                var id = reply.Pdu().Variables[0].Id;
                if (id != IdNotInTimeWindow)
                {
                    next = new List<Variable>(0);
                    return false;
                }

                // according to RFC 3414, send a second request to sync time.
                request = new GetBulkRequestMessage(
                    version,
                    MessageCounter.NextId,
                    RequestCounter.NextId,
                    community,
                    contextName,
                    0,
                    maxRepetitions,
                    variables,
                    privacy!, // TODO: how to ensure this is not null?
                    MaxMessageSize,
                    reply);
                reply = request.GetResponse(timeout, receiver);
            }
            else if (reply.Pdu().ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    receiver.Address,
                    reply);
            }

            next = reply.Pdu().Variables;
            report = request;
            return next.Count != 0;
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <returns></returns>
        /// <remarks>This method supports SNMP v2c and v3.</remarks>
        [CLSCompliant(false)]
#if NET6_0_OR_GREATER
        [RequiresUnreferencedCode("GetTable is incompatible with trimming.")]
#endif
        [Obsolete("This method only works for a few scenarios. Might be replaced by new methods in the future. If it does not work for you, parse WALK result on your own.")]
        public static Variable[,] GetTable(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, int timeout, int maxRepetitions)
        {
            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            IList<Variable> list = new List<Variable>();
            var rows = version == VersionCode.V1 ? Walk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree) : BulkWalk(version, endpoint, community, OctetString.Empty, table, list, timeout, maxRepetitions, WalkMode.WithinSubtree, null, null);

            if (rows == 0)
            {
                return new Variable[0, 0];
            }

            var cols = list.Count / rows;
            var k = 0;
            var result = new Variable[rows, cols];

            for (var j = 0; j < cols; j++)
            {
                for (var i = 0; i < rows; i++)
                {
                    result[i, j] = list[k];
                    k++;
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Gets the request counter.
        /// </summary>
        /// <value>The request counter.</value>
        public static int NextRequestId => RequestCounter.NextId;

        /// <summary>
        /// Gets the message counter.
        /// </summary>
        /// <value>The message counter.</value>
        public static int NextMessageId => MessageCounter.NextId;

        /// <summary>
        /// Max message size used in #SNMP. 
        /// </summary>
        /// <remarks>
        /// You can use any value for your own application. 
        /// Also this value may be changed in #SNMP in future releases.
        /// </remarks>
        public static int MaxMessageSize { get; set; } = Header.MaxMessageSize;

        /// <summary>
        /// If the privacy module returns failure, then the message can
        /// not be processed, so the usmStatsDecryptionErrors counter is
        /// incremented and an error indication (decryptionError) together
        /// with the OID and value of the incremented counter is returned
        /// to the calling module.
        /// </summary>
        public static ObjectIdentifier DecryptionError => IdDecryptionError;

        /// <summary>
        /// If the authentication module returns failure, then the message
        /// cannot be trusted, so the usmStatsWrongDigests counter is
        /// incremented and an error indication (authenticationFailure)
        /// together with the OID and value of the incremented counter is
        /// returned to the calling module.
        /// </summary>
        public static ObjectIdentifier AuthenticationFailure => IdAuthenticationFailure;

        /// <summary>
        /// If the value of the msgAuthoritativeEngineID field in the
        /// securityParameters is unknown then
        /// the usmStatsUnknownEngineIDs counter is incremented, and an
        /// error indication (unknownEngineID) together with the OID and
        /// value of the incremented counter is returned to the calling
        /// module.
        /// </summary>
        public static ObjectIdentifier UnknownEngineId => IdUnknownEngineId;

        /// <summary>
        /// Information about the value of the msgUserName and
        /// msgAuthoritativeEngineID fields is extracted from the Local
        /// Configuration Datastore (LCD, usmUserTable).  If no information
        /// is available for the user, then the usmStatsUnknownUserNames
        /// counter is incremented and an error indication
        /// (unknownSecurityName) together with the OID and value of the
        /// incremented counter is returned to the calling module.
        /// </summary>
        public static ObjectIdentifier UnknownSecurityName => IdUnknownSecurityName;

        /// <summary>
        /// If the message is considered to be outside of the Time Window
        /// then the usmStatsNotInTimeWindows counter is incremented and
        /// an error indication (notInTimeWindow) together with the OID,
        /// the value of the incremented counter, and an indication that
        /// the error must be reported with a securityLevel of authNoPriv,
        /// is returned to the calling module
        /// </summary>
        public static ObjectIdentifier NotInTimeWindow => IdNotInTimeWindow;

        /// <summary>
        /// If the information about the user indicates that it does not
        /// support the securityLevel requested by the caller, then the
        /// usmStatsUnsupportedSecLevels counter is incremented and an error
        /// indication (unsupportedSecurityLevel) together with the OID and
        /// value of the incremented counter is returned to the calling
        /// module.
        /// </summary>
        public static ObjectIdentifier UnsupportedSecurityLevel => IdUnsupportedSecurityLevel;

        /// <summary>
        /// Returns a new discovery request.
        /// </summary>
        /// <param name="type">Message type.</param>
        /// <param name="contextName">The optional context name.</param>
        /// <returns></returns>
        public static Discovery GetNextDiscovery(SnmpType type)
        {
            return GetNextDiscovery(type, OctetString.Empty);
        }

        /// <summary>
        /// Returns a new discovery request.
        /// </summary>
        /// <param name="type">Message type.</param>
        /// <param name="contextName">The optional context name.</param>
        /// <returns></returns>
        public static Discovery GetNextDiscovery(SnmpType type, OctetString contextName)
        {
            return new Discovery(NextMessageId, NextRequestId, MaxMessageSize, type, contextName);
        }

        /// <summary>
        /// Returns error message for the specific <see cref="ObjectIdentifier"/>.
        /// </summary>
        /// <param name="id">The OID.</param>
        /// <returns>Error message.</returns>
        public static string GetErrorMessage(this ObjectIdentifier id)
        {
            if (id == IdUnsupportedSecurityLevel)
            {
                return "unsupported security level";
            }

            if (id == IdNotInTimeWindow)
            {
                return "not in time window";
            }

            if (id == IdUnknownSecurityName)
            {
                return "unknown security name";
            }

            if (id == IdUnknownEngineId)
            {
                return "unknown engine ID";
            }

            if (id == IdAuthenticationFailure)
            {
                return "authentication failure";
            }

            if (id == IdDecryptionError)
            {
                return "decryption error";
            }

            return "unknown error";
        }
    }
}
