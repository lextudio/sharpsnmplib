// SNMP application factory class.
// Copyright (C) 2009-2010 Lex Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP application factory, who holds all pipeline instances.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public sealed class SnmpApplicationFactory
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
        public SnmpApplicationFactory(ILogger logger, ObjectStore store, IMembershipProvider membershipProvider, MessageHandlerFactory factory)
        {
            _logger = logger;
            _membershipProvider = membershipProvider;
            _store = store;
            _factory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpApplicationFactory"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="membershipProvider">The membership provider.</param>
        /// <param name="factory">The factory.</param>
        public SnmpApplicationFactory(ObjectStore store, IMembershipProvider membershipProvider, MessageHandlerFactory factory)
            : this(null, store, membershipProvider, factory) // TODO: handle the null case in the future.
        {
        }

        /// <summary>
        /// Creates a pipeline for the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public SnmpApplication Create(ISnmpContext context)
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