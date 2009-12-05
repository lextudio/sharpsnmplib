namespace Lextm.SharpSnmpLib.Agent
{
    internal class Version1MembershipProvider : MembershipProvider
    {
        private readonly VersionCode _version;
        private readonly OctetString _get;
        private readonly OctetString _set;

        public Version1MembershipProvider(VersionCode version, OctetString getCommunity, OctetString setCommunity)
        {
            _version = version;
            _get = getCommunity;
            _set = setCommunity;
        }

        public bool AuthenticateRequest(ISnmpMessage message)
        {
            if (message.Version != _version)
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
