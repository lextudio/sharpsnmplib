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
            if (version != VersionCode.V1)
            {
                throw new ArgumentException("you can only use SNMP v1 in this version");
            }
            using (GetRequestMessage message = new GetRequestMessage(version,agent, community, variables))
            {
            	return message.Send(_timeout);
            }            
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
            if (version != VersionCode.V1)
            {
                throw new ArgumentException("you can only use SNMP v1 in this version");
            }
            using (SetRequestMessage message = new SetRequestMessage(version, agent, community, variables))
            {
            	message.Send(_timeout);
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
       		return Get(version, agent, community, new List<Variable>() {variable})[0];
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
       		Set(version, agent, community, new List<Variable>() {variable});
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
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variables">Variable binds</param>
		/// <returns></returns>        
        public void Set(IPAddress agent, string community, IList<Variable> variables)
        {
        	Set(_version, agent, community, variables);
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

        private void TrapListener_TrapReceived(object sender, TrapReceivedEventArgs e)
        {
            if (null != TrapReceived)
            {
                TrapReceived(this, e);
            }
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
