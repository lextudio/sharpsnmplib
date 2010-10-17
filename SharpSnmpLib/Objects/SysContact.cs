using System;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// SysContact object.
    /// </summary>
    public class SysContact : ScalarObject
    {
        private OctetString _contact = new OctetString(Environment.UserName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SysContact"/> class.
        /// </summary>
        public SysContact() : base(new ObjectIdentifier("1.3.6.1.2.1.1.4.0"))
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
                return _contact; 
            }
            
            set
            {
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("wrong data type", "value");
                }

                _contact = (OctetString)value;
            }
        }
    }
}
