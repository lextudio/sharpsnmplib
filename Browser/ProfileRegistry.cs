using System;
using System.Collections.Generic;
using System.Text;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using System.Net;

namespace Lextm.SharpSnmpLib.Browser
{
	internal class ProfileRegistry
	{
	    private static string _default;
	    private static AgentProfile _defaultProfile;
	    
	    internal static AgentProfile GetProfile(string ip)
	    {
	        if (profiles.ContainsKey(ip)) 
	        {
	            return profiles[ip];
	        }
	        return null;
	    }

        internal static IEnumerable<string> Names
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
	    
	    internal static string Default
	    {
	        get { return _default; }
	        set 
	        {
	            if (value == null)
	            {
	                throw new ArgumentNullException("value");
	            }
	            if (value.Length == 0) 
	            {
	                throw new ArgumentException("value cannot be empty", "value");
	            }
	            _defaultProfile = GetProfile(value);
	            _default = value;
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
	    
	    private static IDictionary<string, AgentProfile> profiles = new Dictionary<string, AgentProfile>();

        internal static void DeleteProfile(string profile)
        {
            if (profiles.ContainsKey(profile))
            {
                profiles.Remove(profile);
            }
        }
    }
}
