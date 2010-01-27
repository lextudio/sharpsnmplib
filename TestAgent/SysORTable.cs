using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysORTable : VectorObject
    {
        private readonly IEnumerable<ScalarObject> _internal;

        public SysORTable()
        {
            _internal = new List<ScalarObject>();

        }

        protected override IEnumerable<ScalarObject> Objects
        {
            get { return _internal; }
        }
    }
}