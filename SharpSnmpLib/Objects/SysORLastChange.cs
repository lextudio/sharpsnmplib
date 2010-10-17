using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    public class SysORLastChange : ScalarObject
    {
        private readonly ISnmpData _value = new TimeTicks(0);

        public SysORLastChange()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.8.0"))
        {
        }

        public override ISnmpData Data
        {
            get { return _value; }
            set { throw new AccessFailureException(); }
        }
    }
}