using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    // TODO: this is not accessible. So how to handle?
    internal sealed class SysORIndex : ScalarObject
    {
        private readonly ISnmpData _data;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SysORIndex(int index)
            : base("1.3.6.1.2.1.1.9.1.1.{0}", index)
        {
            _data = new Integer32(index);
        }

        public override ISnmpData Data
        {
            get { return _data; }
            set { throw new AccessFailureException(); }
        }
    }
}