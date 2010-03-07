/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 12:09 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Default listener adapter implementation.
    /// </summary>
    [Obsolete("Manager developers should use Manager* adapters. Agent developers should check out snmpd as reference design.")]
    public class DefaultListenerAdapter : IListenerAdapter
    {
        private readonly Listener _listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultListenerAdapter"/> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public DefaultListenerAdapter(Listener listener)
        {
            _listener = listener;
        }

        /// <summary>
        /// Occurs when a <see cref="TrapV1Message" /> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV1Message>> TrapV1Received;

        /// <summary>
        /// Occurs when a <see cref="TrapV2Message"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV2Message>> TrapV2Received;

        /// <summary>
        /// Occurs when a <see cref="InformRequestMessage"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<InformRequestMessage>> InformRequestReceived;

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
            
            switch (message.Pdu.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    {
                        EventHandler<MessageReceivedEventArgs<TrapV1Message>> handler = TrapV1Received;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<TrapV1Message>(sender, (TrapV1Message)message));
                        }

                        break;
                    }

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
                        _listener.SendResponse(inform.GenerateResponse(), sender);

                        EventHandler<MessageReceivedEventArgs<InformRequestMessage>> handler = InformRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<InformRequestMessage>(sender, inform));
                        }

                        break;
                    }

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
