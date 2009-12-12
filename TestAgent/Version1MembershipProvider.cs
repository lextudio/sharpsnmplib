namespace Lextm.SharpSnmpLib.Agent
{
    internal class Version1MembershipProvider : IMembershipProvider
    {
        private readonly VersionCode _version = VersionCode.V1;
        private readonly OctetString _get;
        private readonly OctetString _set;

        public Version1MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
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
