using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Lextm.SharpSnmpLib.Browser
{
    internal static class AgentProfileFactory
    {
        internal static AgentProfile Create(Guid id, VersionCode version, IPEndPoint agent, string getCommunity, string setCommunity, string agentName, string authenticationPassphrase, string privacyPassphrase, int authenticationMethod, int privacyMethod, string userName)
        {
            if (version == VersionCode.V3)
            {
                return new SecureAgentProfile(id, version,
                                    agent, agentName,
                                    authenticationPassphrase, privacyPassphrase,
                                    authenticationMethod, privacyMethod,
                                    userName);
            }

            return new NormalAgentProfile(id, version,
                                    agent, getCommunity,
                                    setCommunity, agentName,
                                    userName);
        }
    }
}
