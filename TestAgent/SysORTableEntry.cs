using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysORTableEntry : EntryObject
    {
        private readonly IList<ScalarObject> _objects;

        public SysORTableEntry(IList<ScalarObject> objects)
        {
            _objects = objects;
        }

        protected override IEnumerable<ScalarObject> Objects
        {
            get { return _objects; }
        }
    }
}