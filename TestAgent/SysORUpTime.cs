namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysORUpTime : ScalarObject
    {
        private readonly ISnmpData _data;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SysORUpTime(int index, TimeTicks time)
            : base("1.3.6.1.2.1.1.9.1.4.{0}", index)
        {
            _data = time;
        }

        protected internal override ISnmpData Data
        {
            get { return _data; }
            set { throw new AccessFailureException(); }
        }
    }
}