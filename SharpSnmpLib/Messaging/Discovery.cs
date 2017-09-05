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
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Lextm.SharpSnmpLib.Security;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Discovery class that participates in SNMP v3 discovery process.
    /// </summary>
    public sealed class Discovery
    {
        private readonly ISnmpMessage _discovery;
        private static readonly UserRegistry Empty = new UserRegistry();
        private static readonly SecurityParameters DefaultSecurityParameters =
            new SecurityParameters(
                OctetString.Empty,
                Integer32.Zero,
                Integer32.Zero,
                OctetString.Empty,
                OctetString.Empty,
                OctetString.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="Discovery"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="maxMessageSize">The max size of message.</param>
        public Discovery(int messageId, int requestId, int maxMessageSize)
        {
            _discovery = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(messageId),
                    new Integer32(maxMessageSize),
                    Levels.Reportable),
                DefaultSecurityParameters,
                new Scope(
                    OctetString.Empty,
                    OctetString.Empty,
                    new GetRequestPdu(requestId, new List<Variable>())),
                DefaultPrivacyProvider.DefaultPair,
                null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Discovery"/> class.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="maxMessageSize">The max size of message.</param>
        /// <param name="type">Message type.</param>
        public Discovery(int messageId, int requestId, int maxMessageSize, SnmpType type)
        {
            switch (type)
            {
                case SnmpType.GetRequestPdu:
                    {
                        _discovery = new GetRequestMessage(
                            VersionCode.V3,
                            new Header(
                            new Integer32(messageId),
                            new Integer32(maxMessageSize),
                            Levels.Reportable),
                            DefaultSecurityParameters,
                            new Scope(
                            OctetString.Empty,
                            OctetString.Empty,
                            new GetRequestPdu(requestId, new List<Variable>())),
                            DefaultPrivacyProvider.DefaultPair,
                            null);
                        break;
                    }

                case SnmpType.GetNextRequestPdu:
                    {
                        _discovery = new GetNextRequestMessage(
                            VersionCode.V3,
                            new Header(
                            new Integer32(messageId),
                            new Integer32(maxMessageSize),
                            Levels.Reportable),
                            DefaultSecurityParameters,
                            new Scope(
                            OctetString.Empty,
                            OctetString.Empty,
                            new GetNextRequestPdu(requestId, new List<Variable>())),
                            DefaultPrivacyProvider.DefaultPair,
                            null);
                        break;
                    }

                case SnmpType.GetBulkRequestPdu:
                    {
                        _discovery = new GetBulkRequestMessage(
                            VersionCode.V3,
                            new Header(
                            new Integer32(messageId),
                            new Integer32(maxMessageSize),
                            Levels.Reportable),
                            DefaultSecurityParameters,
                            new Scope(
                            OctetString.Empty,
                            OctetString.Empty,
                            new GetBulkRequestPdu(requestId, 0, 0, new List<Variable>())),
                            DefaultPrivacyProvider.DefaultPair,
                            null);
                        break;
                    }

                case SnmpType.SetRequestPdu:
                    {
                        _discovery = new SetRequestMessage(
                            VersionCode.V3,
                            new Header(
                            new Integer32(messageId),
                            new Integer32(maxMessageSize),
                            Levels.Reportable),
                            DefaultSecurityParameters,
                            new Scope(
                            OctetString.Empty,
                            OctetString.Empty,
                            new SetRequestPdu(requestId, new List<Variable>())),
                            DefaultPrivacyProvider.DefaultPair,
                            null);
                        break;
                    }

                case SnmpType.InformRequestPdu:
                    {
                        _discovery = new InformRequestMessage(
                            VersionCode.V3,
                            new Header(
                            new Integer32(messageId),
                            new Integer32(maxMessageSize),
                            Levels.Reportable),
                            DefaultSecurityParameters,
                            new Scope(
                            OctetString.Empty,
                            OctetString.Empty,
                            new InformRequestPdu(requestId)),
                            DefaultPrivacyProvider.DefaultPair,
                            null);
                        break;
                    }

                default:
                    throw new ArgumentException("Discovery message must be a request.", nameof(type));
            }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        public ReportMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            using (var socket = receiver.GetSocket())
            {
                return (ReportMessage)_discovery.GetResponse(timeout, receiver, Empty, socket);
            }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        public async Task<ReportMessage> GetResponseAsync(IPEndPoint receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            using (var socket = receiver.GetSocket())
            {
                return (ReportMessage)await _discovery.GetResponseAsync(receiver, Empty, socket).ConfigureAwait(false);
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

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "discovery class: message id: {0}; request id: {1}", _discovery.MessageId(), _discovery.RequestId());
        }
    }
}
