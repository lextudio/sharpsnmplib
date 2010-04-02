using System;
using System.Collections.Generic;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP version 3 membership provider. Not yet implemented.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Version3MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V3;

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(SnmpContext context)
        {
            ISnmpMessage message = context.Request;
            if (message.Version != Version)
            {
                return false;
            }

            return context.HandleMembership();
        }
    }
}
