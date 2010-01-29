namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysORDescr : ScalarObject
    {
        private readonly ISnmpData _data;

        public SysORDescr(int index, OctetString description)
            : base("1.3.6.1.2.1.1.9.1.{0}.3", index)
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