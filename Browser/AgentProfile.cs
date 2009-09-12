using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Browser
{
    internal class AgentProfile
	{
        private readonly string _get;
	    private readonly string _set;
	    private readonly VersionCode _version;
        private readonly IPEndPoint _agent;
        private readonly string _name;
        private readonly Guid _id;

	    internal AgentProfile(Guid id, VersionCode version, IPEndPoint agent, string getCommunity, string setCommunity, string name)
	    {
	    	_id = id;
	        _get = getCommunity;
	        _set = setCommunity;
	        _version = version;
            _agent = agent;
            _name = name;
	    }
	    
	    internal Guid Id
	    {
	    	get { return _id; }
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
	
        internal string Get(Manager manager, string textual)
        {
            Variable result = manager.Objects.CreateVariable(textual);
            TraceSource source = new TraceSource("Browser");
            source.TraceInformation(manager.GetSingle(_agent, _get, result).ToString());
            source.Flush();
            source.Close();
            return result.ToString();
        }

        internal string GetValue(Manager manager, string textual)
        {
            Variable result = manager.Objects.CreateVariable(textual);
            return manager.GetSingle(_agent, _get, result).Data.ToString();
        }

        internal string GetNext(Manager manager, string textual)
        {
            //Variable var = new Variable(textual);

            //Report(manager.GetNext(_agent.Address, _agent.Port, _get, var));
            return "";
        }
	
        //
        // TODO: return success if it succeeded!
        //
	    internal void Set(Manager manager, string textual, ISnmpData data)
	    {
            manager.SetSingle(_agent, _set, manager.Objects.CreateVariable(textual, data));
	    }
	
        internal static bool IsValidIPAddress(string address, out IPAddress ip)
        {
            return IPAddress.TryParse(address, out ip);
        }
	
	    internal void Walk(Manager manager, IDefinition def)
	    {
            IList<Variable> list = new List<Variable>();
	        int rows = Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity), new ObjectIdentifier(def.GetNumericalForm()), list, 1000, WalkMode.WithinSubtree);
            
            // 
            // How many rows are there?
            //
            if (rows > 0)
            {
                FormTable newTable = new FormTable(def);
                newTable.SetRows(rows);
                newTable.PopulateGrid(list);
                newTable.Show();
            }
            else
            {
                TraceSource source = new TraceSource("Browser");
                for (int i = 0; i < list.Count; i++)
                {
                    source.TraceInformation(list[i].ToString());
                }

                source.Flush();
                source.Close();
            }

	    }
    }
}
