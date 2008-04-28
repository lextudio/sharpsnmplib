using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace SharpSnmpLib
{
    public enum ProtocolVersion
    {
        V1 = 0,
        V2c,
        V3
    }

    public class Manager: Component
    {
    	public Manager()
    	{
    		InitializeComponent();
    	}
    	
        public event EventHandler<TrapReceivedEventArgs> OnTrapReceived;
        private TrapListener trapListener;
        private ProtocolVersion _version;
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

        public ProtocolVersion DefaultVersion
        {
            get {
                return _version;
            }
            set {
                _version = value;
            }
        }

        public Variable Get(ProtocolVersion version, IPAddress agent, string community, Variable variable)
        {
            if (version != ProtocolVersion.V1)
            {
                throw new ArgumentException("you can only use SNMP v1 in this version");
            }
            GetMessage message = new GetMessage(agent, community, variable);
            Variable result = message.Send(_timeout);
            return result;
        }
        
        public void Set(ProtocolVersion version, IPAddress agent, string community, Variable variable)
        {
            if (version != ProtocolVersion.V1)
            {
                throw new ArgumentException("you can only use SNMP v1 in this version");
            }
            SetMessage message = new SetMessage(agent, community, variable);
            message.Send(_timeout);
        }

        public Variable Get(IPAddress agent, string community, Variable variable)
        {
            return Get(_version, agent, community, variable);
        }
        
        public void Set(IPAddress agent, string community, Variable variable)
        {
            Set(_version, agent, community, variable);
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
