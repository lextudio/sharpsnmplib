/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 11:53 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Description of ManagerV3ListenerAdapter.
    /// </summary>
    public class ManagerV3ListenerAdapter : IListenerAdapter, IDisposable
    {
        private readonly Listener _listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerV3ListenerAdapter"/> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public ManagerV3ListenerAdapter(Listener listener)
        {
            _listener = listener;
        }

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
            
            if (message.Version != VersionCode.V3)
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
                        _listener.SendResponse(inform.GenerateResponse(), sender);

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
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _listener.Dispose();
        }
    }
}
