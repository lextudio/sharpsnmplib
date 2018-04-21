using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    public interface IListener : IDisposable
    {
        UserRegistry Users { get; }
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;
        bool Active { get; }
        IList<IListenerBinding> Bindings { get; }

        void Start();
        void Stop();
        void AddBinding(IPEndPoint iPEndPoint);
        void ClearBindings();
    }
}
