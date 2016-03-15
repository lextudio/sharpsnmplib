﻿// INFORM message handler class.
// Copyright (C) 2010 Lex Li
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
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Message handler for INFORM.
    /// </summary>    
    public sealed class InformRequestMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public void Handle(ISnmpContext context, IObjectStore store)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            
            InvokeMessageReceived(new InformRequestMessageReceivedEventArgs(context.Sender, (InformRequestMessage)context.Request, context.Binding));
            context.CopyRequest(ErrorCode.NoError, 0);
        }

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<InformRequestMessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Invokes the message received.
        /// </summary>
        /// <param name="e">The <see cref="Lextm.SharpSnmpLib.Pipeline.InformRequestMessageReceivedEventArgs"/> instance containing the event data.</param>
        private void InvokeMessageReceived(InformRequestMessageReceivedEventArgs e)
        {
            var handler = MessageReceived;
            if (handler != null) 
            {
                handler(this, e);
            }
        }
    }
}
