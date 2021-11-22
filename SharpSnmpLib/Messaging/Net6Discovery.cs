// Discovery type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
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
 * User: lextm
 * Date: 5/24/2009
 * Time: 11:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Discovery class that participates in SNMP v3 discovery process.
    /// </summary>
    public sealed partial class Discovery
    {
        #if NET6_0
        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        public async Task<ReportMessage> GetResponseAsync(IPEndPoint receiver, CancellationToken token)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            using var socket = receiver.GetSocket();
            return (ReportMessage)await _discovery.GetResponseAsync(receiver, Empty, socket, token).ConfigureAwait(false);
        }
        #endif
    }
}
