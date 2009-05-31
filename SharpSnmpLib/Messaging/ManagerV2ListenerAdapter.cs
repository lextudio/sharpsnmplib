/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 11:51 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Adapter for SNMP v2c manager.
    /// </summary>
    public class ManagerV2ListenerAdapter : IListenerAdapter
    {
        /// <summary>
        /// Occurs when a <see cref="TrapV2Message"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV2Message>> TrapV2Received;

        /// <summary>
        /// Occurs when a <see cref="InformRequestMessage"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<InformRequestMessage>> InformRequestReceived;
        
        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="sender">Sender.</param>
        public void Process(ISnmpMessage message, System.Net.IPEndPoint sender)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            
            if (message.Version != VersionCode.V2)
            {
                return;
            }
            
            switch (message.Pdu.TypeCode)
            {
                case SnmpType.TrapV2Pdu:
                    {
                        EventHandler<MessageReceivedEventArgs<TrapV2Message>> handler = TrapV2Received;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<TrapV2Message>(sender, (TrapV2Message)message));
                        }

                        break;
                    }

                case SnmpType.InformRequestPdu:
                    {
                        InformRequestMessage inform = (InformRequestMessage)message;
                        inform.SendResponse(sender);

                        EventHandler<MessageReceivedEventArgs<InformRequestMessage>> handler = InformRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<InformRequestMessage>(sender, inform));
                        }

                        break;
                    }

                default:
                    break;
            }
        }
    }
}
