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
            ISnmpObject descr = new SysDescr();
            ISnmpObject objectId = new SysObjectId();
            ISnmpObject upTime = new SysUpTime();
            ISnmpObject contact = new SysContact();
            ISnmpObject name = new SysName();
            ISnmpObject location = new SysLocation();
    
            _list.Add(descr);
            _list.Add(objectId);
            _list.Add(upTime);
            _list.Add(contact);
            _list.Add(name);
            _list.Add(location);
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public ISnmpObject GetObject(ObjectIdentifier oid)
        {
            foreach (ISnmpObject o in _list)
            {
                if (o.MatchGet(oid))
                {
                    return o;
                }

                continue;
            }

            return null;
        }

        /// <summary>
        /// Gets the next object.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns></returns>
        public ISnmpObject GetNextObject(ObjectIdentifier oid)
        {
            foreach (ISnmpObject o in _list)
            {
                if (o.MatchGetNext(oid))
                {
                    return o;
                }
                
                continue;
            }
            
            return null;
        }
    }
}
