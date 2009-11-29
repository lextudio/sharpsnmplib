namespace Lextm.SharpSnmpLib.Agent
{
    internal class SecurityGuard
    {
        private readonly VersionCode _version;
        private readonly OctetString _get;
        private readonly OctetString _set;

        public SecurityGuard(VersionCode version, OctetString getCommunity, OctetString setCommunity)
        {
            _version = version;
            _get = getCommunity;
            _set = setCommunity;
        }

        public bool Allow(ISnmpMessage message)
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
