// GET BULK request message type.
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
 * Date: 2008/8/3
 * Time: 15:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// GETBULK request message.
    /// </summary>
    public sealed class GetBulkRequestMessage : ISnmpMessage
    {
        private readonly byte[] _bytes;

        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="community">Community name.</param>
        /// <param name="nonRepeaters">Non-repeaters.</param>
        /// <param name="maxRepetitions">Max repetitions.</param>
        /// <param name="variables">Variables.</param>
        public GetBulkRequestMessage(int requestId, VersionCode version, OctetString community, int nonRepeaters, int maxRepetitions, IList<Variable> variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }
            
            if (community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }
            
            if (version != VersionCode.V2)
            {
                throw new ArgumentException("Only v2c are supported.", nameof(version));
            }

            if (nonRepeaters > variables.Count)
            {
                throw new ArgumentException("nonRepeaters should not be greater than variable count.", nameof(nonRepeaters));
            }

            if (maxRepetitions < 1)
            {
                throw new ArgumentException("maxRepetitions should be greater than 0.", nameof(maxRepetitions));
            }

            Version = version;
            Header = Header.Empty;
            Parameters = SecurityParameters.Create(community);
            var pdu = new GetBulkRequestPdu(
                requestId,
                nonRepeaters,
                maxRepetitions,
                variables);
            Scope = new Scope(pdu);
            Privacy = DefaultPrivacyProvider.DefaultPair;

            _bytes = this.PackMessage(null).ToBytes();
        }

        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="nonRepeaters">The non repeaters.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        [Obsolete("Please use other overloading ones.")]
        public GetBulkRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, int nonRepeaters, int maxRepetitions, IList<Variable> variables, IPrivacyProvider privacy, ISnmpMessage report)
            : this(version, messageId, requestId, userName, nonRepeaters, maxRepetitions, variables, privacy, 0xFFE3, report)
        {
        }

        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="contextName">Context name.</param>
        /// <param name="nonRepeaters">The non repeaters.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="report">The report.</param>
        public GetBulkRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, OctetString contextName, int nonRepeaters, int maxRepetitions, IList<Variable> variables, IPrivacyProvider privacy, int maxMessageSize, ISnmpMessage report)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
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

            if (nonRepeaters > variables.Count)
            {
                throw new ArgumentException("nonRepeaters should not be greater than variable count.", nameof(nonRepeaters));
            }

            if (maxRepetitions < 1)
            {
                throw new ArgumentException("maxRepetitions should be greater than 0.", nameof(maxRepetitions));
            }

            Version = version;
            Privacy = privacy;

            // TODO: define more constants.
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
            var pdu = new GetBulkRequestPdu(
                requestId,
                nonRepeaters,
                maxRepetitions,
                variables);
            var scope = report.Scope;
            var contextEngineId = scope.ContextEngineId == OctetString.Empty ? parameters.EngineId : scope.ContextEngineId;
            Scope = new Scope(contextEngineId, contextName, pdu);

            Privacy.ComputeHash(Version, Header, Parameters, Scope);
            _bytes = this.PackMessage(null).ToBytes();
        }


        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="nonRepeaters">The non repeaters.</param>
        /// <param name="maxRepetitions">The max repetitions.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="report">The report.</param>
        public GetBulkRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, int nonRepeaters, int maxRepetitions, IList<Variable> variables, IPrivacyProvider privacy, int maxMessageSize, ISnmpMessage report) 
            : this(version, messageId, requestId, userName, OctetString.Empty, nonRepeaters, maxRepetitions, variables, privacy, maxMessageSize, report)
        {
        }

        internal GetBulkRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy, byte[] length)
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
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version { get; private set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetBulkRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "GET BULK request message: version: {0}; {1}; {2}", Version, Parameters.UserName, Scope.Pdu);
        }
    }
}
