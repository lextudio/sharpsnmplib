namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP version 3 membership provider. Not yet implemented.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class Version3MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V3;

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(SnmpContext context)
        {
            return context.Request.Version == Version && context.HandleMembership();
        }
    }
}
