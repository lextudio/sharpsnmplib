// Inform request message received event args.
// Copyright (C) 2010 Lex Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Inform request message received event args.
    /// </summary>
    public sealed class InformRequestMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InformRequestMessageReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="request">The request.</param>
        /// <param name="binding">The binding.</param>
        public InformRequestMessageReceivedEventArgs(IPEndPoint sender, InformRequestMessage request, IListenerBinding binding)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }

            Sender = sender;
            InformRequestMessage = request;
            Binding = binding;
        }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public IPEndPoint Sender { get; private set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>The request.</value>
        public InformRequestMessage InformRequestMessage { get; private set; }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        public IListenerBinding Binding { get; private set; }
    }
}