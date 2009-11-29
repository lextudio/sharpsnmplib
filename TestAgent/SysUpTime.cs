using System;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysUpTime : IScalarObject
    {
        private static readonly ObjectIdentifier Identifier = new ObjectIdentifier("1.3.6.1.2.1.1.3.0");
        private readonly ISnmpData _upTime = new TimeTicks((uint)Environment.TickCount / 10);

        public ISnmpData Data
        {
            get { return _upTime; }
            set { throw new AccessFailureException(); }
        }

        public ObjectIdentifier Id
        {
            get { return Identifier; }
        }
    }
}
