// TRAP message type (SNMP version 2 and above).
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

using System;
using System.Collections.Generic;
using System.Globalization;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// TRAP v2 message.
    /// </summary>
    public sealed class TrapV2Message : ISnmpMessage
    {
        private readonly byte[] _bytes;

        /// <summary>
        /// Creates a <see cref="TrapV2Message"/> instance with all content.
        /// </summary>
        /// <param name="version">Version code.</param>
        /// <param name="community">Community.</param>
        /// <param name="enterprise">Enterprise.</param>
        /// <param name="time">Time stamp.</param>
        /// <param name="variables">Variables.</param>
        /// <param name="requestId">Request ID.</param>
        [CLSCompliant(false)]
        public TrapV2Message(int requestId, VersionCode version, OctetString community, ObjectIdentifier enterprise, uint time, IList<Variable> variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }
            
            if (enterprise == null)
            {
                throw new ArgumentNullException(nameof(enterprise));
            }
            
            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }
            
            if (version != VersionCode.V2)
            {
                throw new ArgumentException("Only v2c are supported.", nameof(version));
            }
            
            Version = version;
            Enterprise = enterprise;
            TimeStamp = time;
            Header = Header.Empty;
            Parameters = SecurityParameters.Create(community);
            var pdu = new TrapV2Pdu(
                requestId,
                enterprise,
                time,
                variables);
            Scope = new Scope(pdu);
            Privacy = DefaultPrivacyProvider.DefaultPair;
            _bytes = this.PackMessage(null).ToBytes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrapV2Message"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="enterprise">The enterprise.</param>
        /// <param name="time">The time.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="engineId">The engine ID.</param>
        /// <param name="engineBoots">The engine boots.</param>
        /// <param name="engineTime">The engine time.</param>
        [CLSCompliant(false)]
        public TrapV2Message(VersionCode version, int messageId, int requestId, OctetString userName, ObjectIdentifier enterprise, uint time, IList<Variable> variables, IPrivacyProvider privacy, int maxMessageSize, OctetString engineId, int engineBoots, int engineTime)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }
            
            if (version != VersionCode.V3)
            {
                throw new ArgumentException("Only v3 is supported.", nameof(version));
            }

            if (enterprise == null)
            {
                throw new ArgumentNullException(nameof(enterprise));
            }

            if (engineId == null)
            {
                throw new ArgumentNullException(nameof(engineId));
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            Version = version;
            Privacy = privacy;
            Enterprise = enterprise;
            TimeStamp = time;

            Header = new Header(new Integer32(messageId), new Integer32(maxMessageSize), privacy.ToSecurityLevel());
            var authenticationProvider = Privacy.AuthenticationProvider;
            Parameters = new SecurityParameters(
                engineId,
                new Integer32(engineBoots), 
                new Integer32(engineTime), 
                userName,
                authenticationProvider.CleanDigest,
                Privacy.Salt);
            var pdu = new TrapV2Pdu(
                requestId,
                enterprise,
                time,
                variables);
            
            // TODO: may expose engine ID in the future.
            Scope = new Scope(OctetString.Empty, OctetString.Empty, pdu);
            Privacy.ComputeHash(Version, Header, Parameters, Scope);
            _bytes = this.PackMessage(null).ToBytes();
        }

        internal TrapV2Message(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy, byte[] length)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }
            
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            Version = version;
            Header = header;
            Parameters = parameters;
            Scope = scope;
            Privacy = privacy;
            var pdu = (TrapV2Pdu)Scope.Pdu;
            Enterprise = pdu.Enterprise;
            TimeStamp = pdu.TimeStamp;
            _bytes = this.PackMessage(length).ToBytes();
        }
        
        #region ISnmpMessage Members

        /// <summary>
        /// Gets the header.
        /// </summary>
        public Header Header { get; private set; }
        
        /// <summary>
        /// Gets the privacy provider.
        /// </summary>
        /// <value>The privacy provider.</value>
        public IPrivacyProvider Privacy { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public SecurityParameters Parameters { get; private set; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public Scope Scope { get; private set; }

        #endregion

        #region ISnmpData Members

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _bytes;
        }

        #endregion

        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; private set; }

        /// <summary>
        /// Time stamp.
        /// </summary>
        [CLSCompliant(false), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version { get; private set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="TrapV2Message"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SNMPv2 trap: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                TimeStamp.ToString(CultureInfo.InvariantCulture),
                this.Community(),
                Enterprise,
                this.Variables().Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
