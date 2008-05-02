using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace SharpSnmpLib
{
    public class Manager: Component
    {
    	public Manager()
    	{
    		InitializeComponent();
    	}
    	
        public event EventHandler<TrapReceivedEventArgs> OnTrapReceived;
        private TrapListener trapListener;
        private VersionCode _version = VersionCode.V1;
        private int _timeout = 5000;

        private void InitializeComponent()
        {
            this.trapListener = new SharpSnmpLib.TrapListener();
            // 
            // trapListener
            // 
            this.trapListener.Port = 162;
            this.trapListener.TrapReceived += new System.EventHandler<SharpSnmpLib.TrapReceivedEventArgs>(this.trapListener_TrapReceived);
        }

        public VersionCode DefaultVersion
        {
            get {
                return _version;
            }
            set {
                _version = value;
            }
        }

        public IList<Variable> Get(VersionCode version, IPAddress agent, string community, IList<Variable> variables)
        {
            if (version != VersionCode.V1)
            {
                throw new ArgumentException("you can only use SNMP v1 in this version");
            }
            GetRequestMessage message = new GetRequestMessage(version,agent, community, variables);
            return message.Send(_timeout);
        }       
                
        public void Set(VersionCode version, IPAddress agent, string community, IList<Variable> variables)
        {
            if (version != VersionCode.V1)
            {
                throw new ArgumentException("you can only use SNMP v1 in this version");
            }
            SetRequestMessage message = new SetRequestMessage(version, agent, community, variables);
            message.Send(_timeout);
        }
        
        public Variable Get(VersionCode version, IPAddress agent, string community, Variable variable)
        {
       		return Get(version, agent, community, new List<Variable>() {variable})[0];
        }
        
        public void Set(VersionCode version, IPAddress agent, string community, Variable variable)
        {
       		Set(version, agent, community, new List<Variable>() {variable});
        }
        
        public Variable Get(IPAddress agent, string community, Variable variable)
        {
        	return Get(_version, agent, community, variable);
        }
        
        public void Set(IPAddress agent, string community, Variable variable)
        {
            Set(_version, agent, community, variable);
        }    
        
        public IList<Variable> Get(IPAddress agent, string community, IList<Variable> variables)
        {
        	return Get(_version, agent, community, variables);
        }
        
        public void Set(IPAddress agent, string community, IList<Variable> variables)
        {
        	Set(_version, agent, community, variables);
        }

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

        private void trapListener_TrapReceived(object sender, TrapReceivedEventArgs e)
        {
            if (null != OnTrapReceived)
            {
                OnTrapReceived(this, e);
            }
        }
    }
}
