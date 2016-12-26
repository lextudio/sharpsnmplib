// ifTable class.
// Copyright (C) 2013 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

/*
 * Created by SharpDevelop.
 * User: Lex
 * Date: 3/3/2013
 * Time: 10:08 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Objects
{
    /// <summary>
    /// ifTable object.
    /// </summary>
    public sealed class IfTable : TableObject
    {
        // "1.3.6.1.2.1.2.2"
        private readonly IList<ScalarObject> _elements = new List<ScalarObject>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IfTable"/> class.
        /// </summary>
        public IfTable()
        {
            NetworkChange.NetworkAddressChanged +=
                (sender, args) => LoadElements();
#if !NETSTANDARD
            NetworkChange.NetworkAvailabilityChanged +=
                (sender, args) => LoadElements();
#endif                
            LoadElements();
        }

        private void LoadElements()
        {
            _elements.Clear();
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            var columnTypes = new[]
                {
                    typeof(IfIndex),
                    typeof(IfDescr),
                    typeof(IfType),
                    typeof(IfMtu),
                    typeof(IfSpeed),
                    typeof(IfPhysAddress),
                    typeof(IfAdminStatus),
                    typeof(IfOperStatus),
                    typeof(IfLastChange),
                    typeof(IfInOctets),
                    typeof(IfInUcastPkts),
                    typeof(IfInNUcastPkts),
                    typeof(IfInDiscards),
                    typeof(IfInErrors),
                    typeof(IfInUnknownProtos),
                    typeof(IfOutOctets),
                    typeof(IfOutUcastPkts),
                    typeof(IfOutNUcastPkts),
                    typeof(IfOutDiscards),
                    typeof(IfOutErrors),
                    typeof(IfOutQLen),
                    typeof(IfSpecific)
                };
            foreach (var type in columnTypes)
            {
                for (int i = 0; i < interfaces.Length; i++)
                {
                    _elements.Add((ScalarObject)Activator.CreateInstance(type, new object[] { i + 1, interfaces[i] }));
                }
            }
        }

        /// <summary>
        /// Gets the objects in the table.
        /// </summary>
        /// <value>
        /// The objects.
        /// </value>
        protected override IEnumerable<ScalarObject> Objects 
        {
            get { return _elements; }
        }
    }
}
