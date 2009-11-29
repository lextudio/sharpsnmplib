using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysObjectId : IScalarObject
    {
        private static readonly ObjectIdentifier id = new ObjectIdentifier("1.3.6.1.2.1.1.2.0");
        private ObjectIdentifier _objectId = new ObjectIdentifier("1.3.6.1");

        public ISnmpData Data
        {
            get { return _objectId; }
            set { throw new AccessFailureException(); }
        }

        public ObjectIdentifier Id
        {
            get { return id; }
        }
    }
}
