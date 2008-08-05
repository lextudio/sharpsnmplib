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
	    private IPEndPoint _default;
	    private AgentProfile _defaultProfile;
	    private string _defaultString;

        private static ProfileRegistry _instance;

        internal static ProfileRegistry Instance
        {
            get
            {
                lock (typeof(ProfileRegistry))
                {
                    if (_instance == null)
                    {
                        _instance = new ProfileRegistry();
                    }
                }
                return _instance;
            }
        }
	    
	    internal AgentProfile GetProfile(IPEndPoint endpoint)
	    {
	        if (profiles.ContainsKey(endpoint)) 
	        {
	            return profiles[endpoint];
	        }
	        return null;
	    }

        internal event EventHandler OnChanged;

        internal IEnumerable<IPEndPoint> Names
        {
            get { return profiles.Keys; }
        }

        internal IEnumerable<AgentProfile> Profiles
        {
            get { return profiles.Values; }
        }

	    internal AgentProfile DefaultProfile
	    {
	        get { return _defaultProfile; }
	    }
	    
	    internal string DefaultString
	    {
	        get { return _defaultString; }
	    }
	    
	    internal IPEndPoint Default
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
	    
	    internal void AddProfile(AgentProfile profile)
	    {
            AddInternal(profile);
            if (OnChanged != null)
            {
                OnChanged(null, EventArgs.Empty);
            }
	    }

        private void AddInternal(AgentProfile profile)
        {
            if (profiles.ContainsKey(profile.Agent))
            {
                throw new MibBrowserException("This endpoint is already registered");
            }

            profiles.Add(profile.Agent, profile);            
        }
	    
	    private IDictionary<IPEndPoint, AgentProfile> profiles = new Dictionary<IPEndPoint, AgentProfile>();

        internal void DeleteProfile(IPEndPoint profile)
        {
            DeleteInternal(profile);
            if (OnChanged != null)
            {
                OnChanged(null, EventArgs.Empty);
            }
        }

        private void DeleteInternal(IPEndPoint profile)
        {
            if (profiles.ContainsKey(profile))
            {
                profiles.Remove(profile);
            }
        }

        internal void ReplaceProfile(AgentProfile agentProfile)
        {
            DeleteInternal(agentProfile.Agent);
            AddInternal(agentProfile);
            if (OnChanged != null)
            {
                OnChanged(null, EventArgs.Empty);
            }
        }
    }
}
