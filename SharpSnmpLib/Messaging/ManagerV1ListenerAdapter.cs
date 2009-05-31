/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 11:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Adapter for SNMP v1 manager.
    /// </summary>
    public class ManagerV1ListenerAdapter : IListenerAdapter
    {
        /// <summary>
        /// Occurs when a <see cref="TrapV1Message" /> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV1Message>> TrapV1Received;
        
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
                case SnmpType.TrapV1Pdu:
                    {
                        EventHandler<MessageReceivedEventArgs<TrapV1Message>> handler = TrapV1Received;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<TrapV1Message>(sender, (TrapV1Message)message));
                        }

                        break;
                    }

                default:
                    break;
            }
        }
    }
}
