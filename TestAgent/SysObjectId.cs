using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysObjectId : IScalarObject
    {
        private static readonly ObjectIdentifier id = new ObjectIdentifier("1.3.6.1.2.1.1.2.0");
        private ISnmpObject _next;

        public ISnmpData Get()
        {
            return new ObjectIdentifier("1.3.6.1");
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

        public ISnmpObject Next
        {
            get { return _next; }
            set { _next = value; }
        }
    }
}
