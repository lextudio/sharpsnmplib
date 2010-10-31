using System;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// sysName object.
    /// </summary>
    public class SysName : ScalarObject
    {
        private OctetString _name = new OctetString(Environment.MachineName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SysName"/> class.
        /// </summary>
        public SysName()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"))
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public override ISnmpData Data
        {            
            get 
            { 
                return _name; 
            }
            
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("data");
                }

                _name = (OctetString)value;
            }
        }
    }
}
