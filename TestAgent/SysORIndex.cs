namespace Lextm.SharpSnmpLib.Agent
{
    // TODO: this is not accessible. So how to handle?
    internal class SysORIndex : ScalarObject
    {
        private readonly ISnmpData _data;

        public SysORIndex(int index)
            : base("1.3.6.1.2.1.1.9.1.{0}.1", index)
        {
            _data = new Integer32(index);
        }

        protected internal override ISnmpData Data
        {
            get { return _data; }
            set { throw new AccessFailureException(); }
        }
    }
}