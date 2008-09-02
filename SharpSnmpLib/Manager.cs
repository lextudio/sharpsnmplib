using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP manager component that provides SNMP operations.
    /// </summary>
    /// <remarks>
    /// <para>Drag this component into your form in designer, or create an instance in code.</para>
    /// <para>Use <see cref="TrapListener" /> component if you only need TRAP operation.</para>
    /// <para>Currently only SNMP v1 and v2c operations are supported.</para>
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
        public event EventHandler<TrapV1ReceivedEventArgs> TrapV1Received;
        
        /// <summary>
        /// Occurs when a <see cref="TrapV2Message"/> is received.
        /// </summary>
        public event EventHandler<TrapV2ReceivedEventArgs> TrapV2Received;
        
        /// <summary>
        /// Occurs when a <see cref="InformRequestMessage"/> is received.
        /// </summary>
        public event EventHandler<InformRequestReceivedEventArgs> InformRequestReceived;
        
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
        /// <param name="endpoint">Broadcast endpoint.</param>
        /// <param name="community">Comunity name.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns></returns>
        public static IDictionary<IPEndPoint, Variable> Discover(VersionCode version, IPEndPoint endpoint, string community, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            Variable v = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 }));
            GetRequestMessage message = new GetRequestMessage(version, endpoint.Address, community, new List<Variable>() { v });
            return message.Broadcast(timeout, endpoint.Port);
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
        public static IList<Variable> Get(VersionCode version, IPEndPoint endpoint, string community, IList<Variable> variables, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            GetRequestMessage message = new GetRequestMessage(version, endpoint.Address, community, variables);
            return message.Send(timeout, endpoint.Port);
        }

        /// <summary>
        /// Gets a variable bind.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <returns></returns>
        public Variable GetSingle(IPEndPoint endpoint, string community, Variable variable)
        {
            return Get(_version, endpoint, community, new List<Variable>() { variable }, _timeout)[0];
        }

        /// <summary>
        /// Gets a variable bind.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <returns></returns>
        public Variable GetSingle(string address, string community, Variable variable)
        {
            return GetSingle(IPAddress.Parse(address), community, variable);
        }
        
        /// <summary>
        /// Gets a variable bind.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <returns></returns>
        public Variable GetSingle(IPAddress address, string community, Variable variable)
        {
            return GetSingle(new IPEndPoint(address, DefaultPort), community, variable);
        }
        
        private const int DefaultPort = 162;

        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public IList<Variable> Get(IPEndPoint endpoint, string community, IList<Variable> variables)
        {
            return Get(_version, endpoint, community, variables, _timeout);
        }
        
        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public IList<Variable> Get(string address, string community, IList<Variable> variables)
        {
            return Get(IPAddress.Parse(address), community, variables);
        }
        
        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public IList<Variable> Get(IPAddress address, string community, IList<Variable> variables)
        {
            return Get(new IPEndPoint(address, DefaultPort), community, variables);
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
        public static void Set(VersionCode version, IPEndPoint endpoint, string community, IList<Variable> variables, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            SetRequestMessage message = new SetRequestMessage(version, endpoint.Address, community, variables);
            message.Send(timeout, endpoint.Port);
        }

        /// <summary>
        /// Sets a variable bind.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <returns></returns>
        public void SetSingle(IPEndPoint endpoint, string community, Variable variable)
        {
            Set(_version, endpoint, community, new List<Variable>() { variable }, _timeout);
        }
        
        /// <summary>
        /// Sets a variable bind.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <returns></returns>
        public void SetSingle(IPAddress address, string community, Variable variable)
        {
            SetSingle(new IPEndPoint(address, DefaultPort), community, variable);
        }
        
        /// <summary>
        /// Sets a variable bind.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variable">Variable bind.</param>
        /// <returns></returns>
        public void SetSingle(string address, string community, Variable variable)
        {
            SetSingle(IPAddress.Parse(address), community, variable);
        }

        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public void Set(IPEndPoint endpoint, string community, IList<Variable> variables)
        {
            Set(_version, endpoint, community, variables, _timeout);
        }
        
        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public void Set(string address, string community, IList<Variable> variables)
        {
            Set(IPAddress.Parse(address), community, variables);
        }
        
        /// <summary>
        /// Sets a list of variable binds.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public void Set(IPAddress address, string community, IList<Variable> variables)
        {
            Set(new IPEndPoint(address, DefaultPort), community, variables);
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public static Variable[,] GetTable(VersionCode version, IPEndPoint endpoint, string community, ObjectIdentifier table, int timeout)
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
            int rows = Walk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree);
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
        /// Gets a table of variables.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(IPEndPoint endpoint, string community, ObjectIdentifier table)
        {
            return GetTable(endpoint, community, table);
        }
        
        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(IPAddress address, string community, ObjectIdentifier table)
        {
            return GetTable(new IPEndPoint(address, DefaultPort), community, table);
        }
        
        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(string address, string community, ObjectIdentifier table)
        {
            return GetTable(address, community, table);
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(IPEndPoint endpoint, string community, uint[] table)
        {
            return GetTable(_version, endpoint, community, new ObjectIdentifier(table), _timeout);
        }
        
        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(string address, string community, uint[] table)
        {
            return GetTable(IPAddress.Parse(address), community, new ObjectIdentifier(table));
        }

        /// <summary>
        /// Gets a table of variables.
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        public Variable[,] GetTable(IPAddress address, string community, uint[] table)
        {
            return GetTable(new IPEndPoint(address, DefaultPort), community, new ObjectIdentifier(table));
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
        /// <returns>Returns row count if the OID is a table. Otherwise this value is meaningless.</returns>
        public static int Walk(VersionCode version, IPEndPoint endpoint, string community, ObjectIdentifier table, IList<Variable> list, int timeout, WalkMode mode)
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
            while (HasNext(version, endpoint, community, seed, timeout, out next));
            return result;
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
            trapListener.Start(new IPEndPoint(IPAddress.Any, port));
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

        private void TrapListener_TrapV1Received(object sender, TrapV1ReceivedEventArgs e)
        {
            if (null != TrapV1Received)
            {
                TrapV1Received(this, e);
            }
        }
        
        private void TrapListener_TrapV2Received(object sender, TrapV2ReceivedEventArgs e)
        {
            if (null != TrapV2Received)
            {
                TrapV2Received(this, e);
            }
        }
        
        private void TrapListener_InformRequestReceived(object sender, InformRequestReceivedEventArgs e)
        {
            if (null != InformRequestReceived)
            {
                InformRequestReceived(this, e);
            }
        }

        private static bool HasNext(VersionCode version, IPEndPoint endpoint, string community, Variable seed, int timeout, out Variable next)
        {
            bool result;
            try
            {
                GetNextRequestMessage message = new GetNextRequestMessage(
                    version,
                    endpoint.Address,
                    community,
                    new List<Variable>(1) { seed });

                next = message.Send(timeout, endpoint.Port)[0];
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
            this.trapListener.TrapV1Received += new System.EventHandler<SharpSnmpLib.TrapV1ReceivedEventArgs>(this.TrapListener_TrapV1Received);
            this.trapListener.TrapV2Received += new System.EventHandler<SharpSnmpLib.TrapV2ReceivedEventArgs>(this.TrapListener_TrapV2Received);
            this.trapListener.InformRequestReceived += new System.EventHandler<SharpSnmpLib.InformRequestReceivedEventArgs>(this.TrapListener_InformRequestReceived);
        }
    }
}