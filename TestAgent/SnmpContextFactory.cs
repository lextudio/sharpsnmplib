using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    internal static class SnmpContextFactory
    {
        public static SnmpContext Create(ISnmpMessage request, IPEndPoint sender, Listener listener, AgentObjects objects)
        {
            if (request.Version == VersionCode.V3)
            {
                return new SecureSnmpContext(request, sender, listener, objects);
            }

            return new NormalSnmpContext(request, sender, listener);
        }
    }
}