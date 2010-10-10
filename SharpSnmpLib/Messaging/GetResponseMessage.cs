// GET response message type.
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
 * Date: 2008/5/1
 * Time: 18:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// GET response message.
    /// </summary>
    public class GetResponseMessage : ISnmpMessage
    {
        private readonly Header _header;
        private readonly SecurityParameters _parameters;
        private readonly Scope _scope;
        private readonly VersionCode _version;
        private readonly IPrivacyProvider _privacy;

        /// <summary>
        /// Creates a <see cref="GetResponseMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">Request ID.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="community">Community name.</param>
        /// <param name="error">Error code.</param>
        /// <param name="index">Error index.</param>
        /// <param name="variables">Variables.</param>
        public GetResponseMessage(int requestId, VersionCode version, OctetString community, ErrorCode error, int index, IList<Variable> variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }
            
            if (community == null)
            {
                throw new ArgumentNullException("community");
            }
            
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("Please use overload constructor for v3", "version");
            }

            _version = version;
            _header = Header.Empty;
            _parameters = new SecurityParameters(null, null, null, community, null, null);
            GetResponsePdu pdu = new GetResponsePdu(
                requestId,
                error,
                index,
                variables);
            _scope = new Scope(pdu);
            _privacy = DefaultPrivacyProvider.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetResponseMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="privacy">The privacy provider.</param>
        public GetResponseMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            _version = version;
            _header = header;
            _parameters = parameters;
            _scope = scope;
            _privacy = privacy;
        }
      
        /// <summary>
        /// Error status.
        /// </summary>
        public ErrorCode ErrorStatus
        {
            get { return _scope.Pdu.ErrorStatus.ToErrorCode(); }
        }
        
        /// <summary>
        /// Error index.
        /// </summary>
        public int ErrorIndex
        {
            get { return _scope.Pdu.ErrorIndex.ToInt32(); }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Request ID.
        /// </summary>
        public int RequestId
        {
            get { return _scope.Pdu.RequestId.ToInt32(); }
        }
        
        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        /// <remarks>For v3, message ID is different from request ID. For v1 and v2c, they are the same.</remarks>
        public int MessageId
        {
            get
            {
                return (_header == Header.Empty) ? RequestId : _header.MessageId;
            }
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get { return _scope.Pdu.Variables; }
        }
        
        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu
        {
            get { return _scope.Pdu; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public SecurityParameters Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public Scope Scope
        {
            get { return _scope; }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Helper.PackMessage(_version, _privacy, _header, _parameters, _scope).ToBytes();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="GetResponseMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET response message: version: " + _version + "; " + _parameters.UserName + "; " + _scope.Pdu;
        }
    }
}
