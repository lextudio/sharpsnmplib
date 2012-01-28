// sysORTable class.
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

using System.Collections.Generic;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// sysORTable object.
    /// </summary>
    public sealed class SysORTable : TableObject
    {
        // "1.3.6.1.2.1.1.9.1"
        private readonly IList<ScalarObject> _elements = new List<ScalarObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SysORTable"/> class.
        /// </summary>
        public SysORTable()
        {
            _elements.Add(new SysORIndex(1));
            _elements.Add(new SysORIndex(2));
            _elements.Add(new SysORID(1, new ObjectIdentifier("1.3")));
            _elements.Add(new SysORID(2, new ObjectIdentifier("1.4")));
            _elements.Add(new SysORDescr(1, new OctetString("Test1")));
            _elements.Add(new SysORDescr(2, new OctetString("Test2")));
            _elements.Add(new SysORUpTime(1, new TimeTicks(1)));
            _elements.Add(new SysORUpTime(2, new TimeTicks(2)));
        }

        /// <summary>
        /// Gets the objects in the table.
        /// </summary>
        /// <value>The objects.</value>
        protected override IEnumerable<ScalarObject> Objects
        {
            get { return _elements; }
        }
    }
}