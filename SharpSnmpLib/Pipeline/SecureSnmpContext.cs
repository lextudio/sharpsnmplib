// Secure SNMP context class.
// Copyright (C) 2009-2010 Lex Li
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

using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Secure SNMP context. It is specific to v3.
    /// </summary>
    internal sealed class SecureSnmpContext : SnmpContextBase
    {
        private static readonly ObjectIdentifier ReportId = new ObjectIdentifier("1.3.6.1.6.3.15.1.1.4.0");
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureSnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="users">The users.</param>
        /// <param name="group">The engine core group.</param>
        /// <param name="binding">The binding.</param>
        public SecureSnmpContext(ISnmpMessage request, IPEndPoint sender, UserRegistry users, EngineGroup group, IListenerBinding binding)
            : base(request, sender, users, group, binding)
        {
        }

        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        public override void HandleAuthenticationFailure()
        {
            var defaultPair = DefaultPrivacyProvider.DefaultPair;
            Response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId()),
                    new Integer32(Messenger.MaxMessageSize),
                    0), // no need to encrypt.
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(Group.EngineBoots),
                    new Integer32(Group.EngineTime),
                    Request.Parameters.UserName,
                    defaultPair.AuthenticationProvider.CleanDigest,
                    defaultPair.Salt),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ResponsePdu(
                        Request.RequestId(),
                        ErrorCode.AuthorizationError,
                        0,
                        Request.Pdu().Variables)),
                defaultPair,
                true);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }

        public override void CopyRequest(ErrorCode status, int index)
        {
            var userName = Request.Parameters.UserName;
            var privacy = Users.Find(userName);
            Response = new ResponseMessage(
                    Request.Version,
                    new Header(
                        new Integer32(Request.MessageId()),
                        new Integer32(Messenger.MaxMessageSize),
                        privacy.ToSecurityLevel()),
                    new SecurityParameters(
                        Group.EngineId,
                        new Integer32(Group.EngineBoots),
                        new Integer32(Group.EngineTime),
                        userName,
                        privacy.AuthenticationProvider.CleanDigest,
                        privacy.Salt),
                    new Scope(
                        Group.EngineId,
                        OctetString.Empty,
                        new ResponsePdu(
                            Request.RequestId(),
                            status,
                            index,
                            Request.Pdu().Variables)),
                    privacy,
                    true);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }

        /// <summary>
        /// Generates too big message.
        /// </summary>
        public override void GenerateTooBig()
        {
            var userName = Request.Parameters.UserName;
            var privacy = Users.Find(userName);
            Response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId()),
                    new Integer32(Messenger.MaxMessageSize),
                    privacy.ToSecurityLevel()),
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(Group.EngineBoots),
                    new Integer32(Group.EngineTime),
                    userName,
                    privacy.AuthenticationProvider.CleanDigest,
                    privacy.Salt),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ResponsePdu(
                        Request.RequestId(),
                        ErrorCode.TooBig,
                        0,
                        Request.Pdu().Variables)),
                privacy,
                true);
            if (TooBig)
            {
                Response = null;
                
                // TODO: snmpSilentDrops++;
            }
        }

        /// <summary>
        /// Handles the membership.
        /// </summary>
        /// <returns></returns>
        public override bool HandleMembership()
        {
            var request = Request;
            if (request is MalformedMessage)
            {
                return false;
            }

            var parameters = request.Parameters;
            if (parameters.UserName == OctetString.Empty)
            {
                HandleDiscovery();
                return true;
            }

            if (parameters.EngineId != Group.EngineId)
            {
                // not from this engine.
                return false;
            }

            if (parameters.EngineBoots.ToInt32() != Group.EngineBoots)
            {
                // does not match boot count.
                return false;
            }

            // TODO: make 500 configurable
            return parameters.EngineTime.ToInt32() <= Group.EngineTime + 500;
        }

        private void HandleDiscovery()
        {
            Group.ReportCount++;
            
            // discovery message received.
            Response = new ReportMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(Request.MessageId()),
                    new Integer32(Messenger.MaxMessageSize),
                    0), // no need to encrypt for discovery.
                new SecurityParameters(
                    Group.EngineId,
                    Integer32.Zero,
                    new Integer32(Environment.TickCount),
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ReportPdu(
                        Request.RequestId(),
                        ErrorCode.NoError,
                        0,
                        new List<Variable>(1)
                            {
                                new Variable(
                                    ReportId,
                                    new Counter32(Group.ReportCount))
                            })),
                DefaultPrivacyProvider.DefaultPair);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="variables">The variables.</param>
        public override void GenerateResponse(IList<Variable> variables)
        {
            var userName = Request.Parameters.UserName;
            var privacy = Users.Find(userName);
            Response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId()),
                    new Integer32(Messenger.MaxMessageSize),
                    privacy.ToSecurityLevel()),
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(Group.EngineBoots),
                    new Integer32(Group.EngineTime),
                    userName,
                    privacy.AuthenticationProvider.CleanDigest,
                    privacy.Salt),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ResponsePdu(
                        Request.RequestId(),
                        ErrorCode.NoError,
                        0,
                        variables)),
                privacy,
                true);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }
    }
}