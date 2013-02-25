// Secure SNMP context class.
// Copyright (C) 2009-2010 Lex Li
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
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System.Globalization;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Secure SNMP context. It is specific to v3.
    /// </summary>
    internal sealed class SecureSnmpContext : SnmpContextBase
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

        private void HandleFailure(Variable failure)
        {
            var defaultPair = DefaultPrivacyProvider.DefaultPair;
            Response = new ReportMessage(
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
                    new ReportPdu(
                        Request.RequestId(),
                        ErrorCode.NoError,
                        0,
                        new List<Variable>(1) { failure })),
                defaultPair,
                null);
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
                    true,
                    null);
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
                true,
                null);
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
            var parameters = request.Parameters;
            var typeCode = Request.TypeCode();
            if (parameters.EngineId != Group.EngineId && typeCode != SnmpType.Unknown)
            {
                HandleDiscovery();
                return true;
            }

            var user = Users.Find(parameters.UserName);
            if (user == null) 
            {
                HandleFailure(Group.UnknownSecurityName);
                return false;
            }

            if (typeCode == SnmpType.Unknown)
            {
                HandleFailure(Group.DecryptionError);
                return false;
            }

            if (parameters.IsInvalid)
            {
                HandleFailure(Group.AuthenticationFailure);
                return false;
            }

            if ((user.ToSecurityLevel() | Levels.Reportable) != request.Header.SecurityLevel)
            {
                HandleFailure(Group.UnsupportedSecurityLevel);
                return false;
            }

            // TODO: improve here, so if request's EngineBoots = agent's EngineBoots - 1 we can calculate if it is in time.
            if (parameters.EngineBoots.ToInt32() != Group.EngineBoots)
            {
                HandleFailure(Group.NotInTimeWindow);
                return false;
            }

            var inTime = EngineGroup.IsInTime(Group.EngineTime, parameters.EngineTime.ToInt32());
            if (!inTime)
            {
                HandleFailure(Group.NotInTimeWindow);
                return false;
            }

            return true;
        }

        private static bool? _timeIncluded;

        private static bool TimeIncluded
        {
            get
            {
                if (_timeIncluded == null)
                {
#if MA || CF || MT
                    _timeIncluded = true;
#else
                    object setting = ConfigurationManager.AppSettings["TimeIncluded"];
                    _timeIncluded = setting != null && Convert.ToBoolean(setting.ToString(), CultureInfo.InvariantCulture);
#endif
                }

                return _timeIncluded.Value;
            }
        }

        private void HandleDiscovery()
        {         
            // discovery message received.
            // TODO: pick up time information from Group instead of using raw data.
            Response = new ReportMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(Request.MessageId()),
                    new Integer32(Messenger.MaxMessageSize),
                    0), // no need to encrypt for discovery.
                new SecurityParameters(
                    Group.EngineId,
                    TimeIncluded ? Integer32.Zero : Integer32.Zero,
                    TimeIncluded ? new Integer32(Environment.TickCount) : Integer32.Zero,
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
                        new List<Variable>(1) { Group.UnknownEngineID })),
                DefaultPrivacyProvider.DefaultPair,
                null);
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
                true,
                null);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }
    }
}