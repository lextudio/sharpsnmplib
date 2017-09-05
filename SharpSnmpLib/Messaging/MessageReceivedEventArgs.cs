// Message received event args class.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

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
}