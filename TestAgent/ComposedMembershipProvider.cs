namespace Lextm.SharpSnmpLib.Agent
{
    internal class ComposedMembershipProvider : IMembershipProvider
    {
        private readonly IMembershipProvider[] _providers;

        public ComposedMembershipProvider(IMembershipProvider[] providers)
        {
            _providers = providers;
        }

        public bool AuthenticateRequest(ISnmpMessage message)
        {
            foreach (IMembershipProvider provider in _providers)
            {
                if (provider.AuthenticateRequest(message))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
