using System;
using System.Globalization;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SysDescr object.
    /// </summary>
    internal class SysDescr : ScalarObject
    {
        private readonly OctetString _description =
            new OctetString(string.Format(CultureInfo.InvariantCulture, "#SNMP Agent on {0}", Environment.OSVersion));

        /// <summary>
        /// Initializes a new instance of the <see cref="SysDescr"/> class.
        /// </summary>
        public SysDescr()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public override ISnmpData Data
        {
            get { return _description; }
            set { throw new AccessFailureException(); }
        }
    }
}
