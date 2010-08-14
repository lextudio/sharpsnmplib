using System;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SysUpTime object.
    /// </summary>
    internal class SysUpTime : ScalarObject
    {
        private readonly ISnmpData _time = new TimeTicks((uint)Environment.TickCount / 10);

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
        public override ISnmpData Data
        {
            get { return _time; }
            set { throw new AccessFailureException(); }
        }
    }
}
