using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Agent component.
    /// </summary>
    public partial class Agent : Component
    {
        private const int DefaultPort = 161;
        
        /// <summary>
        /// Initiates an <see cref="Agent"/> instance.
        /// </summary>
        public Agent()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Stops.
        /// </summary>
        public void Stop()
        {
            trapListener.Stop();
        }
        
        /// <summary>
        /// Starts.
        /// </summary>
        public void Start()
        {
            Start(DefaultPort);
        }
        
        /// <summary>
        /// Starts on a specific port.
        /// </summary>
        /// <param name="port">Port.</param>
        public void Start(int port)
        {
            trapListener.Start(port);
        }

        /// <summary>
        /// Occurs when a GET request is received.
        /// </summary>
        public event EventHandler<GetRequestReceivedEventArgs> GetRequestReceived;
        
        /// <summary>
        /// Occurs when a SET request is received.
        /// </summary>
        public event EventHandler<SetRequestReceivedEventArgs> SetRequestReceived;
        
        /// <summary>
        /// Occurs when a GET NEXT request is received.
        /// </summary>
        public event EventHandler<GetNextRequestReceivedEventArgs> GetNextRequestReceived;
        
        /// <summary>
        /// Occurs when a GET BULK request is received.
        /// </summary>
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
        [CLSCompliant(false)]
        public static void SendTrapV2(VersionCode version, IPEndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
        {
            TrapV2Message message = new TrapV2Message(VersionCode.V2, community, enterprise, timestamp, variables);
            message.Send(receiver);
        }

        /// <summary>
        /// Sends INFORM message.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="timeout">Timeout.</param>
        [CLSCompliant(false)]
        public static void SendInform(VersionCode version, IPEndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, int timeout)
        {
            InformRequestMessage message = new InformRequestMessage(version, community, enterprise, timestamp, variables);
            GetResponseMessage response = message.GetResponse(timeout, receiver);
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    receiver.Address,
                    response.ErrorStatus,
                    response.ErrorIndex,
                    response.Variables[response.ErrorIndex - 1].Id);
            }
        }

        private void TrapListener_GetRequestReceived(object sender, GetRequestReceivedEventArgs e)
        {
            if (GetRequestReceived != null)
            {
                GetRequestReceived(this, e);
            }
        }

        #region Component Designer generated code

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
            this.trapListener.Port = 161;            
            this.trapListener.GetRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetRequestReceivedEventArgs>(this.TrapListener_GetRequestReceived);
            this.trapListener.GetBulkRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetBulkRequestReceivedEventArgs>(this.TrapListener_GetBulkRequestReceived);
            this.trapListener.SetRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.SetRequestReceivedEventArgs>(this.TrapListener_SetRequestReceived);
            this.trapListener.GetNextRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetNextRequestReceivedEventArgs>(this.TrapListener_GetNextRequestReceived);

        }

        #endregion

        private TrapListener trapListener;

        private void TrapListener_GetBulkRequestReceived(object sender, GetBulkRequestReceivedEventArgs e)
        {
            if (GetBulkRequestReceived != null)
            {
                GetBulkRequestReceived(this, e);
            }
        }

        private void TrapListener_GetNextRequestReceived(object sender, GetNextRequestReceivedEventArgs e)
        {
            if (GetNextRequestReceived != null)
            {
                GetNextRequestReceived(this, e);
            }
        }

        private void TrapListener_SetRequestReceived(object sender, SetRequestReceivedEventArgs e)
        {
            if (SetRequestReceived != null)
            {
                SetRequestReceived(this, e);
            }
        }
    }
}
