using System;
using System.Collections.Generic;
using System.Text;
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
	    private string _ip;
	
	    internal AgentProfile(string ip, VersionCode version, string getCommunity, string setCommunity)
	    {
	        _get = getCommunity;
	        _set = setCommunity;
	        _version = version;
	        _ip = ip;
	    }
	    
	    internal string IP
	    {
	        get { return _ip; }
	    }

        internal VersionCode VersionCode
        {
            get { return _version; }
        }
	
	    internal event ReportMessage OnOperationCompleted;
	
	    internal void Get(Manager manager, string textual)
	    {
	        IPAddress ip = ValidateIP();
	        Report(manager.Get(ip, _get, new Variable(textual)));
	    }
	
	    private void Report(Variable variable)
	    {
	        if (OnOperationCompleted != null)
	        {
	            OnOperationCompleted(variable.ToString());
	        }
	    }
	
	    internal void Set(Manager manager, string textual, ISnmpData data)
	    {
	        IPAddress ip = ValidateIP();
	        manager.Set(ip, _get, new Variable(textual, data));
	    }
	
	    private IPAddress ValidateIP()
	    {
	        IPAddress ip;
	        bool succeeded = IPAddress.TryParse(_ip, out ip);
	        if (!succeeded)
	        {
	            throw new MibBrowserException("Invalid IP address: " + _ip);
	        }
	        return ip;
	    }
	
	    internal void Walk(Manager manager, IDefinition def)
	    {
	        IPAddress ip = ValidateIP();
	    }
	}
}
