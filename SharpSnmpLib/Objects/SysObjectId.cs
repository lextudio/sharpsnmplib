using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// SysObjectId object.
    /// </summary>
    public class SysObjectId : ScalarObject
    {
        private readonly ObjectIdentifier _objectId = new ObjectIdentifier("1.3.6.1");

        /// <summary>
        /// Initializes a new instance of the <see cref="SysObjectId"/> class.
        /// </summary>
        public SysObjectId()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public override ISnmpData Data
        {
            get { return _objectId; }
            set { throw new AccessFailureException(); }
        }
    }
}
