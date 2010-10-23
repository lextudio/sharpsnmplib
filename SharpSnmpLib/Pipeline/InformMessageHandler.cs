using System;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Message handler for INFORM.
    /// </summary>    
// ReSharper disable ClassNeverInstantiated.Global
    public class InformMessageHandler : IMessageHandler
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
            
            InvokeMessageReceived(new MessageReceivedEventArgs<InformRequestMessage>(context.Sender, (InformRequestMessage)context.Request, context.Binding));
            context.CopyRequest(ErrorCode.NoError, 0);
        }

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<InformRequestMessage>> MessageReceived;

        /// <summary>
        /// Invokes the message received event handler.
        /// </summary>
        /// <param name="e">The <see cref="Lextm.SharpSnmpLib.Messaging.MessageReceivedEventArgs"/> instance containing the event data.</param>
        public void InvokeMessageReceived(MessageReceivedEventArgs<InformRequestMessage> e)
        {
            EventHandler<MessageReceivedEventArgs<InformRequestMessage>> handler = MessageReceived;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
    }
}
