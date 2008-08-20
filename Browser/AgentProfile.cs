using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using System.Net;

namespace Lextm.SharpSnmpLib.Browser
{
	internal delegate void ReportMessage(string message);
	
    internal class AgentProfile
	{
        private string _get;
	    private string _set;
	    private VersionCode _version;
        private IPEndPoint _agent;
        private string _name;

	    internal AgentProfile(VersionCode version, IPEndPoint agent, string getCommunity, string setCommunity, string name)
	    {
	        _get = getCommunity;
	        _set = setCommunity;
	        _version = version;
            _agent = agent;
            _name = name;
	    }

        internal string Name
        {
            get { return _name; }
        }

//	    internal int Port
//	    {
//	        get { return _agent.Port; }
//	    }
//	    
//	    internal IPAddress IP
//	    {
//	        get { return _agent.Address; }
//	    }

        internal IPEndPoint Agent
        {
            get
            {
                return _agent;
            }
        }

        internal VersionCode VersionCode
        {
            get { return _version; }
        }

        internal string GetCommunity
        {
            get { return _get; }
        }

        internal string SetCommunity
        {
            get { return _set; }
        }
	
	    internal event ReportMessage OnOperationCompleted;

        internal string Get(Manager manager, string textual)
        {
            Variable var = new Variable(textual);

            Report(manager.Get(_agent, _get, var));
            return var.ToString();
        }

        internal string GetValue(Manager manager, string textual)
        {
            Variable var = new Variable(textual);

            return manager.Get(_agent, _get, var).Data.ToString();
        }

        internal string GetNext(Manager manager, string textual)
        {
            //Variable var = new Variable(textual);

            //Report(manager.GetNext(_agent.Address, _agent.Port, _get, var));
            return "";
        }
	
	    private void Report(Variable variable)
	    {
	        if (OnOperationCompleted != null)
	        {
	            OnOperationCompleted(variable.ToString());
	        }
	    }
	
        //
        // TODO: return success if it succeeded!
        //
	    internal void Set(Manager manager, string textual, ISnmpData data)
	    {
            manager.Set(VersionCode, _agent, _set, new Variable(textual, data));
	    }
	
	    // private IPAddress ValidateIP()
	    // {
            // IPAddress ip;
            // bool succeeded = IsValidIPAddress(_ip, out ip);
	        // if (!succeeded)
	        // {
	            // throw new MibBrowserException("Invalid IP address: " + _ip);
	        // }
	        // return ip;
	    // }

        internal static bool IsValidIPAddress(string address, out IPAddress ip)
        {
            return IPAddress.TryParse(address, out ip);
        }
	
	    internal void Walk(Manager manager, IDefinition def)
	    {
            IList<Variable> list = new List<Variable>();
 
            Manager.Walk(this.VersionCode, this.Agent, this.GetCommunity, new ObjectIdentifier(def.GetNumericalForm()), list, 1000, WalkMode.WithinSubtree);
            for (int i = 0; i < list.Count; i++)
            {
                Report(list[i]);
            }

	    }
    }
}
