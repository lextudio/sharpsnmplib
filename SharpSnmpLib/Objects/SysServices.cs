using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// sysServices object.
    /// </summary>
    public class SysServices : ScalarObject
    {
        private readonly Integer32 _value = new Integer32(72);

        /// <summary>
        /// Initializes a new instance of the <see cref="SysServices"/> class.
        /// </summary>
        public SysServices()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.7.0"))
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public override ISnmpData Data
        {
            get { return _value; }
            set { throw new AccessFailureException(); }
        }
    }
}