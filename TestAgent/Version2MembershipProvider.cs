namespace Lextm.SharpSnmpLib.Agent
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Version2MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V2;
        private readonly OctetString _get;
        private readonly OctetString _set;

        public Version2MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
            _get = getCommunity;
            _set = setCommunity;
        }

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
