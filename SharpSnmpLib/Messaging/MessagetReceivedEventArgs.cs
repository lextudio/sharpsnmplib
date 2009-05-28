using System;
using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
{       
    /// <summary>
    /// The <see cref="EventArgs"/> for one kind of <see cref="ISnmpMessage"/>, used when that kind of message is received.
    /// </summary>
    public sealed class MessageReceivedEventArgs<T> : EventArgs where T : ISnmpMessage
    {
        private readonly ISnmpMessage _message;
        private readonly IPEndPoint _sender;
        
        /// <summary>
        /// Creates a <see cref="MessageReceivedEventArgs{T}"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">The message received.</param>
        public MessageReceivedEventArgs(IPEndPoint sender, T message)
        {
            _sender = sender;
            _message = message;
        }

        /// <summary>
        /// The received message.
        /// </summary>
        public T Message
        {
            get { return (T)_message; }
        }
        
        /// <summary>
        /// Sender.
        /// </summary>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _message.GetType().ToString() + _message + "; sender: " + _sender;
        }
    }
}