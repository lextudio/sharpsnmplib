// sysName class.
// Copyright (C) 2009-2010 Lex Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

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
