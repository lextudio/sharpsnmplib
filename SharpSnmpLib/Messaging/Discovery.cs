/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/24/2009
 * Time: 11:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;

using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Discovery class that participates in SNMP v3 discovery process.
    /// </summary>
    public sealed class Discovery
    {
        private GetRequestMessage discovery;

        /// <summary>
        /// Initializes a new instance of the <see cref="Discovery"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="messageId">The message id.</param>
        public Discovery(int requestId, int messageId)
        {
            discovery = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(messageId),
                    new Integer32(0xFFE3),
                    new OctetString(new byte[] { (byte)Levels.Reportable }),
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
            return (ReportMessage)MessageFactory.GetResponse(receiver, discovery.ToBytes(), discovery.RequestId, timeout, new UserRegistry(), Helper.GetSocket(receiver));
        }
    }
}
