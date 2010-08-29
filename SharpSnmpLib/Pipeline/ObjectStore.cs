using System;
using System.Collections.Generic;
using System.Linq;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP object store, who holds all implemented SNMP objects in the agent.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class ObjectStore
    {
        private readonly IList<ISnmpObject> _list = new List<ISnmpObject>();

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="id">The oid.</param>
        /// <returns></returns>
        public ScalarObject GetObject(ObjectIdentifier id)
        {
            return _list.Select(o => o.MatchGet(id)).FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Gets the next object.
        /// </summary>
        /// <param name="id">The oid.</param>
        /// <returns></returns>
        public ScalarObject GetNextObject(ObjectIdentifier id)
        {
            return _list.Select(o => o.MatchGetNext(id)).FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Adds the specified <seealso cref="ISnmpObject"/>.
        /// </summary>
        /// <param name="newObject">The object.</param>
        public void Add(ISnmpObject newObject)
        {
            _list.Add(newObject);
        }
    }
}
