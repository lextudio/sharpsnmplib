using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP context.
    /// </summary>
    internal class SnmpContext
    {
        private readonly ISnmpMessage _request;
        private readonly DateTime _createdTime;
        private readonly IPEndPoint _sender;
        private ISnmpMessage _response;
        private readonly Listener _listener;
        private AgentObjects _objects;
        private const int MaxResponseSize = 1500;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="objects">The agent core objects.</param>
        public SnmpContext(ISnmpMessage request, ISnmpMessage response, IPEndPoint sender, Listener listener, AgentObjects objects)
        {
            _request = request;
            _listener = listener;
            _sender = sender;
            _response = response;
            _createdTime = DateTime.Now;
            _objects = objects;
        }

        private UserRegistry Users
        {
            get { return _listener.Users; }
        }

        private AgentObjects Objects
        {
            get { return _objects; }
        }

        /// <summary>
        /// Gets the created time.
        /// </summary>
        /// <value>The created time.</value>
        public DateTime CreatedTime
        {
            get { return _createdTime; }
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public ISnmpMessage Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Gets the listener.
        /// </summary>
        /// <value>The listener.</value>
        public Listener Listener
        {
            get { return _listener; }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The response.</value>
        public ISnmpMessage Response
        {
            get { return _response; }
            set { _response = value; }
        }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }
        
        /// <summary>
        /// Sends out response message.
        /// </summary>
        public void SendResponse()
        {
            if (_response != null)
            {
                if (_response.Version == VersionCode.V3)
                {
                    ProviderPair providers = _listener.Users.Find(_request.Parameters.UserName);
                    providers = providers ?? ProviderPair.Default;
                    Helper.Authenticate(_response, providers);
                }

                Listener.SendResponse(_response, Sender);
            }
        }

        internal void HandleAuthenticationFailure()
        {
            GetResponseMessage response = null;
            if (Request.Version == VersionCode.V3)
            {
                ISnmpMessage message = Request;
                ProviderPair providers = ProviderPair.Default;
                response = new GetResponseMessage(
                    message.Version,
                    new Header(
                        new Integer32(message.MessageId),
                        new Integer32(0xFFE3),
                        new OctetString(new[] { (byte)Levels.Reportable }),
                        new Integer32(3)),
                    new SecurityParameters(
                        Objects.EngineId,
                        new Integer32(Objects.EngineBoots),
                        new Integer32(Objects.EngineTime),
                        message.Parameters.UserName,
                        providers.Authentication.CleanDigest,
                        providers.Privacy.Salt),
                    new Scope(
                        Objects.EngineId,
                        OctetString.Empty,
                        new GetResponsePdu(
                            message.RequestId,
                            ErrorCode.AuthorizationError,
                            0,
                            new List<Variable>())),
                    providers);
            }

            _response = response;
        }

        internal void GenerateResponse(ResponseData data)
        {
            ISnmpMessage message = Request;
            IList<Variable> result = data.Variables;

            GetResponseMessage response;
            if (Request.Version != VersionCode.V3)
            {

                if (data.ErrorStatus == ErrorCode.NoError)
                {
                    // for v1 and v2 reply.
                    response = new GetResponseMessage(
                        message.RequestId,
                        message.Version,
                        message.Parameters.UserName,
                        ErrorCode.NoError,
                        0,
                        result);
                }
                else
                {
                    response = new GetResponseMessage(
                        message.RequestId,
                        message.Version,
                        message.Parameters.UserName,
                        data.ErrorStatus,
                        data.ErrorIndex,
                        message.Pdu.Variables);
                }

                if (response.ToBytes().Length > MaxResponseSize)
                {
                    response = new GetResponseMessage(
                        message.RequestId,
                        message.Version,
                        message.Parameters.UserName,
                        ErrorCode.TooBig,
                        0,
                        // TODO: check RFC to see what should be returned here.
                        new List<Variable>());
                }
            }
            else
            {
                ProviderPair providers = Users.Find(Request.Parameters.UserName);
                // TODO: reply with v3.
                if (data.ErrorStatus == ErrorCode.NoError)
                {
                    // for v3
                    response = new GetResponseMessage(
                        message.Version,
                        new Header(
                            new Integer32(message.MessageId),
                            new Integer32(0xFFE3),
                            new OctetString(new[] { (byte)Levels.Reportable }),
                            new Integer32(3)),
                        new SecurityParameters(
                            Objects.EngineId,
                            new Integer32(Objects.EngineBoots),
                            new Integer32(Objects.EngineTime),
                            message.Parameters.UserName,
                            providers.Authentication.CleanDigest,
                            providers.Privacy.Salt),
                        new Scope(
                            Objects.EngineId,
                            OctetString.Empty,
                            new GetResponsePdu(
                                message.RequestId,
                                ErrorCode.NoError,
                                0,
                                result)),
                        providers);
                }
                else
                {
                    response = new GetResponseMessage(
                        message.Version,
                        new Header(
                            new Integer32(message.MessageId),
                            new Integer32(0xFFE3),
                            new OctetString(new[] { (byte)Levels.Reportable }),
                            new Integer32(3)),
                        new SecurityParameters(
                            Objects.EngineId,
                            new Integer32(Objects.EngineBoots),
                            new Integer32(Objects.EngineTime),
                            message.Parameters.UserName,
                            providers.Authentication.CleanDigest,
                            providers.Privacy.Salt),
                        new Scope(
                            Objects.EngineId,
                            OctetString.Empty,
                            new GetResponsePdu(
                                message.RequestId,
                                data.ErrorStatus,
                                data.ErrorIndex,
                                message.Pdu.Variables)),
                        providers);
                }

                if (response.ToBytes().Length > MaxResponseSize)
                {
                    response = new GetResponseMessage(
                        message.Version,
                        new Header(
                            new Integer32(message.MessageId),
                            new Integer32(0xFFE3),
                            new OctetString(new[] { (byte)Levels.Reportable }),
                            new Integer32(3)),
                        new SecurityParameters(
                            Objects.EngineId,
                            new Integer32(Objects.EngineBoots),
                            new Integer32(Objects.EngineTime),
                            message.Parameters.UserName,
                            providers.Authentication.CleanDigest,
                            providers.Privacy.Salt),
                        new Scope(
                            Objects.EngineId,
                            OctetString.Empty,
                            new GetResponsePdu(
                                message.RequestId,
                                ErrorCode.TooBig,
                                0,
                                new List<Variable>())),
                        providers);
                }
            }

            _response = response;        
        }
    }
}