using System;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SysUpTime object.
    /// </summary>
    internal class SysUpTime : ScalarObject
    {
        private readonly ISnmpData _upTime = new TimeTicks((uint)Environment.TickCount / 10);

        /// <summary>
        /// Initializes a new instance of the <see cref="SysUpTime"/> class.
        /// </summary>
        public SysUpTime()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"))
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        protected internal override ISnmpData Data
        {
            get { return _upTime; }
            set { throw new AccessFailureException(); }
        }
    }
}
