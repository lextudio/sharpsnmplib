using System.Collections.Generic;
namespace Lextm.SharpSnmpLib.Agent
{
    internal class SnmpApplicationFactory
    {
        private readonly Logger _logger;
        private readonly ObjectStore _store;
        private readonly IMembershipProvider _membershipProvider;
        private readonly MessageHandlerFactory _factory;
        private object root = new object();
        private Queue<SnmpApplication> _queue = new Queue<SnmpApplication>();

        public SnmpApplicationFactory(Logger logger, ObjectStore store, IMembershipProvider membershipProvider, MessageHandlerFactory factory)
        {
            _logger = logger;
            _membershipProvider = membershipProvider;
            _store = store;
            _factory = factory;
        }

        public SnmpApplication Create(SnmpContext context)
        {
            SnmpApplication result = null;
            lock (root)
            {
                if (_queue.Count > 0)
                {
                    result = _queue.Dequeue();
                }
            }

            if (result == null)
            {
                result = new SnmpApplication(this, _logger, _store, _membershipProvider, _factory);              
            }

            result.Context = context;
            return result;
        }

        internal void Reuse(SnmpApplication application)
        {
            lock (root)
            {
                _queue.Enqueue(application);
            }
        }
    }
}