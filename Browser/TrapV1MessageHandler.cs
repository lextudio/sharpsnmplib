using System;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Browser
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class TrapV1MessageHandler : IMessageHandler
    {
        public ResponseData Handle(SnmpContext context, ObjectStore store)
        {
            InvokeMessageReceived(new MessageReceivedEventArgs<TrapV1Message>(context.Sender, (TrapV1Message)context.Request, context.Binding));
            return new ResponseData();
        }

        public event EventHandler<MessageReceivedEventArgs<TrapV1Message>> MessageReceived;

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
