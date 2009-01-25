using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using Lextm.SharpSnmpLib.Mib;
using System.Threading;

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
        private VersionCode _version;
        private int _timeout = 5000;
        private readonly object locker = new object();

        /// <summary>
        /// Creates a <see cref="Manager"></see> instance.
        /// </summary>
        public Manager()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Returns a value if the listener is still working.
        /// </summary>
        [Obsolete("Use TrapListener property instead.")]
        public bool Active
        {
            get { return trapListener.Active; }
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
                lock (locker)
                {
                    _version = value;
                }
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
        public static IDictionary<IPEndPoint, Variable> Discover(VersionCode version, IPEndPoint endpoint, OctetString community, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            Variable v = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 }));
            List<Variable> variables = new List<Variable>();
            variables.Add(v);

            GetRequestMessage message = new GetRequestMessage(version, community, variables);
            return message.Broadcast(timeout, endpoint);
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
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            GetRequestMessage message = new GetRequestMessage(version, community, variables);
            GetResponseMessage response = message.GetResponse(timeout, endpoint);
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response.ErrorStatus,
                    response.ErrorIndex,
                    response.Variables[response.ErrorIndex - 1].Id);
            }

            return response.Variables;
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
            List<Variable> variables = new List<Variable>();
            variables.Add(variable);

            return Get(_version, endpoint, new OctetString(community), variables, _timeout)[0];
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
        
        private const int DefaultPort = 161;

        /// <summary>
        /// Gets a list of variable binds.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variable binds.</param>
        /// <returns></returns>
        public IList<Variable> Get(IPEndPoint endpoint, string community, IList<Variable> variables)
        {
            return Get(_version, endpoint, new OctetString(community), variables, _timeout);
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
        public static void Set(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
            }

            SetRequestMessage message = new SetRequestMessage(version, community, variables);
            GetResponseMessage response = message.GetResponse(timeout, endpoint);

            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    endpoint.Address,
                    response.ErrorStatus,
                    response.ErrorIndex,
                    response.Variables[response.ErrorIndex - 1].Id);
            }
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
            List<Variable> variables = new List<Variable>();
            variables.Add(variable);

            Set(_version, endpoint, new OctetString(community), variables, _timeout);
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
            Set(_version, endpoint, new OctetString(community), variables, _timeout);
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
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "ByDesign")]
        public static Variable[,] GetTable(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, int timeout)
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
            int rows = SteveWalk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree);
            
            // int rows = Walk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree);
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
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">Table OID.</param>
        /// <param name="timeout">Timeout.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return", Justification = "ByDesign")]
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "ByDesign")]
        public static Variable[,] GetInnerTable(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, int timeout)
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
            int rows = SteveWalk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree);
            
            // int rows = Walk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree);
            if (rows == 0)
            {
                return new Variable[0, 0];
            }

            int cols = list.Count / rows;

            if (list.Count % rows != 0)
            {
                cols++;
            }

            int k = 0;
            Variable[,] result = new Variable[rows, cols];
            string index = list[0].Id.ToString().Replace(table.ToString(), null).Remove(0, 1);
            int end = index.IndexOf('.');
            index = index.Substring(0, end);

            for (int j = 0; j < cols; j++)
            {
                string newIndex = list[k].Id.ToString().Replace(table.ToString(), null).Remove(0, 1);
                end = newIndex.IndexOf('.');
                newIndex = newIndex.Substring(0, end);
                int i = 0;

                while (index == newIndex)
                {
                    result[i, j] = list[k];
                    k++;
                    i++;

                    if (k < list.Count)
                    {
                        newIndex = list[k].Id.ToString().Replace(table.ToString(), null).Remove(0, 1);
                        end = newIndex.IndexOf('.');
                        newIndex = newIndex.Substring(0, end);
                    }
                    else
                    {
                        break;
                    }
                }

                index = newIndex;
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
            return GetTable(DefaultVersion, endpoint, new OctetString(community), table, Timeout);
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
            return GetTable(IPAddress.Parse(address), community, table);
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
        /// Steve Walks.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="community">Community name.</param>
        /// <param name="table">OID.</param>
        /// <param name="list">A list to hold the results.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="mode">Walk mode.</param>
        /// <returns>Returns row count if the OID is a table. Otherwise this value is meaningless.</returns>
        public static int SteveWalk(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, int timeout, WalkMode mode)
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
                    if (index != newIndex)
                    {
                        index = newIndex;
                        result++;
                    }
                }
            }
            while (HasNext(version, endpoint, community, seed, timeout, out next));
            return result;
        }

        /// <summary>
        /// Starts trap listener.
        /// </summary>
        [Obsolete("Use TrapListener property instead.")]
        public void Start()
        {
            trapListener.Start();
        }

        /// <summary>
        /// Starts trap listener on a specific port.
        /// </summary>
        /// <param name="port">Manager port number</param>
        [Obsolete("Use TrapListener property instead.")]
        public void Start(int port)
        {
            trapListener.Start(port);
        }

        /// <summary>
        /// Stops trap listener.
        /// </summary>
        [Obsolete("Use TrapListener property instead.")]
        public void Stop()
        {
            trapListener.Stop();
        }

        /// <summary>
        /// Trap listener.
        /// </summary>
        public TrapListener TrapListener
        {
            get { return trapListener; }
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
                Interlocked.Exchange(ref _timeout, value);
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Manager"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
// ReSharper disable RedundantToStringCall
            return "SNMP manager: timeout: " + Timeout.ToString(CultureInfo.InvariantCulture) + "; version: " + DefaultVersion.ToString() + "; " + trapListener;
// ReSharper restore RedundantToStringCall
        }

        private void TrapListener_TrapV1Received(object sender, TrapV1ReceivedEventArgs e)
        {
            EventHandler<TrapV1ReceivedEventArgs> handler = TrapV1Received;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        private void TrapListener_TrapV2Received(object sender, TrapV2ReceivedEventArgs e)
        {
            EventHandler<TrapV2ReceivedEventArgs> handler = TrapV2Received;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        
        private void TrapListener_InformRequestReceived(object sender, InformRequestReceivedEventArgs e)
        {
            EventHandler<InformRequestReceivedEventArgs> handler = InformRequestReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private static bool HasNext(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed, int timeout, out Variable next)
        {
            bool result;
            try
            {
                List<Variable> variables = new List<Variable>();
                variables.Add(new Variable(seed.Id));

                GetNextRequestMessage message = new GetNextRequestMessage(
                    version,
                    community,
                    variables);

                GetResponseMessage response = message.GetResponse(timeout, endpoint);
                if (response.ErrorStatus != ErrorCode.NoError)
                {
                    throw SharpErrorException.Create(
                        "error in response",
                        endpoint.Address,
                        response.ErrorStatus,
                        response.ErrorIndex,
                        response.Variables[response.ErrorIndex - 1].Id);
                }

                next = response.Variables[0];
                result = true;
            }
            catch (SharpOperationException)
            {
                next = null;
                result = false;
            }

            return result;
        }
// ReSharper disable all
        private void InitializeComponent()
        {

            this.trapListener = new SharpSnmpLib.TrapListener();
            this.trapListener.TrapV1Received += new System.EventHandler<SharpSnmpLib.TrapV1ReceivedEventArgs>(this.TrapListener_TrapV1Received);
            this.trapListener.TrapV2Received += new System.EventHandler<SharpSnmpLib.TrapV2ReceivedEventArgs>(this.TrapListener_TrapV2Received);
            this.trapListener.InformRequestReceived += new System.EventHandler<SharpSnmpLib.InformRequestReceivedEventArgs>(this.TrapListener_InformRequestReceived);
        }
// ReSharper restore all
    }
}