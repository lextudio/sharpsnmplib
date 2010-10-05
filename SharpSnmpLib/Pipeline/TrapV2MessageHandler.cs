using System;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Message handler for TRAP v2.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class TrapV2MessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public ResponseData Handle(SnmpContext context, ObjectStore store)
        {
            InvokeMessageReceived(new MessageReceivedEventArgs<TrapV2Message>(context.Sender, (TrapV2Message)context.Request, context.Binding));
            return new ResponseData();
        }

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV2Message>> MessageReceived;

        /// <summary>
        /// Invokes the message received event handler.
        /// </summary>
        /// <param name="e">The <see cref="Lextm.SharpSnmpLib.Messaging.MessageReceivedEventArgs&lt;Lextm.SharpSnmpLib.Messaging.TrapV2Message&gt;"/> instance containing the event data.</param>
        public void InvokeMessageReceived(MessageReceivedEventArgs<TrapV2Message> e)
        {
            EventHandler<MessageReceivedEventArgs<TrapV2Message>> handler = MessageReceived;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
    }
}
