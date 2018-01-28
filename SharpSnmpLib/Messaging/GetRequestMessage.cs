// GET request message type.
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
    /// GET request message.
    /// </summary>
    public sealed class GetRequestMessage : ISnmpMessage
    {
        private readonly byte[] _bytes;
        
        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public GetRequestMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }
            
            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }
            
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("Only v1 and v2c are supported.", nameof(version));
            }
            
            Version = version;
            Header = Header.Empty;
            Parameters = SecurityParameters.Create(community);
            var pdu = new GetRequestPdu(
                requestId,
                variables);
            Scope = new Scope(pdu);
            Privacy = DefaultPrivacyProvider.DefaultPair;

            _bytes = this.PackMessage(null).ToBytes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        [Obsolete("Please use other overloading ones.")]
        public GetRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, IList<Variable> variables, IPrivacyProvider privacy, ISnmpMessage report)
            : this(version, messageId, requestId, userName, variables, privacy, 0xFFE3, report)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="contextName">Context name.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="report">The report.</param>
        public GetRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, OctetString contextName, IList<Variable> variables, IPrivacyProvider privacy, int maxMessageSize, ISnmpMessage report)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (contextName == null)
            {
                throw new ArgumentNullException(nameof(contextName));
            }

            if (version != VersionCode.V3)
            {
                throw new ArgumentException("Only v3 is supported.", nameof(version));
            }

            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            Version = version;
            Privacy = privacy;

            Header = new Header(new Integer32(messageId), new Integer32(maxMessageSize), privacy.ToSecurityLevel() | Levels.Reportable);
            var parameters = report.Parameters;
            var authenticationProvider = Privacy.AuthenticationProvider;
            Parameters = new SecurityParameters(
                parameters.EngineId,
                parameters.EngineBoots,
                parameters.EngineTime,
                userName,
                authenticationProvider.CleanDigest,
                Privacy.Salt);
            var pdu = new GetRequestPdu(
                requestId,
                variables);
            var scope = report.Scope;
            var contextEngineId = scope.ContextEngineId == OctetString.Empty ? parameters.EngineId : scope.ContextEngineId;
            Scope = new Scope(contextEngineId, contextName, pdu);

            Privacy.ComputeHash(Version, Header, Parameters, Scope);
            _bytes = this.PackMessage(null).ToBytes();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="report">The report.</param>
        public GetRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, IList<Variable> variables, IPrivacyProvider privacy, int maxMessageSize, ISnmpMessage report)
           : this(version, messageId, requestId, userName, OctetString.Empty, variables, privacy, maxMessageSize, report)
        {
        }

        internal GetRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy, byte[] length)
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

            _bytes = this.PackMessage(length).ToBytes();
        }

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
        /// Version.
        /// </summary>
        public VersionCode Version { get; private set; }

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _bytes;
        }

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

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "GET request message: version: {0}; {1}; {2}", Version, this.Community(), this.Pdu());
        }
    }
}
