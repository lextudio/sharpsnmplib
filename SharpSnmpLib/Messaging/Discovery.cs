// Discovery type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/24/2009
 * Time: 11:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Discovery class that participates in SNMP v3 discovery process.
    /// </summary>
    public sealed class Discovery
    {
        private readonly GetRequestMessage _discovery;

        /// <summary>
        /// Initializes a new instance of the <see cref="Discovery"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="messageId">The message id.</param>
        public Discovery(int requestId, int messageId)
        {
            _discovery = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(messageId),
                    new Integer32(0xFFE3),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    OctetString.Empty,
                    new Integer32(0),
                    new Integer32(0),
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    OctetString.Empty,
                    OctetString.Empty,
                    new GetRequestPdu(requestId, ErrorCode.NoError, 0, new List<Variable>())),
                ProviderPair.Default);
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        public ReportMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            using (Socket socket = Helper.GetSocket(receiver))
            {
                return (ReportMessage)MessageFactory.GetResponse(receiver, ToBytes(), _discovery.RequestId, timeout, UserRegistry.Empty, socket);
            }
        }

        /// <summary>
        /// Converts to the bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _discovery.ToBytes();
        }
    }
}
