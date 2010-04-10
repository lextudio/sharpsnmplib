using System;
using System.Net;

using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Browser
{
	internal abstract class AgentProfile
	{
	    internal AgentProfile(Guid id, VersionCode version, IPEndPoint agent, string name, string userName)
		{
			Id = id;
	        UserName = userName;
			VersionCode = version;
			Agent = agent;
			Name = name;
		}

	    internal Guid Id { get; private set; }

	    internal string Name { get; private set; }

	    internal IPEndPoint Agent { get; private set; }

	    internal VersionCode VersionCode { get; private set; }

	    public string UserName { get; private set; }

	    internal abstract void Get(Manager manager, Variable variable);

	    internal abstract string GetValue(Manager manager, Variable variable);

	    internal abstract void GetNext(Manager manager, Variable variable);

	    internal abstract void Set(Manager manager, Variable variable);

	    internal static bool IsValid(string address, out IPAddress ip)
		{
			return IPAddress.TryParse(address, out ip);
		}
		
		internal abstract void GetTable(Manager manager, IDefinition def);

	    public abstract void Walk(Manager manager, IDefinition definition);
	}
}
