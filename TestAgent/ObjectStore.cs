using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ObjectStore
    {
        private readonly IDictionary<ObjectIdentifier, ISnmpObject> _table = new Dictionary<ObjectIdentifier, ISnmpObject>();
        private readonly LinkedList<ObjectIdentifier> _list = new LinkedList<ObjectIdentifier>();
        
        public ObjectStore()
        {
            ISnmpObject descr = new SysDescr();
            ISnmpObject objectId = new SysObjectId();
            ISnmpObject upTime = new SysUpTime();
            ISnmpObject contact = new SysContact();
            ISnmpObject name = new SysName();
            ISnmpObject location = new SysLocation();
            
            _table.Add(descr.Id, descr);
            _table.Add(objectId.Id, objectId);
            _table.Add(upTime.Id, upTime);
            _table.Add(contact.Id, contact);
            _table.Add(name.Id, name);
            _table.Add(location.Id, location);
            
            _list.AddLast(descr.Id);
            _list.AddLast(objectId.Id);
            _list.AddLast(upTime.Id);
            _list.AddLast(contact.Id);
            _list.AddLast(name.Id);
            _list.AddLast(location.Id);
        }
        
        public ISnmpObject GetObject(ObjectIdentifier oid)
        {
            if (_table.ContainsKey(oid))
            {
                return _table[oid];
            }

            return null;
        }
        
        public ISnmpObject GetNextObject(ObjectIdentifier oid)
        {
            foreach (ObjectIdentifier node in _list)
            {
                if (oid.CompareTo(node) < 0)
                {
                    return _table[node];
                }
                
                continue;
            }
            
            return null;
        }
    }
}
