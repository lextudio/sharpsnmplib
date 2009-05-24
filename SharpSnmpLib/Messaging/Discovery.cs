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
    /// Description of Discovery.
    /// </summary>
    public class Discovery
    {
        private GetRequestMessage discovery;
        
        public Discovery(int requestId, int messageId)
        {
            discovery = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(messageId),
                    new Integer32(0xFFE3),
                    new OctetString(new byte[] { (byte)SecurityLevel.Reportable }),
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
                    ProviderPair.Default
               );
        }
        
        public ReportMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return (ReportMessage)MessageFactory.GetResponse(receiver, discovery.ToBytes(), discovery.RequestId, timeout, new UserRegistry(), Messenger.GetSocket(receiver));
        }
    }
}
