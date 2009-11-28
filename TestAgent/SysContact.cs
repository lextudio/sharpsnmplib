using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysContact : IScalarObject
    {
        private static readonly ObjectIdentifier _id = new ObjectIdentifier("1.3.6.1.2.1.1.4.0");
        private OctetString _contact = new OctetString(Environment.UserName);
        
        public ISnmpData Get()
        {
            return _contact;
        }

        public void Set(ISnmpData data)
        {
            if (data.TypeCode != SnmpType.OctetString)
            {
                throw new ArgumentException("data");
            }
            
            _contact = (OctetString)data;
        }

        public ObjectIdentifier Id
        {
            get
            {
                return _id;
            }
        }
    }
}
