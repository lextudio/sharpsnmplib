using System;
using System.Globalization;
using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
{ 
    /// <summary>
    /// The <see cref="EventArgs"/> for one kind of <see cref="ISnmpMessage"/>, used when that kind of message is received.
    /// </summary>
    public sealed class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a <see cref="MessageReceivedEventArgs"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">The message received.</param>
        /// <param name="binding">The binding.</param>
        public MessageReceivedEventArgs(IPEndPoint sender, ISnmpMessage message, IListenerBinding binding)
        {
            Sender = sender;
            Message = message;
            Binding = binding;
        }

        /// <summary>
        /// The received message.
        /// </summary>
        public ISnmpMessage Message { get; private set; }

        /// <summary>
        /// Sender.
        /// </summary>
        public IPEndPoint Sender { get; private set; }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        public IListenerBinding Binding { get; private set; }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}; sender: {1}", Message, Sender);
        }
    }
    
    /// <summary>
    /// The <see cref="EventArgs"/> for one kind of <see cref="ISnmpMessage"/>, used when that kind of message is received.
    /// </summary>
    /// <typeparam name="T">The <see cref="ISnmpMessage"/> object received.</typeparam>
    [Obsolete("Please use the non generic version.")]
    public sealed class MessageReceivedEventArgs<T> : EventArgs where T : ISnmpMessage
    {
        /// <summary>
        /// Creates a <see cref="MessageReceivedEventArgs{T}"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">The message received.</param>
        /// <param name="binding">The binding.</param>
        public MessageReceivedEventArgs(IPEndPoint sender, T message, IListenerBinding binding)
        {
            Sender = sender;
            Message = message;
            Binding = binding;
        }

        /// <summary>
        /// The received message.
        /// </summary>
        public T Message { get; private set; }

        /// <summary>
        /// Sender.
        /// </summary>
        public IPEndPoint Sender { get; private set; }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        public IListenerBinding Binding { get; private set; }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}: {1}; sender: {2}", Message.GetType().ToString(), Message, Sender);
        }
    }
}