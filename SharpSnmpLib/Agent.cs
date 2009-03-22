using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
#pragma warning disable 612,618
namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Agent component.
    /// </summary>
    public class Agent : Component
    {       
        /// <summary>
        /// Initiates an <see cref="Agent"/> instance.
        /// </summary>
        public Agent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Agent(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Message monitor.
        /// </summary>
        [Obsolete("Use a stand alone Listener component")]
        public TrapListener Monitor
        {
            get { return trapListener; }
        }

        /// <summary>
        /// Occurs when an exception is raised.
        /// </summary>
        [Obsolete("Use a stand alone Listener component")]
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;
        
        /// <summary>
        /// Occurs when a GET request is received.
        /// </summary>
        [Obsolete("Use a stand alone Listener component")]
        public event EventHandler<GetRequestReceivedEventArgs> GetRequestReceived;
        
        /// <summary>
        /// Occurs when a SET request is received.
        /// </summary>
        [Obsolete("Use a stand alone Listener component")]
        public event EventHandler<SetRequestReceivedEventArgs> SetRequestReceived;
        
        /// <summary>
        /// Occurs when a GET NEXT request is received.
        /// </summary>
        [Obsolete("Use a stand alone Listener component")]
        public event EventHandler<GetNextRequestReceivedEventArgs> GetNextRequestReceived;
        
        /// <summary>
        /// Occurs when a GET BULK request is received.
        /// </summary>
        [Obsolete("Use a stand alone Listener component")]
        public event EventHandler<GetBulkRequestReceivedEventArgs> GetBulkRequestReceived;

        /// <summary>
        /// Sends a TRAP v1 message.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        /// <param name="agent">Agent.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="generic">Generic code.</param>
        /// <param name="specific">Specific code.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        [CLSCompliant(false)]
        public static void SendTrapV1(IPEndPoint receiver, IPAddress agent, OctetString community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint timestamp, IList<Variable> variables)
        {
            TrapV1Message message = new TrapV1Message(VersionCode.V1, agent, community, enterprise, generic, specific, timestamp, variables);
            message.Send(receiver);
        }

        /// <summary>
        /// Sends TRAP v2 message.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="requestId">Request ID.</param>
        [CLSCompliant(false)]
        public static void SendTrapV2(int requestId, VersionCode version, IPEndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
        {
            if (version == VersionCode.V1)
            {
                throw new ArgumentException("SNMP v1 is not support", "version");
            }

            TrapV2Message message = new TrapV2Message(requestId, version, community, enterprise, timestamp, variables);
            message.Send(receiver);
        }

        /// <summary>
        /// Sends INFORM message.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="timeout">Timeout.</param>
        [CLSCompliant(false)]
        public static void SendInform(int requestId, VersionCode version, IPEndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, int timeout)
        {
            InformRequestMessage message = new InformRequestMessage(requestId, version, community, enterprise, timestamp, variables);
            GetResponseMessage response = message.GetResponse(timeout, receiver);
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    receiver.Address,
                    response);
            }
        }

        private void TrapListener_GetRequestReceived(object sender, GetRequestReceivedEventArgs e)
        {
            EventHandler<GetRequestReceivedEventArgs> handler = GetRequestReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #region Component Designer generated code
// ReSharper disable all
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            this.trapListener = new Lextm.SharpSnmpLib.TrapListener();

            // 
            // trapListener
            //           
            this.trapListener.GetRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetRequestReceivedEventArgs>(this.TrapListener_GetRequestReceived);
            this.trapListener.GetBulkRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetBulkRequestReceivedEventArgs>(this.TrapListener_GetBulkRequestReceived);
            this.trapListener.SetRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.SetRequestReceivedEventArgs>(this.TrapListener_SetRequestReceived);
            this.trapListener.GetNextRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetNextRequestReceivedEventArgs>(this.TrapListener_GetNextRequestReceived);
            this.trapListener.ExceptionRaised += new EventHandler<ExceptionRaisedEventArgs>(TrapListener_ExceptionRaised);
        }
// ReSharper restore all
        private void TrapListener_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            EventHandler<ExceptionRaisedEventArgs> handler = ExceptionRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        private TrapListener trapListener;

        private void TrapListener_GetBulkRequestReceived(object sender, GetBulkRequestReceivedEventArgs e)
        {
            EventHandler<GetBulkRequestReceivedEventArgs> handler = GetBulkRequestReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void TrapListener_GetNextRequestReceived(object sender, GetNextRequestReceivedEventArgs e)
        {
            EventHandler<GetNextRequestReceivedEventArgs> handler = GetNextRequestReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void TrapListener_SetRequestReceived(object sender, SetRequestReceivedEventArgs e)
        {
            EventHandler<SetRequestReceivedEventArgs> handler = SetRequestReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
#pragma warning restore 612,618