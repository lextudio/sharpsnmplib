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
using System.Net.Sockets;

using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Messenger class contains all static helper methods you need to send out SNMP messages. 
    /// Static methods in Manager or Agent class will be removed in the future.
    /// </summary>
    public static class Messenger
    {
        private static readonly Socket udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static Socket udpV6;
        private const string STR_CannotUseIPV6AsTheOSDoesNotSupportIt = "cannot use IP v6 as the OS does not support it";

        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public static Socket GetSocket(EndPoint endpoint)
        {
            if (endpoint.AddressFamily == AddressFamily.InterNetwork)
            {
                return udp;
            }
            else
            {
                return UdpV6;
            }
        }
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

            GetRequestMessage message = new GetRequestMessage(RequestCounter.NextCount, version, community, variables);
            GetResponseMessage response = message.GetResponse(timeout, endpoint, GetSocket(endpoint));
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            return response.Variables;
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

            SetRequestMessage message = new SetRequestMessage(RequestCounter.NextCount, version, community, variables);
            GetResponseMessage response = message.GetResponse(timeout, endpoint, GetSocket(endpoint));
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }
            
            return response.Variables;
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
            int index = -1;
            Variable tableV = new Variable(table);
            Variable seed;
            Variable next = tableV;
            bool first = true;
            bool oldWay = false;

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

                // Here we need to figure out which way we will be counting tables
                if (first && seed.Id.ToString().StartsWith(table + ".1.1.", StringComparison.Ordinal))
                {
                    oldWay = true;
                }
                else if (first)
                {
                    string part = seed.Id.ToString().Replace(table.ToString(), null).Remove(0, 1);
                    int end = part.IndexOf('.');
                    index = Int32.Parse(part.Substring(0, end), CultureInfo.InvariantCulture);
                }

                first = false;
                if (oldWay && seed.Id.ToString().StartsWith(table + ".1.1.", StringComparison.Ordinal))
                {
                    result++;
                }
                else if (!oldWay)
                {
                    string part = seed.Id.ToString().Replace(table.ToString(), null).Remove(0, 1);
                    int end = part.IndexOf('.');
                    int newIndex = Int32.Parse(part.Substring(0, end), CultureInfo.InvariantCulture);
                    if (index == newIndex)
                    {
                        result++;
                    }
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
        public static bool HasNext(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed, int timeout, out Variable next)
        {
            if (seed == null)
            {
                throw new ArgumentNullException("seed");
            }
            
            List<Variable> variables = new List<Variable>();
            variables.Add(new Variable(seed.Id));

            GetNextRequestMessage message = new GetNextRequestMessage(
                RequestCounter.NextCount,
                version,
                community,
                variables);

            GetResponseMessage response = message.GetResponse(timeout, endpoint, GetSocket(endpoint));
            next = response.ErrorStatus == ErrorCode.NoSuchName ? null : response.Variables[0];
            return response.ErrorStatus != ErrorCode.NoSuchName;
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
                RequestCounter.NextCount,
                version,
                community,
                0,
                maxRepetitions,
                variables);

            GetResponseMessage response = message.GetResponse(timeout, endpoint, GetSocket(endpoint));
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response);
            }

            next = response.Variables;
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
            message.Send(receiver, GetSocket(receiver));
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
            message.Send(receiver, GetSocket(receiver));
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
            GetResponseMessage response = message.GetResponse(timeout, receiver, GetSocket(receiver));
            if (response.ErrorStatus != ErrorCode.NoError)
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
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }
            
            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            // bool canContinue = registry.ValidateTable(table);
            // if (!canContinue)
            // {
            //    throw new ArgumentException("not a table OID: " + table);
            // }
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

        private static object root = new object();

        private static Socket UdpV6
        {
            get
            {
                #if !(CF)
                if (!Socket.OSSupportsIPv6)
                {
                    throw new InvalidOperationException(STR_CannotUseIPV6AsTheOSDoesNotSupportIt);
                }
                #endif

                lock (root)
                {
                    if (udpV6 == null)
                    {
                        udpV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                    }
                }

                return udpV6;
            }
        }
    }
}
