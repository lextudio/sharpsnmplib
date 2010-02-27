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
	    private readonly string _authenticationPassphrase;
	    private readonly string _privacyPassphrase;
	    private readonly string _authenticationMethod;
	    private readonly string _privacyMethod;
	    private readonly string _userName;

	    internal AgentProfile(Guid id, VersionCode version, IPEndPoint agent, string getCommunity, string setCommunity, string name, string authenticationPassphrase, string privacyPassphrase, string authenticationMethod, string privacyMethod, string userName)
		{
			_id = id;
	        _userName = userName;
	        _privacyMethod = privacyMethod;
	        _authenticationMethod = authenticationMethod;
	        _privacyPassphrase = privacyPassphrase;
	        _authenticationPassphrase = authenticationPassphrase;
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

		internal IPEndPoint Agent
		{
			get { return _agent; }
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

	    public string AuthenticationPassphrase
	    {
	        get { return _authenticationPassphrase; }
	    }

	    public string PrivacyPassphrase
	    {
	        get { return _privacyPassphrase;}
	    }

	    public string AuthenticationMethod
	    {
	        get { return _authenticationMethod; }
	    }

	    public string PrivacyMethod
	    {
            get { return _privacyMethod; }
	    }

	    public string UserName
	    {
	        get {
	            return _userName;
	        }
	    }

	    internal void Get(Manager manager, string textual)
		{
			Variable result = manager.Objects.CreateVariable(textual);
			TraceSource source = new TraceSource("Browser");
			source.TraceInformation(manager.GetSingle(Agent, GetCommunity, result).ToString());
			source.Flush();
			source.Close();
		}

		internal string GetValue(Manager manager, string textual)
		{
			Variable result = manager.Objects.CreateVariable(textual);
			return manager.GetSingle(Agent, GetCommunity, result).Data.ToString();
		}

		internal void GetNext(Manager manager, string textual)
		{
			Variable result = manager.Objects.CreateVariable(textual);
			TraceSource source = new TraceSource("Browser");
		    GetNextRequestMessage message = new GetNextRequestMessage(Messenger.NextId, VersionCode, new OctetString(GetCommunity),
		                                                              new List<Variable> {result});
		    ISnmpMessage response = message.GetResponse(manager.Timeout, _agent);
            if (response.Pdu.ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    Agent.Address,
                    response);
            }

            source.TraceInformation(response.Pdu.Variables[0].ToString());
            source.Flush();
		    source.Close();
		}
		
		internal void Set(Manager manager, string textual, ISnmpData data)
		{
            TraceSource source = new TraceSource("Browser");
            source.TraceInformation(manager.SetSingle(Agent, GetCommunity, manager.Objects.CreateVariable(textual, data)).ToString());
            source.Flush();
            source.Close();
		}
		
		internal static bool IsValidIPAddress(string address, out IPAddress ip)
		{
			return IPAddress.TryParse(address, out ip);
		}
		
		internal void GetTable(Manager manager, IDefinition def)
		{
			IList<Variable> list = new List<Variable>();
			int rows = Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity), new ObjectIdentifier(def.GetNumericalForm()), list, manager.Timeout, WalkMode.WithinSubtree);
			
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

	    public void Walk(Manager manager, IDefinition definition)
	    {
            TraceSource source = new TraceSource("Browser");
            IList<Variable> list = new List<Variable>();
            if (VersionCode == VersionCode.V1)
            {
                Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity),
                               new ObjectIdentifier(definition.GetNumericalForm()), list, manager.Timeout,
                               WalkMode.WithinSubtree);
            }
            else
            {
                Messenger.BulkWalk(VersionCode, Agent, new OctetString(GetCommunity),
                                   new ObjectIdentifier(definition.GetNumericalForm()), list, manager.Timeout, 10,
                                   WalkMode.WithinSubtree);
            }

	        foreach (Variable v in list)
            {
                source.TraceInformation(v.ToString());
            }

	        source.Flush();
            source.Close();
	    }
	}
}
