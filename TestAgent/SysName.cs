using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysName: IScalarObject
    {
        private static readonly ObjectIdentifier _id = new ObjectIdentifier("1.3.6.1.2.1.1.5.0");
        private OctetString _name = OctetString.Empty;
        
        public ISnmpData Get()
        {
            return new OctetString(Environment.MachineName);
        }

        public void Set(ISnmpData data)
        {
            if (data.TypeCode != SnmpType.OctetString)
            {
                throw new ArgumentException("data");
            }
            
            _name = (OctetString)data;
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
