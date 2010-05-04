namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysORDescr : ScalarObject
    {
        private readonly OctetString _data;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SysORDescr(int index, OctetString description)
            : base("1.3.6.1.2.1.1.9.1.3.{0}", index)
        {
            _data = description;
        }

        protected internal override ISnmpData Data
        {
            get { return _data; }
            set { throw new AccessFailureException(); }
        }
    }
}