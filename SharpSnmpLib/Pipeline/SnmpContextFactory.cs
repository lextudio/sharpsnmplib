using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Pipeline
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
        /// <param name="users">The users.</param>
        /// <param name="group">The engine group.</param>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        public static SnmpContext Create(ISnmpMessage request, IPEndPoint sender, UserRegistry users, EngineGroup group, IListenerBinding binding)
        {
            if (request.Version == VersionCode.V3)
            {
                return new SecureSnmpContext(request, sender, users, group, binding);
            }

            return new NormalSnmpContext(request, sender, users, binding);
        }
    }
}