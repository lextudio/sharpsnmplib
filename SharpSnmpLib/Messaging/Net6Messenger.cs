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
using System.Threading;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    #if NET6_0
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
        #region async methods

        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public static async Task<IList<Variable>> GetAsync(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, CancellationToken token)
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

            var message = new GetRequestMessage(Messenger.NextRequestId, version, community, variables);
            var response = await message.GetResponseAsync(endpoint, token).ConfigureAwait(false);
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
        public static async Task<IList<Variable>> SetAsync(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, CancellationToken token)
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

            var message = new SetRequestMessage(Messenger.NextRequestId, version, community, variables);
            var response = await message.GetResponseAsync(endpoint, token).ConfigureAwait(false);
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
        public static async Task<int> WalkAsync(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, WalkMode mode, CancellationToken token)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = 0;
            var tableV = new Variable(table);
            Variable seed;
            var next = tableV;
            var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
            var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
            Tuple<bool, Variable> data = new Tuple<bool, Variable>(false, next);
            do
            {
                seed = data.Item2;
                if (seed == tableV)
                {
                    data = await HasNextAsync(version, endpoint, community, seed, token).ConfigureAwait(false);
                    continue;
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

                data = await HasNextAsync(version, endpoint, community, seed, token).ConfigureAwait(false);
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
        private static async Task<Tuple<bool, Variable>> HasNextAsync(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed, CancellationToken token)
        {
            if (seed == null)
            {
                throw new ArgumentNullException(nameof(seed));
            }

            var variables = new List<Variable> { new Variable(seed.Id) };
            var message = new GetNextRequestMessage(
                Messenger.NextRequestId,
                version,
                community,
                variables);

            var response = await message.GetResponseAsync(endpoint, token).ConfigureAwait(false);
            var pdu = response.Pdu();
            var errorFound = pdu.ErrorStatus.ToErrorCode() == ErrorCode.NoSuchName;
            return new Tuple<bool, Variable>(!errorFound, errorFound ? null : pdu.Variables[0]);
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
        public static async Task<int> BulkWalkAsync(VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, ObjectIdentifier table, IList<Variable> list, int maxRepetitions, WalkMode mode, IPrivacyProvider privacy, ISnmpMessage report, CancellationToken token)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var tableV = new Variable(table);
            var seed = tableV;
            var result = 0;
            var message = report;
            var data = await BulkHasNextAsync(version, endpoint, community, contextName, seed, maxRepetitions, privacy, message, token).ConfigureAwait(false);
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
                data = await BulkHasNextAsync(version, endpoint, community, contextName, seed, maxRepetitions, privacy, message, token).ConfigureAwait(false);
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
        public static async Task SendTrapV1Async(EndPoint receiver, IPAddress agent, OctetString community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint timestamp, IList<Variable> variables, CancellationToken token)
        {
            var message = new TrapV1Message(VersionCode.V1, agent, community, enterprise, generic, specific, timestamp, variables);
            await message.SendAsync(receiver, token).ConfigureAwait(false);
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
        public static async Task SendTrapV2Async(int requestId, VersionCode version, EndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, CancellationToken token)
        {
            if (version != VersionCode.V2)
            {
                throw new NotSupportedException("Only SNMP v2c is supported");
            }

            var message = new TrapV2Message(requestId, version, community, enterprise, timestamp, variables);
            await message.SendAsync(receiver, token).ConfigureAwait(false);
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
        public static async Task SendInformAsync(int requestId, VersionCode version, IPEndPoint receiver, OctetString community, OctetString contextName, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, IPrivacyProvider privacy, ISnmpMessage report, CancellationToken token)
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
                                          Messenger.NextMessageId,
                                          requestId,
                                          community,
                                          contextName,
                                          enterprise,
                                          timestamp,
                                          variables,
                                          privacy,
                                          Messenger.MaxMessageSize,
                                          report)
                                    : new InformRequestMessage(
                                          requestId,
                                          version,
                                          community,
                                          enterprise,
                                          timestamp,
                                          variables);

            var response = await message.GetResponseAsync(receiver, token).ConfigureAwait(false);
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
        private static async Task<Tuple<bool, IList<Variable>, ISnmpMessage>> BulkHasNextAsync(VersionCode version, IPEndPoint receiver, OctetString community, OctetString contextName, Variable seed, int maxRepetitions, IPrivacyProvider privacy, ISnmpMessage report, CancellationToken token)
        {
            // TODO: report should be updated with latest message from agent.
            if (version == VersionCode.V1)
            {
                throw new NotSupportedException("SNMP v1 is not supported");
            }

            var variables = new List<Variable> { new Variable(seed.Id) };
            var request = version == VersionCode.V3
                                                ? new GetBulkRequestMessage(
                                                      version,
                                                      Messenger.NextMessageId,
                                                      Messenger.NextRequestId,
                                                      community,
                                                      contextName,
                                                      0,
                                                      maxRepetitions,
                                                      variables,
                                                      privacy,
                                                      Messenger.MaxMessageSize,
                                                      report)
                                                : new GetBulkRequestMessage(
                                                      Messenger.NextRequestId,
                                                      version,
                                                      community,
                                                      0,
                                                      maxRepetitions,
                                                      variables);
            var reply = await request.GetResponseAsync(receiver, token).ConfigureAwait(false);
            if (reply is ReportMessage)
            {
                if (reply.Pdu().Variables.Count == 0)
                {
                    // TODO: whether it is good to return?
                    return new Tuple<bool, IList<Variable>, ISnmpMessage>(false, new List<Variable>(0), report);
                }

                var id = reply.Pdu().Variables[0].Id;
                if (id != Messenger.NotInTimeWindow)
                {
                    // var error = id.GetErrorMessage();
                    // TODO: whether it is good to return?
                    return new Tuple<bool, IList<Variable>, ISnmpMessage>(false, new List<Variable>(0), report);
                }

                // according to RFC 3414, send a second request to sync time.
                request = new GetBulkRequestMessage(
                    version,
                    Messenger.NextMessageId,
                    Messenger.NextRequestId,
                    community,
                    contextName,
                    0,
                    maxRepetitions,
                    variables,
                    privacy,
                    Messenger.MaxMessageSize,
                    reply);
                reply = await request.GetResponseAsync(receiver, token).ConfigureAwait(false);
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
    }
    #endif
}
