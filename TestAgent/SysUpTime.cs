using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysUpTime : IScalarObject
    {
        private static readonly ObjectIdentifier _id = new ObjectIdentifier("1.3.6.1.2.1.1.3.0");

        public ISnmpData Get()
        {
            return new TimeTicks((uint)Environment.TickCount / 10);
        }

        public void Set(ISnmpData data)
        {
            throw new ReadOnlyException();
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
