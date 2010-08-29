using System;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Browser
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class TrapV2MessageHandler : IMessageHandler
    {
        public ResponseData Handle(SnmpContext context, ObjectStore store)
        {
            InvokeMessageReceived(new MessageReceivedEventArgs<TrapV2Message>(context.Sender, (TrapV2Message)context.Request, context.Binding));
            return new ResponseData();
        }

        public event EventHandler<MessageReceivedEventArgs<TrapV2Message>> MessageReceived;

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
