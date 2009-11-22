using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class ObjectStore
    {
        private IDictionary<ObjectIdentifier, ISnmpObject> _table = new Dictionary<ObjectIdentifier, ISnmpObject>();

        public ObjectStore()
        {
            ISnmpObject descr = new SysDescr();
            ISnmpObject objectId = new SysObjectId();
            descr.Next = objectId;
            ISnmpObject upTime = new SysUpTime();
            objectId.Next = upTime;
            ISnmpObject contact = new SysContact();
            upTime.Next = contact;
            ISnmpObject name = new SysName();
            contact.Next = name;
            ISnmpObject location = new SysLocation();
            name.Next = location;
            _table.Add(descr.Id, descr);
            _table.Add(objectId.Id, objectId);
            _table.Add(upTime.Id, upTime);
            _table.Add(contact.Id, contact);
            _table.Add(name.Id, name);
            _table.Add(location.Id, location);
        }
        
        public ISnmpObject GetObject(ObjectIdentifier oid)
        {
            if (_table.ContainsKey(oid))
            {
                return _table[oid];
            }

            return null;
        }
    }
}
