using System;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;

namespace Lextm.SharpSnmpLib.Browser
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class InformMessageHandler : IMessageHandler
    {
        public ResponseData Handle(SnmpContext context, ObjectStore store)
        {
            InvokeMessageReceived(new MessageReceivedEventArgs<InformRequestMessage>(context.Sender, (InformRequestMessage)context.Request, context.Binding));
            return new ResponseData(context.Request.Pdu.Variables, ErrorCode.NoError, 0);
        }

        public event EventHandler<MessageReceivedEventArgs<InformRequestMessage>> MessageReceived;

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
