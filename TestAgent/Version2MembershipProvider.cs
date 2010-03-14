namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP version 2 membership provider, who checks community names for security.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Version2MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V2;
        private readonly OctetString _get;
        private readonly OctetString _set;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version2MembershipProvider"/> class.
        /// </summary>
        /// <param name="getCommunity">The get community.</param>
        /// <param name="setCommunity">The set community.</param>
        public Version2MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
            _get = getCommunity;
            _set = setCommunity;
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(SnmpContext context)
        {
            if (context.Request.Version != Version)
            {
                return false;
            }

            if (context.Request.Pdu.TypeCode == SnmpType.SetRequestPdu)
            {
                return context.Request.Parameters.UserName == _set;
            }

            return context.Request.Parameters.UserName == _get;
        }
    }
}
