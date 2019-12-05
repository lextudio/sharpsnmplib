using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace SharpSnmpLib.DTLS
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
        public MessageReceivedEventArgs(IPEndPoint sender, ISnmpMessage message, SecureListenerBinding binding)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

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
        public SecureListenerBinding Binding { get; private set; }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}; sender: {1}", Message, Sender);
        }
    }
}
