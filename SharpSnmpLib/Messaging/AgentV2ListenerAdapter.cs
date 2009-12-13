/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 11:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Adapter for SNMP v2c agent.
    /// </summary>
    [Obsolete("Use SnmpDemon instead.")]
    public class AgentV2ListenerAdapter : IListenerAdapter
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
        /// Occurs when a GET BULK request is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<GetBulkRequestMessage>> GetBulkRequestReceived;
        
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

                case SnmpType.GetBulkRequestPdu:
                    {
                        EventHandler<MessageReceivedEventArgs<GetBulkRequestMessage>> handler = GetBulkRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<GetBulkRequestMessage>(sender, (GetBulkRequestMessage)message));
                        }

                        break;
                    }

                default:
                    break;
            }
        }
    }
}
