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
    internal sealed class SecureSnmpContext : SnmpContext
    {
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
        internal override void HandleAuthenticationFailure()
        {
            Response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(Messenger.MaxMessageSize),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(Group.EngineBoots),
                    new Integer32(Group.EngineTime),
                    Request.Parameters.UserName,
                    DefaultPrivacyProvider.DefaultPair.AuthenticationProvider.CleanDigest,
                    DefaultPrivacyProvider.DefaultPair.Salt),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ResponsePdu(
                        Request.RequestId,
                        ErrorCode.AuthorizationError,
                        0,
                        Request.Pdu.Variables)),
                DefaultPrivacyProvider.DefaultPair,
                true);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }

        internal override void CopyRequest(ErrorCode status, int index)
        {
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);
            Response = new ResponseMessage(
                    Request.Version,
                    new Header(
                        new Integer32(Request.MessageId),
                        new Integer32(Messenger.MaxMessageSize),
                        new OctetString(new[] { (byte)Levels.Reportable }),
                        new Integer32(3)),
                    new SecurityParameters(
                        Group.EngineId,
                        new Integer32(Group.EngineBoots),
                        new Integer32(Group.EngineTime),
                        Request.Parameters.UserName,
                        privacy.AuthenticationProvider.CleanDigest,
                        privacy.Salt),
                    new Scope(
                        Group.EngineId,
                        OctetString.Empty,
                        new ResponsePdu(
                            Request.RequestId,
                            status,
                            index,
                            Request.Pdu.Variables)),
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
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);
            Response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(Messenger.MaxMessageSize),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(Group.EngineBoots),
                    new Integer32(Group.EngineTime),
                    Request.Parameters.UserName,
                    privacy.AuthenticationProvider.CleanDigest,
                    privacy.Salt),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ResponsePdu(
                        Request.RequestId,
                        ErrorCode.TooBig,
                        0,
                        Request.Pdu.Variables)),
                privacy,
                true);
        }

        /// <summary>
        /// Handles the membership.
        /// </summary>
        /// <returns></returns>
        public override bool HandleMembership()
        {
            if (Request is MalformedMessage)
            {
                return false;
            }

            if (Request.Parameters.UserName == OctetString.Empty)
            {
                HandleDiscovery();
                return true;
            }

            if (Request.Parameters.EngineId != Group.EngineId)
            {
                // not from this engine.
                return false;
            }

            if (Request.Parameters.EngineBoots.ToInt32() != Group.EngineBoots)
            {
                // does not match boot count.
                return false;
            }

            if (Request.Parameters.EngineTime.ToInt32() > Group.EngineTime + 500)
            {
                // timeout.
                return false;
            }

            return true;
        }

        private void HandleDiscovery()
        {
            Group.ReportCount++;
            
            // discovery message received.
            Response = new ReportMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(Messenger.MaxMessageSize),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(0),
                    new Integer32(Environment.TickCount),
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ReportPdu(
                        Request.RequestId,
                        ErrorCode.NoError,
                        0,
                        new List<Variable>(1)
                            {
                                new Variable(
                                    new ObjectIdentifier("1.3.6.1.6.3.15.1.1.4.0"),
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
        internal override void GenerateResponse(IList<Variable> variables)
        {
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);

            // for v3
            Response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(Messenger.MaxMessageSize),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    Group.EngineId,
                    new Integer32(Group.EngineBoots),
                    new Integer32(Group.EngineTime),
                    Request.Parameters.UserName,
                    privacy.AuthenticationProvider.CleanDigest,
                    privacy.Salt),
                new Scope(
                    Group.EngineId,
                    OctetString.Empty,
                    new ResponsePdu(
                        Request.RequestId,
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