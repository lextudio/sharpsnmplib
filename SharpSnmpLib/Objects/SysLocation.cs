using System;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// sysLocation object.
    /// </summary>
    public class SysLocation : ScalarObject
    {
        private OctetString _location = OctetString.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SysLocation"/> class.
        /// </summary>
        public SysLocation()
            : base(new ObjectIdentifier("1.3.6.1.2.1.1.6.0"))
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
                return _location; 
            }
            
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("wrong data type", "value");
                }

                _location = (OctetString)value;
            }
        }
    }
}
