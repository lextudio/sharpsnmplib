using System;
using System.Collections.Generic;
using System.Net;

namespace Lextm.SharpSnmpLib.Browser
{
    internal interface IProfileRegistry
    {
        AgentProfile DefaultProfile { get; set; }
        IEnumerable<AgentProfile> Profiles { get; }
        void AddProfile(AgentProfile profile);
        void DeleteProfile(IPEndPoint profile);
        void ReplaceProfile(AgentProfile agentProfile);
        void LoadProfiles();
        void SaveProfiles();
        event EventHandler<EventArgs> OnChanged;
    }
}