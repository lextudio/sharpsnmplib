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
        /// <param name="objects">The agent core objects.</param>
        /// <param name="binding">The binding.</param>
        public SecureSnmpContext(ISnmpMessage request, IPEndPoint sender, UserRegistry users, EngineObjects objects, IListenerBinding binding)
            : base(request, sender, users, objects, binding)
        {
        }

        /// <summary>
        /// Authenticates the message.
        /// </summary>
        protected override void AuthenticateMessage()
        {
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName) ?? DefaultPrivacyProvider.Default;
            Helper.Authenticate(Response, privacy);
        }

        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        internal override void HandleAuthenticationFailure()
        {
            Response = new GetResponseMessage(
                Request.Version,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(0xFFE3),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    Objects.EngineId,
                    new Integer32(Objects.EngineBoots),
                    new Integer32(Objects.EngineTime),
                    Request.Parameters.UserName,
                    DefaultPrivacyProvider.Default.AuthenticationProvider.CleanDigest,
                    DefaultPrivacyProvider.Default.Salt),
                new Scope(
                    Objects.EngineId,
                    OctetString.Empty,
                    new GetResponsePdu(
                        Request.RequestId,
                        ErrorCode.AuthorizationError,
                        0,
                        Request.Pdu.Variables)),
                DefaultPrivacyProvider.Default);
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

            if (Request.Parameters.EngineId != Objects.EngineId)
            {
                // not from this engine.
                return false;
            }

            if (Request.Parameters.EngineBoots.ToInt32() != Objects.EngineBoots)
            {
                // does not match boot count.
                return false;
            }

            if (Request.Parameters.EngineTime.ToInt32() > Objects.EngineTime + 500)
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
            Objects.ReportCount++;
            
            // discovery message received.
            Response = new ReportMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(Request.MessageId),
                    new Integer32(0xFFE3),
                    new OctetString(new[] { (byte)Levels.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    Objects.EngineId,
                    new Integer32(0),
                    new Integer32(Environment.TickCount),
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    Objects.EngineId,
                    OctetString.Empty,
                    new ReportPdu(
                        Request.RequestId,
                        ErrorCode.NoError,
                        0,
                        new List<Variable>(1)
                            {
                                new Variable(
                                    new ObjectIdentifier("1.3.6.1.6.3.15.1.1.4.0"),
                                    new Counter32(Objects.ReportCount)
                                    )
                            })),
                DefaultPrivacyProvider.Default);
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="data">The data.</param>
        internal override void GenerateResponse(ResponseData data)
        {
            if (!data.HasResponse)
            {
                return;
            }

            GetResponseMessage response;
            IPrivacyProvider privacy = Users.Find(Request.Parameters.UserName);
            if (data.ErrorStatus == ErrorCode.NoError)
            {
                // for v3
                response = new GetResponseMessage(
                    Request.Version,
                    new Header(
                        new Integer32(Request.MessageId),
                        new Integer32(0xFFE3),
                        new OctetString(new[] {(byte) Levels.Reportable}),
                        new Integer32(3)),
                    new SecurityParameters(
                        Objects.EngineId,
                        new Integer32(Objects.EngineBoots),
                        new Integer32(Objects.EngineTime),
                        Request.Parameters.UserName,
                        privacy.AuthenticationProvider.CleanDigest,
                        privacy.Salt),
                    new Scope(
                        Objects.EngineId,
                        OctetString.Empty,
                        new GetResponsePdu(
                            Request.RequestId,
                            ErrorCode.NoError,
                            0,
                            data.Variables)),
                    privacy);
            }
            else
            {
                response = new GetResponseMessage(
                    Request.Version,
                    new Header(
                        new Integer32(Request.MessageId),
                        new Integer32(0xFFE3),
                        new OctetString(new[] {(byte) Levels.Reportable}),
                        new Integer32(3)),
                    new SecurityParameters(
                        Objects.EngineId,
                        new Integer32(Objects.EngineBoots),
                        new Integer32(Objects.EngineTime),
                        Request.Parameters.UserName,
                        privacy.AuthenticationProvider.CleanDigest,
                        privacy.Salt),
                    new Scope(
                        Objects.EngineId,
                        OctetString.Empty,
                        new GetResponsePdu(
                            Request.RequestId,
                            data.ErrorStatus,
                            data.ErrorIndex,
                            Request.Pdu.Variables)),
                    privacy);
            }

            if (response.ToBytes().Length > MaxResponseSize)
            {
                response = new GetResponseMessage(
                    Request.Version,
                    new Header(
                        new Integer32(Request.MessageId),
                        new Integer32(0xFFE3),
                        new OctetString(new[] {(byte) Levels.Reportable}),
                        new Integer32(3)),
                    new SecurityParameters(
                        Objects.EngineId,
                        new Integer32(Objects.EngineBoots),
                        new Integer32(Objects.EngineTime),
                        Request.Parameters.UserName,
                        privacy.AuthenticationProvider.CleanDigest,
                        privacy.Salt),
                    new Scope(
                        Objects.EngineId,
                        OctetString.Empty,
                        new GetResponsePdu(
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