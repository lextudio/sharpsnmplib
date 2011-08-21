// Object store class.
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
using System.Linq;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP object store, who holds all implemented SNMP objects in the agent.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public sealed class ObjectStore
    {
        private readonly IList<ISnmpObject> _list = new List<ISnmpObject>();

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="id">The oid.</param>
        /// <returns></returns>
        public ScalarObject GetObject(ObjectIdentifier id)
        {
            return _list.Select(o => o.MatchGet(id)).FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Gets the next object.
        /// </summary>
        /// <param name="id">The oid.</param>
        /// <returns></returns>
        public ScalarObject GetNextObject(ObjectIdentifier id)
        {
            return _list.Select(o => o.MatchGetNext(id)).FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Adds the specified <see cref="ISnmpObject"/>.
        /// </summary>
        /// <param name="newObject">The object.</param>
        public void Add(ISnmpObject newObject)
        {
            _list.Add(newObject);
        }
    }
}
