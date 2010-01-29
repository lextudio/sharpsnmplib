using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysORTable : TableObject
    {
        // "1.3.6.1.2.1.1.9.1"
        private readonly IList<EntryObject> _entries = new List<EntryObject>();

        public SysORTable()
        {
            List<ScalarObject> elements = new List<ScalarObject>();
            elements.Add(new SysORIndex(1));
            elements.Add(new SysORID(1, new ObjectIdentifier("1.3")));
            elements.Add(new SysORDescr(1, new OctetString("Test1")));
            elements.Add(new SysORUpTime(1, new TimeTicks(1)));
            _entries.Add(new SysORTableEntry(elements));

            List<ScalarObject> elements2 = new List<ScalarObject>();
            elements2.Add(new SysORIndex(2));
            elements2.Add(new SysORID(2, new ObjectIdentifier("1.4")));
            elements2.Add(new SysORDescr(2, new OctetString("Test2")));
            elements2.Add(new SysORUpTime(2, new TimeTicks(2)));
            _entries.Add(new SysORTableEntry(elements2));
        }

        protected override IEnumerable<EntryObject> Objects
        {
            get { return _entries; }
        }
    }
}