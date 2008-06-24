using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
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
	public class Manager: Component
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
		/// Occurs when a <see cref="TrapMessage" /> is received.
		/// </summary>
		public event EventHandler<TrapReceivedEventArgs> TrapReceived;
		/// <summary>
		/// Default protocol version for operations.
		/// </summary>
		/// <remarks>By default, the value is SNMP v1.</remarks>
		public VersionCode DefaultVersion
		{
			get {
				return _version;
			}
			set {
				_version = value;
			}
		}
		/// <summary>
		/// Gets a list of variable binds.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variables">Variable binds</param>
		/// <returns></returns>
		public IList<Variable> Get(VersionCode version, IPAddress agent, string community, IList<Variable> variables)
		{
			if (version == VersionCode.V3)
			{
				throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
			}
			using (GetRequestMessage message = new GetRequestMessage(version,agent, community, variables))
			{
				return message.Send(_timeout);
			}
		}
		/// <summary>
		/// Gets a variable bind.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variable">Variable bind</param>
		/// <returns></returns>
		public Variable Get(VersionCode version, IPAddress agent, string community, Variable variable)
		{
			return Get(version, agent, community, new List<Variable>() { variable })[0];
		}
		/// <summary>
		/// Gets a variable bind.
		/// </summary>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variable">Variable bind</param>
		/// <returns></returns>
		public Variable Get(IPAddress agent, string community, Variable variable)
		{
			return Get(_version, agent, community, variable);
		}
		/// <summary>
		/// Gets a list of variable binds.
		/// </summary>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variables">Variable binds</param>
		/// <returns></returns>
		public IList<Variable> Get(IPAddress agent, string community, IList<Variable> variables)
		{
			return Get(_version, agent, community, variables);
		}
		/// <summary>
		/// Sets a list of variable binds.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variables">Variable binds</param>
		/// <returns></returns>
		public void Set(VersionCode version, IPAddress agent, string community, IList<Variable> variables)
		{
			if (version == VersionCode.V3)
			{
				throw new ArgumentException("you can only use SNMP v1 or v2 in this version");
			}
			using (SetRequestMessage message = new SetRequestMessage(version, agent, community, variables))
			{
				message.Send(_timeout);
			}
		}

		/// <summary>
		/// Sets a variable bind.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variable">Variable bind</param>
		/// <returns></returns>
		public void Set(VersionCode version, IPAddress agent, string community, Variable variable)
		{
			Set(version, agent, community, new List<Variable>() { variable });
		}

		/// <summary>
		/// Sets a variable bind.
		/// </summary>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variable">Variable bind</param>
		/// <returns></returns>
		public void Set(IPAddress agent, string community, Variable variable)
		{
			Set(_version, agent, community, variable);
		}

		/// <summary>
		/// Sets a list of variable binds.
		/// </summary>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variables">Variable binds</param>
		/// <returns></returns>
		public void Set(IPAddress agent, string community, IList<Variable> variables)
		{
			Set(_version, agent, community, variables);
		}
		/// <summary>
		/// Gets a table of variables.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="table">Table OID</param>
		/// <returns></returns>
		public Variable[,] GetTable(VersionCode version, IPAddress agent, string community, ObjectIdentifier table)
		{
			if (version == VersionCode.V2)
			{
				throw new ArgumentException("only SNMP v1 or v2 is supported");
			}
			if (!Mib.ObjectRegistry.Instance.IsTableId(table.ToNumerical())) {
				throw new ArgumentException("not a table OID: " + table);
			}
			IList<Variable> list = new List<Variable>();
            int rows = Walk(version, agent, community, table, list, _timeout);
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
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="table">OID</param>
		/// <param name="list">A list to hold the results</param>
		/// <param name="timeout">Timeout</param>
		/// <returns>Returns row count if the OID is a table. Otherwise this value is meaningless.</returns>
        public static int Walk(VersionCode version, IPAddress agent, string community, ObjectIdentifier table, IList<Variable> list, int timeout)
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
                if (!seed.Id.ToString().StartsWith(table + ".1.", StringComparison.Ordinal))
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
            while (HasNext(version, agent, community, seed, timeout, out next));
            return result;
        }
		/// <summary>
		/// Gets a table of variables.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="table">Table OID</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Variable[,] GetTable(VersionCode version, IPAddress agent, string community, uint[] table)
		{
			return GetTable(version, agent, community, new ObjectIdentifier(table));
		}
		/// <summary>
		/// Gets a table of variables.
		/// </summary>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="table">Table OID</param>
		/// <returns></returns>
		public Variable[,] GetTable(IPAddress agent, string community, ObjectIdentifier table)
		{
			return GetTable(_version, agent, community, table);
		}
		/// <summary>
		/// Gets a table of variables.
		/// </summary>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="table">Table OID</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Variable[,] GetTable(IPAddress agent, string community, uint[] table)
		{
			return GetTable(agent, community, new ObjectIdentifier(table));
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
		/// <param name="port">Port number</param>
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

		private void TrapListener_TrapReceived(object sender, TrapReceivedEventArgs e)
		{
			if (null != TrapReceived)
			{
				TrapReceived(this, e);
			}
		}

		static bool HasNext(VersionCode version, IPAddress agent, string community, Variable seed, int timeout, out Variable next)
		{
			bool result;
			try
			{
				GetNextRequestMessage message = new GetNextRequestMessage(version,
				                                                          agent,
				                                                          community,
				                                                          new List<Variable>(1) {
				                                                          	seed});

				next = message.Send(timeout)[0];
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
			// 
			// trapListener
			// 
			this.trapListener.Port = 162;
			this.trapListener.TrapReceived += new System.EventHandler<SharpSnmpLib.TrapReceivedEventArgs>(this.TrapListener_TrapReceived);
		}
	}
}
