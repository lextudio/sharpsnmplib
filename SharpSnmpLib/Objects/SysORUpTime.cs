using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    internal sealed class SysORUpTime : ScalarObject
    {
        private readonly TimeTicks _data;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SysORUpTime(int index, TimeTicks time)
            : base("1.3.6.1.2.1.1.9.1.4.{0}", index)
        {
            _data = time;
        }

        public override ISnmpData Data
        {
            get { return _data; }
            set { throw new AccessFailureException(); }
        }
    }
}