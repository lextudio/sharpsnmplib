namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP version 3 membership provider. Not yet implemented.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Version3MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V3;
        private readonly OctetString _get;
        private readonly OctetString _set;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version3MembershipProvider"/> class.
        /// </summary>
        /// <param name="getCommunity">The get community.</param>
        /// <param name="setCommunity">The set community.</param>
        public Version3MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
            // TODO: implement v3 checking.
            _get = getCommunity;
            _set = setCommunity;
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(ISnmpMessage message)
        {
            if (message.Version != Version)
            {
                return false;
            }

            if (message.Pdu.TypeCode == SnmpType.SetRequestPdu)
            {
                return message.Parameters.UserName == _set;
            }

            return message.Parameters.UserName == _get;
        }
    }
}
