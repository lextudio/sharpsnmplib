using System;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Message handler for TRAP v1.
    /// </summary>    
// ReSharper disable ClassNeverInstantiated.Global
    public class TrapV1MessageHandler : IMessageHandler
// ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public void Handle(SnmpContext context, ObjectStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            InvokeMessageReceived(new MessageReceivedEventArgs<TrapV1Message>(context.Sender, (TrapV1Message)context.Request, context.Binding));
        }

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV1Message>> MessageReceived;

        /// <summary>
        /// Invokes the message received event handler.
        /// </summary>
        /// <param name="e">The <see cref="Lextm.SharpSnmpLib.Messaging.MessageReceivedEventArgs"/> instance containing the event data.</param>
        public void InvokeMessageReceived(MessageReceivedEventArgs<TrapV1Message> e)
        {
            EventHandler<MessageReceivedEventArgs<TrapV1Message>> handler = MessageReceived;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }
    }
}
