using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    public class SysServices : ScalarObject
    {
        private readonly Integer32 _value = new Integer32(72);

        public SysServices()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.7.0"))
        {
        }

        public override ISnmpData Data
        {
            get { return _value; }
            set { throw new AccessFailureException(); }
        }
    }
}