namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP application class, who is a pipeline for message processing.
    /// </summary>
    internal class SnmpApplication
    {
        private readonly ILogger _logger;
        private readonly IMembershipProvider _provider;
        private readonly MessageHandlerFactory _factory;
        private IMessageHandler _handler;
        private readonly ObjectStore _store;
        private readonly SnmpApplicationFactory _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpApplication"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="store">The store.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="factory">The factory.</param>
        public SnmpApplication(SnmpApplicationFactory owner, ILogger logger, ObjectStore store, IMembershipProvider provider, MessageHandlerFactory factory)
        {
            _owner = owner;
            _provider = provider;
            _logger = logger;
            _store = store;
            _factory = factory;
        }

        /// <summary>
        /// Inits the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Init(SnmpContext context)
        {
            Context = context;
            ProcessingFinished = false;
            _handler = null;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public SnmpContext Context { get; private set; }

        /// <summary>
        /// Gets a value indicating whether processing is finished.
        /// </summary>
        /// <value><c>true</c> if processing is finished; otherwise, <c>false</c>.</value>
        public bool ProcessingFinished { get; private set; }

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

            Context.GenerateResponse(_handler.Handle(Context.Request, _store));
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
                Context.HandleAuthenticationFailure();
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
            ProcessingFinished = true;
        }
    }
}