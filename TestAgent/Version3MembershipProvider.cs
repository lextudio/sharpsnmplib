namespace Lextm.SharpSnmpLib.Agent
{
    internal class Version3MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V3;
        private readonly OctetString _get;
        private readonly OctetString _set;

        public Version3MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
            // TODO: implement v3 checking.
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
