using System.Collections.Generic;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP application class, who is a pipeline for message processing.
    /// </summary>
    internal class SnmpApplication
    {
        private SnmpContext _context;
        private bool _finished;
        private readonly ILogger _logger;
        private readonly IMembershipProvider _provider;
        private readonly MessageHandlerFactory _factory;
        private IMessageHandler _handler;
        private const int MaxResponseSize = 1500;
        private readonly ObjectStore _store;
        private readonly SnmpApplicationFactory _owner;
        private readonly UserRegistry _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpApplication"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="store">The store.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="users">The users.</param>
        public SnmpApplication(SnmpApplicationFactory owner, ILogger logger, ObjectStore store, IMembershipProvider provider, MessageHandlerFactory factory, UserRegistry users)
        {
            _owner = owner;
            _provider = provider;
            _logger = logger;
            _store = store;
            _factory = factory;
            _users = users;
        }

        /// <summary>
        /// Inits the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Init(SnmpContext context)
        {
            _context = context;
            _finished = false;
            _handler = null;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public SnmpContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets a value indicating whether processing is finished.
        /// </summary>
        /// <value><c>true</c> if processing is finished; otherwise, <c>false</c>.</value>
        public bool ProcessingFinished
        {
            get { return _finished; }
        }

        /// <summary>
        /// Processes an incoming request.
        /// </summary>
        public void Process()
        {
            OnAuthenticateRequest();
            
            // TODO: add authorization.
            OnMapRequestHandler();
            OnRequestHandlerExecute();
            OnLogRequest();
            _owner.Reuse(this);
        }

        private void OnRequestHandlerExecute()
        {
            if (ProcessingFinished)
            {
                return;
            }

            IList<Variable> result = _handler.Handle(Context.Request, _store);

            GetResponseMessage response;
            if (Context.Request.Version != VersionCode.V3)
            {
                
                if (_handler.ErrorStatus == ErrorCode.NoError)
                {
                    // for v1 and v2 reply.
                    response = new GetResponseMessage(
                        Context.Request.RequestId,
                        Context.Request.Version,
                        Context.Request.Parameters.UserName,
                        ErrorCode.NoError,
                        0,
                        result);
                }
                else
                {
                    response = new GetResponseMessage(
                        Context.Request.RequestId,
                        Context.Request.Version,
                        Context.Request.Parameters.UserName,
                        _handler.ErrorStatus,
                        _handler.ErrorIndex,
                        Context.Request.Pdu.Variables);
                }

                if (response.ToBytes().Length > MaxResponseSize)
                {
                    response = new GetResponseMessage(
                        Context.Request.RequestId,
                        Context.Request.Version,
                        Context.Request.Parameters.UserName,
                        ErrorCode.TooBig,
                        0,
                        Context.Request.Pdu.Variables);
                }
            }
            else
            {
                // TODO: reply with v3.
                if (_handler.ErrorStatus == ErrorCode.NoError)
                {
                    // for v3
                    response = new GetResponseMessage(
                        Context.Request.Version,
                        Context.Request.RequestId,
                        Context.Request.MessageId,
                        Context.Request.Parameters.UserName,
                        ErrorCode.NoError,
                        0,
                        result,
                        Users.Find(Context.Request.Parameters.UserName));
                }
                else
                {
                    response = new GetResponseMessage(
                        Context.Request.Version,
                        Context.Request.RequestId,
                        Context.Request.MessageId,
                        Context.Request.Parameters.UserName,
                        _handler.ErrorStatus,
                        _handler.ErrorIndex,
                        Context.Request.Pdu.Variables,
                        Users.Find(Context.Request.Parameters.UserName));
                }

                if (response.ToBytes().Length > MaxResponseSize)
                {
                    response = new GetResponseMessage(
                        Context.Request.Version,
                        Context.Request.RequestId,
                        Context.Request.MessageId,
                        Context.Request.Parameters.UserName,
                        ErrorCode.TooBig,
                        0,
                        Context.Request.Pdu.Variables,
                        Users.Find(Context.Request.Parameters.UserName));
                }
            }

            Context.Response = response;
        }

        public UserRegistry Users
        {
            get { return _users; }
        }

        private void OnMapRequestHandler()
        {
            if (ProcessingFinished)
            {
                return;
            }

            _handler = _factory.GetHandler(Context.Request);
            if (_handler.GetType() == typeof(NullMessageHandler))
            {
                // TODO: handle error here.
                CompleteProcessing();
            }
        }

        private void OnAuthenticateRequest()
        {
            if (!_provider.AuthenticateRequest(Context))
            {
                // TODO: handle error here.
                // return TRAP saying authenticationFailed.
                CompleteProcessing();
            }
            
            if (Context.Response != null)
            {
                CompleteProcessing();
            }
        }

        private void OnLogRequest()
        {
            Context.SendResponse();
            _logger.Log(Context);
        }

        /// <summary>
        /// Completes the processing.
        /// </summary>
        public void CompleteProcessing()
        {
            _finished = true;
        }
    }
}