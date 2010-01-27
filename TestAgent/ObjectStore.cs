using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP object store, who holds all implemented SNMP objects in the agent.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ObjectStore
    {
        private readonly IList<ISnmpObject> _list = new List<ISnmpObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectStore"/> class.
        /// </summary>
        public ObjectStore()
        {
            _list.Add(new SysDescr());
            _list.Add(new SysObjectId());
            _list.Add(new SysUpTime());
            _list.Add(new SysContact());
            _list.Add(new SysName());
            _list.Add(new SysLocation());
            _list.Add(new SysServices());
            _list.Add(new SysORLastChange());
            _list.Add(new SysORTable());
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public ScalarObject GetObject(ObjectIdentifier oid)
        {
            foreach (ISnmpObject o in _list)
            {
                ScalarObject result = o.MatchGet(oid);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the next object.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public ScalarObject GetNextObject(ObjectIdentifier oid)
        {
            foreach (ISnmpObject o in _list)
            {
                ScalarObject result = o.MatchGetNext(oid);
                if (result != null)
                {
                    return result;
                }
            }
            
            return null;
        }
    }
}
