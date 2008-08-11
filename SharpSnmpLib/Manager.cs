using Lextm.SharpSnmpLib.Mib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP manager component that provides SNMP operations.
    /// </summary>
    /// <remarks>
    /// <para>Drag this component into your form in designer, or create an instance in code.</para>
    /// <para>Use <see cref="TrapListener" /> component if you only need TRAP operation.</para>
    /// <para>Currently only SNMP v1 operations are supported.</para>
    /// </remarks>
    public class Manager : Component
    {
        private TrapListener trapListener;
        private VersionCode _version = VersionCode.V1;
        private int _timeout = 5000;

        /// <summary>
        /// Creates a <see cref="Manager"></see> instance.
        /// </summary>
        public Manager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when a <see cref="TrapV1Message" /> is received.
        /// </summary>
        public event EventHandler<TrapV1ReceivedEventArgs> TrapReceived;

        /// <summary>
        /// Default protocol version for operations.
        /// </summary>
        /// <remarks>By default, the value is SNMP v1.</remarks>
        public VersionCode DefaultVersion
        {
            get
            {
                return _version;
            }

            set
            {
                _version = value;
            }
        }
        
        /// <summary>
        /// Discovers SNMP agents in the network
        /// </summary>
        /// <param name="version">Version code.</param>
        /// <param name="broadcast">Broadcast address.</param>
        /// <param name="port">Port number.</param>
        /// <param name="community">Comunity name.</param>
        /// <returns></returns>
        public IDictionary<IPEndPoint, ISnmpData> Discover(VersionCode version, IPAddress broadcast, int port, string community)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            Variable v = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 }));
            using (GetRequestMessage message = new GetRequestMessage(version, broadcast, community, new List<Variable>() { v }))
            {
                return message.Broadcast(_timeout, port);
            }
        }

        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variable binds</param>
        /// <returns></returns>
        public IList<Variable> Get(VersionCode version, IPAddress agent, int port, string community, IList<Variable> variables)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            using (GetRequestMessage message = new GetRequestMessage(version, agent, community, variables))
            {
                return message.Send(_timeout, port);
            }
        }

        /// <summary>
        /// Gets a variable bind.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="variable">Variable bind</param>
        /// <returns></returns>
        public Variable Get(VersionCode version, IPAddress agent, int port, string community, Variable variable)
        {
            return Get(version, agent, port, community, new List<Variable>() { variable })[0];
        }

        /// <summary>
        /// Gets a variable bind.
        /// </summary>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="variable">Variable bind</param>
        /// <returns></returns>
        public Variable Get(IPAddress agent, int port, string community, Variable variable)
        {
            return Get(_version, agent, port, community, variable);
        }

        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variable binds</param>
        /// <returns></returns>
        public IList<Variable> Get(IPAddress agent, int port, string community, IList<Variable> variables)
        {
            return Get(_version, agent, port, community, variables);
        }

        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variable binds</param>
        /// <returns></returns>
        public void Set(VersionCode version, IPAddress agent, int port, string community, IList<Variable> variables)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            using (SetRequestMessage message = new SetRequestMessage(version, agent, community, variables))
            {
                message.Send(_timeout, port);
            }
        }

        /// <summary>
        /// Sets a variable bind.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="variable">Variable bind</param>
        /// <returns></returns>
        public void Set(VersionCode version, IPAddress agent, int port, string community, Variable variable)
        {
            Set(version, agent, port, community, new List<Variable>() { variable });
        }

        /// <summary>
        /// Sets a variable bind.
        /// </summary>
        /// <param name="agent">Agent address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        public void Set(IPAddress agent, int port, string community, Variable variable)
        {
            Set(_version, agent, port, community, variable);
        }

        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="agent">Agent address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        public void Set(IPAddress agent, int port, string community, IList<Variable> variables)
        {
            Set(_version, agent, port, community, variables);
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="table">Table OID</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(VersionCode version, IPAddress agent, int port, string community, ObjectIdentifier table)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("only SNMP v1 or v2 is supported");
            }

            bool canContinue = ObjectRegistry.ValidateTable(table);
            if (!canContinue)
            {
                throw new ArgumentException("not a table OID: " + table);
            }

            IList<Variable> list = new List<Variable>();
            int rows = Walk(version, agent, port, community, table, list, _timeout, WalkMode.WithinSubtree);
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
        /// Walks.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="agent">Agent address.</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="mode">Walk mode.</param>
        /// <returns>Returns row count if the OID is a table. Otherwise this value is meaningless.</returns>
        public static int Walk(VersionCode version, IPAddress agent, int port, string community, ObjectIdentifier table, IList<Variable> list, int timeout, WalkMode mode)
        {
            int result = 0;
            Variable tableV = new Variable(table);
            Variable seed = null;
            Variable next = tableV;
            do
            {
                seed = next;
                if (seed == tableV)
                {
                    continue;
                }

                if (mode == WalkMode.WithinSubtree && !seed.Id.ToString().StartsWith(table + ".1.", StringComparison.Ordinal))
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
            while (HasNext(version, agent, port, community, seed, timeout, out next));
            return result;
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="table">Table OID</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(VersionCode version, IPAddress agent, int port, string community, uint[] table)
        {
            return GetTable(version, agent, port, community, new ObjectIdentifier(table));
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="table">Table OID</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(IPAddress agent, int port, string community, ObjectIdentifier table)
        {
            return GetTable(_version, agent, port, community, table);
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="agent">Agent address</param>
        /// <param name="port">Agent port number</param>
        /// <param name="community">Community name</param>
        /// <param name="table">Table OID</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(IPAddress agent, int port, string community, uint[] table)
        {
            return GetTable(agent, port, community, new ObjectIdentifier(table));
        }

        /// <summary>
        /// Starts trap listener.
        /// </summary>
        public void Start()
        {
            trapListener.Start();
        }

        /// <summary>
        /// Starts trap listener on a specific port.
        /// </summary>
        /// <param name="port">Manager port number</param>
        public void Start(int port)
        {
            trapListener.Start(port);
        }

        /// <summary>
        /// Stops trap listener.
        /// </summary>
        public void Stop()
        {
            trapListener.Stop();
        }

        /// <summary>
        /// Timeout value.
        /// </summary>
        /// <remarks>By default, the value is 5,000-milliseconds (5 seconds).</remarks>
        public int Timeout
        {
            get
            {
                return _timeout;
            }

            set
            {
                _timeout = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Manager"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SNMP manager: timeout: " + Timeout + "; version: " + DefaultVersion + "; " + trapListener;
        }

        private void TrapListener_TrapReceived(object sender, TrapV1ReceivedEventArgs e)
        {
            if (null != TrapReceived)
            {
                TrapReceived(this, e);
            }
        }

        private static bool HasNext(VersionCode version, IPAddress agent, int port, string community, Variable seed, int timeout, out Variable next)
        {
            bool result;
            try
            {
                GetNextRequestMessage message = new GetNextRequestMessage(
                    version,
                    agent,
                    community,
                    new List<Variable>(1) { seed });

                next = message.Send(timeout, port)[0];
                result = true;
            }
            catch (SharpErrorException)
            {
                next = null;
                result = false;
            }

            return result;
        }

        private void InitializeComponent()
        {
            this.trapListener = new SharpSnmpLib.TrapListener();
            this.trapListener.Port = 162;
            this.trapListener.TrapV1Received += new System.EventHandler<SharpSnmpLib.TrapV1ReceivedEventArgs>(this.TrapListener_TrapReceived);
        }
    }
}