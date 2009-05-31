/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 11:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Adapter for SNMP v1 agent.
    /// </summary>
    public class AgentV1ListenerAdapter : IListenerAdapter
    {
        /// <summary>
        /// Occurs when a <see cref="GetRequestMessage"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<GetRequestMessage>> GetRequestReceived;

        /// <summary>
        /// Occurs when a SET request is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<SetRequestMessage>> SetRequestReceived;

        /// <summary>
        /// Occurs when a GET NEXT request is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<GetNextRequestMessage>> GetNextRequestReceived;

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
            
            if (message.Version != VersionCode.V1)
            {
                return;
            }
            
            switch (message.Pdu.TypeCode)
            {
                case SnmpType.GetRequestPdu:
                    {
                        EventHandler<MessageReceivedEventArgs<GetRequestMessage>> handler = GetRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<GetRequestMessage>(sender, (GetRequestMessage)message));
                        }

                        break;
                    }

                case SnmpType.SetRequestPdu:
                    {
                        EventHandler<MessageReceivedEventArgs<SetRequestMessage>> handler = SetRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<SetRequestMessage>(sender, (SetRequestMessage)message));
                        }

                        break;
                    }

                case SnmpType.GetNextRequestPdu:
                    {
                        EventHandler<MessageReceivedEventArgs<GetNextRequestMessage>> handler = GetNextRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<GetNextRequestMessage>(sender, (GetNextRequestMessage)message));
                        }

                        break;
                    }

                default:
                    break;
            }
        }
    }
}
