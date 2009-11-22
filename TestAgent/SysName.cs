using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysName: IScalarObject
    {
        private static readonly ObjectIdentifier id = new ObjectIdentifier("1.3.6.1.2.1.1.5.0");

        public ISnmpData Get()
        {
            return new OctetString(Environment.MachineName);
        }

        public void Set(ISnmpData data)
        {
            throw new NotImplementedException();
        }

        public ObjectIdentifier Id
        {
            get
            {
                return id;
            }
        }
    }
}
