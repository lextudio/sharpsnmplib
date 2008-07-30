using System;
using System.Collections.Generic;
using System.Text;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using System.Net;

namespace Lextm.SharpSnmpLib.Browser
{
	internal static class ProfileRegistry
	{
	    private static IPAddress _default;
	    private static AgentProfile _defaultProfile;
	    private static string _defaultString;
	    
	    internal static AgentProfile GetProfile(IPAddress ip)
	    {
	        if (profiles.ContainsKey(ip)) 
	        {
	            return profiles[ip];
	        }
	        return null;
	    }

        internal static IEnumerable<IPAddress> Names
        {
            get { return profiles.Keys; }
        }

        internal static IEnumerable<AgentProfile> Profiles
        {
            get { return profiles.Values; }
        }

	    internal static AgentProfile DefaultProfile
	    {
	        get { return _defaultProfile; }
	    }
	    
	    internal static string DefaultString
	    {
	        get { return _defaultString; }
	    }
	    
	    internal static IPAddress Default
	    {
	        get { return _default; }
	        set 
	        {
	            if (value == null)
	            {
	                throw new ArgumentNullException("value");
	            }
	            _defaultProfile = GetProfile(value);
	            _default = value;
	            _defaultString = value.ToString();
	        }
	    }
	    
	    internal static void AddProfile(AgentProfile profile)
	    {
	        if (profiles.ContainsKey(profile.IP))
	        {
	            profiles.Remove(profile.IP);
	        }
	        
	        profiles.Add(profile.IP, profile);
	    }
	    
	    private static IDictionary<IPAddress, AgentProfile> profiles = new Dictionary<IPAddress, AgentProfile>();

        internal static void DeleteProfile(IPAddress profile)
        {
            if (profiles.ContainsKey(profile))
            {
                profiles.Remove(profile);
            }
        }
    }
}
