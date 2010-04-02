using System.Collections.Generic;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP application factory, who holds all pipeline instances.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SnmpApplicationFactory
    {
        private readonly ILogger _logger;
        private readonly ObjectStore _store;
        private readonly IMembershipProvider _membershipProvider;
        private readonly MessageHandlerFactory _factory;
        private readonly object _root = new object();
        private readonly Queue<SnmpApplication> _queue = new Queue<SnmpApplication>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpApplicationFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="store">The store.</param>
        /// <param name="membershipProvider">The membership provider.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="objects">The objects.</param>
        public SnmpApplicationFactory(ILogger logger, ObjectStore store, IMembershipProvider membershipProvider, MessageHandlerFactory factory)
        {
            _logger = logger;
            _membershipProvider = membershipProvider;
            _store = store;
            _factory = factory;
        }

        /// <summary>
        /// Creates a pipeline for the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public SnmpApplication Create(SnmpContext context)
        {
            SnmpApplication result = null;
            lock (_root)
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

            result.Init(context);
            return result;
        }

        /// <summary>
        /// Reuses the specified pipeline.
        /// </summary>
        /// <param name="application">The application.</param>
        internal void Reuse(SnmpApplication application)
        {
            lock (_root)
            {
                _queue.Enqueue(application);
            }
        }
    }
}