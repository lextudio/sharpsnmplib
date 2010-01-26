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
using System.Net;
using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Messenger class contains all static helper methods you need to send out SNMP messages.
    /// Static methods in Manager or Agent class will be removed in the future.
    /// </summary>
    public static class Messenger
    {
        private static readonly IdGenerator RequestCounter = new IdGenerator();
        
        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns></returns>
        public static IList<Variable> Get(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            GetRequestMessage message = new GetRequestMessage(RequestCounter.NextId, version, community, variables);
            ISnmpMessage response = message.GetResponse(timeout, endpoint);
            if (response.Pdu.ErrorStatus.ToInt32() != 0)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return response.Pdu.Variables;
        }
        
        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns></returns>
        public static IList<Variable> Set(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            SetRequestMessage message = new SetRequestMessage(RequestCounter.NextId, version, community, variables);
            ISnmpMessage response = message.GetResponse(timeout, endpoint);
            if (response.Pdu.ErrorStatus.ToInt32() != 0)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return response.Pdu.Variables;
        }
        
        /// <summary>
        /// Walks.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="mode">Walk mode.</param>
        /// <returns>
        /// Returns row count if the OID is a table. Otherwise this value is meaningless.
        /// </returns>
        public static int Walk(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, int timeout, WalkMode mode)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            int result = 0;
            Variable tableV = new Variable(table);
            Variable seed;
            Variable next = tableV;

            do
            {
                seed = next;
                if (seed == tableV)
                {
                    continue;
                }

                if (mode == WalkMode.WithinSubtree && !seed.Id.ToString().StartsWith(table + ".", StringComparison.Ordinal))
                {
                    // not in sub tree
                    break;
                }

                list.Add(seed);
                if (seed.Id.ToString().StartsWith(table + ".1.1.", StringComparison.Ordinal))
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
        /// <param name="timeout">The timeout.</param>
        /// <param name="next">The next.</param>
        /// <returns>
        ///     <c>true</c> if the specified seed has next item; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "5#")]
        private static bool HasNext(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed, int timeout, out Variable next)
        {
            if (seed == null)
            {
                throw new ArgumentNullException("seed");
            }

            List<Variable> variables = new List<Variable>();
            variables.Add(new Variable(seed.Id));

            GetNextRequestMessage message = new GetNextRequestMessage(
                RequestCounter.NextId,
                version,
                community,
                variables);

            ISnmpMessage response = message.GetResponse(timeout, endpoint);
            bool errorFound = response.Pdu.ErrorStatus.ToErrorCode() == ErrorCode.NoSuchName;
            next = errorFound ? null : response.Pdu.Variables[0];
            return !errorFound;
        }
        
        /// <summary>
        /// Walks.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="mode">Walk mode.</param>
        /// <returns></returns>
        public static int BulkWalk(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, int timeout, int maxRepetitions, WalkMode mode)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            Variable tableV = new Variable(table);
            Variable seed = tableV;
            IList<Variable> next;
            int result = 0;
            while (BulkHasNext(version, endpoint, community, seed, timeout, maxRepetitions, out next))
            {
                foreach (Variable v in next)
                {
                    string id = v.Id.ToString();
                    if (v.Data.TypeCode == SnmpType.EndOfMibView)
                    {
                        goto end;
                    }

                    if (mode == WalkMode.WithinSubtree &&
                        !id.StartsWith(table + ".", StringComparison.Ordinal))
                    {
                        // not in sub tree
                        goto end;
                    }

                    list.Add(v);

                    if (id.StartsWith(table + ".1.1.", StringComparison.Ordinal))
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
        /// Determines whether the specified seed has next item.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="community">The community.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="next">The next.</param>
        /// <returns>
        ///     <c>true</c> if the specified seed has next item; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "5#")]
        private static bool BulkHasNext(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed, int timeout, int maxRepetitions, out IList<Variable> next)
        {
            if (version != VersionCode.V2)
            {
                throw new NotSupportedException("SNMP v1 and v3 is not supported");
            }

            List<Variable> variables = new List<Variable>();
            variables.Add(new Variable(seed.Id));

            GetBulkRequestMessage message = new GetBulkRequestMessage(
                RequestCounter.NextId,
                version,
                community,
                0,
                maxRepetitions,
                variables);

            ISnmpMessage response = message.GetResponse(timeout, endpoint);
            if (response.Pdu.ErrorStatus.ToInt32() != 0)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            next = response.Pdu.Variables;
            return next.Count != 0;
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
        [CLSCompliant(false)]
        public static void SendTrapV1(EndPoint receiver, IPAddress agent, OctetString community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint timestamp, IList<Variable> variables)
        {
            TrapV1Message message = new TrapV1Message(VersionCode.V1, agent, community, enterprise, generic, specific, timestamp, variables);
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
        [CLSCompliant(false)]
        public static void SendTrapV2(int requestId, VersionCode version, EndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
        {
            if (version == VersionCode.V1)
            {
                throw new ArgumentException("SNMP v1 is not support", "version");
            }

            TrapV2Message message = new TrapV2Message(requestId, version, community, enterprise, timestamp, variables);
            message.Send(receiver);
        }

        /// <summary>
        /// Sends INFORM message.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="timeout">Timeout.</param>
        [CLSCompliant(false)]
        public static void SendInform(int requestId, VersionCode version, IPEndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, int timeout)
        {
            InformRequestMessage message = new InformRequestMessage(requestId, version, community, enterprise, timestamp, variables);
            ISnmpMessage response = message.GetResponse(timeout, receiver);
            if (response.Pdu.ErrorStatus.ToInt32() != 0)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    receiver.Address,
                    response);
            }
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "ByDesign")]
        [CLSCompliant(false)]
        public static Variable[,] GetTable(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, int timeout, int maxRepetitions, IObjectRegistry registry)
        {
            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            bool canContinue = registry == null || registry.ValidateTable(table);
            if (!canContinue)
            {
                throw new ArgumentException("not a table OID: " + table);
            }
            
            IList<Variable> list = new List<Variable>();
            int rows = version == VersionCode.V1 ? Walk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree) : BulkWalk(version, endpoint, community, table, list, timeout, maxRepetitions, WalkMode.WithinSubtree);
            
            if (rows == 0)
            {
                return new Variable[0, 0];
            }

            int cols = list.Count / rows;
            int k = 0;
            Variable[,] result = new Variable[rows, cols];

            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    result[i, j] = list[k];
                    k++;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the request counter.
        /// </summary>
        /// <value>The request counter.</value>
        public static int NextId
        {
            get
            {
                return RequestCounter.NextId;
            }
        }
    }
}
