using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP context factory.
    /// </summary>
    internal static class SnmpContextFactory
    {
        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="objects">The objects.</param>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        public static SnmpContext Create(ISnmpMessage request, IPEndPoint sender, Listener listener, AgentObjects objects, ListenerBinding binding)
        {
            if (request.Version == VersionCode.V3)
            {
                return new SecureSnmpContext(request, sender, listener, objects, binding);
            }

            return new NormalSnmpContext(request, sender, listener, binding);
        }
    }
}