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
        /// <summary>
        /// Initiates an <see cref="Agent"/> instance.
        /// </summary>
        public Agent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when a GET request is received.
        /// </summary>
        public event EventHandler<GetRequestReceivedEventArgs> GetRequestReceived;

        /// <summary>
        /// Sends a TRAP v1 message.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="generic">Generic code.</param>
        /// <param name="specific">Specific code.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        [CLSCompliant(false)]
        public static void SendTrapV1(IPEndPoint receiver, string community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint timestamp, IList<Variable> variables)
        {
            TrapV1Message message = new TrapV1Message(VersionCode.V1, receiver.Address, community, enterprise, generic, specific, timestamp, variables);
            message.Send(receiver.Address, receiver.Port);
        }

        /// <summary>
        /// Sends TRAP v2 message.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        [CLSCompliant(false)]
        public static void SendTrapV2(IPEndPoint receiver, string community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
        {
            TrapV2Message message = new TrapV2Message(VersionCode.V2, community, enterprise, timestamp, variables);
            message.Send(receiver.Address, receiver.Port);
        }

        /// <summary>
        /// Sends INFORM message.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise OID.</param>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="variables">Variable bindings.</param>
        /// <param name="timeout">Timeout.</param>
        [CLSCompliant(false)]
        public static void SendInform(IPEndPoint receiver, string community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables, int timeout)
        {
            InformRequestMessage message = new InformRequestMessage(VersionCode.V2, community, enterprise, timestamp, variables);
            message.Send(receiver.Address, timeout, receiver.Port);
        }

        // TODO: send response.
        // TODO: add other request handler.
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
            this.trapListener.Port = 162;
            this.trapListener.GetRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.GetRequestReceivedEventArgs>(this.TrapListener_GetRequestReceived);

        }

        #endregion

        private TrapListener trapListener;
    }
}
