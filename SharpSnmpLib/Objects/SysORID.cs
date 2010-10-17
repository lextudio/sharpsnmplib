using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    internal class SysORID : ScalarObject
    {
        private readonly ObjectIdentifier _data;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SysORID(int index, ObjectIdentifier dots)
            : base("1.3.6.1.2.1.1.9.1.2.{0}", index)
        {
            _data = dots;
        }

        public override ISnmpData Data
        {
            get { return _data; }
            set { throw new AccessFailureException(); }
        }
    }
}