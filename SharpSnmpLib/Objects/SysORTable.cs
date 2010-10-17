using System.Collections.Generic;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    public class SysORTable : TableObject
    {
        // "1.3.6.1.2.1.1.9.1"
        private readonly IList<ScalarObject> _elements = new List<ScalarObject>();

        public SysORTable()
        {
            _elements.Add(new SysORIndex(1));
            _elements.Add(new SysORIndex(2));
            _elements.Add(new SysORID(1, new ObjectIdentifier("1.3")));
            _elements.Add(new SysORID(2, new ObjectIdentifier("1.4")));
            _elements.Add(new SysORDescr(1, new OctetString("Test1")));
            _elements.Add(new SysORDescr(2, new OctetString("Test2")));
            _elements.Add(new SysORUpTime(1, new TimeTicks(1)));
            _elements.Add(new SysORUpTime(2, new TimeTicks(2)));
        }

        protected override IEnumerable<ScalarObject> Objects
        {
            get { return _elements; }
        }
    }
}