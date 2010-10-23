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
        /// Authenticates the message.
        /// </summary>
        protected override void AuthenticateResponse()
        {
            SnmpMessageExtension.Authenticate(Response);
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
                DefaultPrivacyProvider.DefaultPair);
        }

        internal override void CopyRequest(ErrorCode status, int index)
        {
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);
            var response = new ResponseMessage(
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
                    privacy);
            if (response.ToBytes().Length > MaxResponseSize)
            {
                response = new ResponseMessage(
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
                    privacy);
            }

            Response = response;
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

            OctetString embedded = Request.Parameters.AuthenticationParameters;
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);
            Request.Parameters.AuthenticationParameters = privacy.AuthenticationProvider.CleanDigest;
            OctetString calculated = privacy.AuthenticationProvider.ComputeHash(Request);
            
            // other checking were performed in MessageFactory when decrypting message body.
            return embedded == calculated;
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
                                    new Counter32(Group.ReportCount)
                                    )
                            })),
                DefaultPrivacyProvider.DefaultPair);
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="variables">The variables.</param>
        internal override void GenerateResponse(IList<Variable> variables)
        {
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);

            // for v3
            ResponseMessage response = new ResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(Messenger.MaxMessageSize),
                    new OctetString(new[] {(byte) Levels.Reportable}),
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
                privacy);

            if (response.ToBytes().Length > MaxResponseSize)
            {
                response = new ResponseMessage(
                    Request.Version,
                    new Header(
                        new Integer32(Request.MessageId),
                        new Integer32(Messenger.MaxMessageSize),
                        new OctetString(new[] {(byte) Levels.Reportable}),
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
                    privacy);
            }

            Response = response;
        }
    }
}