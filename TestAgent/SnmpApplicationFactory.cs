namespace Lextm.SharpSnmpLib.Agent
{
    internal class SnmpApplicationFactory
    {
        private readonly Logger _logger;
        private readonly ObjectStore _store;
        private readonly Version1MembershipProvider _membershipProvider;

        public SnmpApplicationFactory(Logger logger, ObjectStore store, Version1MembershipProvider membershipProvider)
        {
            _logger = logger;
            _membershipProvider = membershipProvider;
            _store = store;
        }

        public SnmpApplication Create(SnmpContext context)
        {
            return new SnmpApplication(context, _logger, _store, _membershipProvider);
        }
    }
}