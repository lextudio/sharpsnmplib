// Manager v3 listener adapter class.
// Copyright (C) 2009-2010 Lex Li
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/31/2009
 * Time: 11:53 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Description of ManagerV3ListenerAdapter.
    /// </summary>
    [Obsolete("Please switch to SnmpEngine.")]
    public class ManagerV3ListenerAdapter : IListenerAdapter
    {
        /// <summary>
        /// Occurs when a <see cref="TrapV2Message"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<TrapV2Message>> TrapV2Received;

        /// <summary>
        /// Occurs when a <see cref="InformRequestMessage"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<InformRequestMessage>> InformRequestReceived;

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="binding">The binding.</param>
        public void Process(ISnmpMessage message, System.Net.IPEndPoint sender, ListenerBinding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }
            
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            
            if (message.Version != VersionCode.V3)
            {
                return;
            }
            
            switch (message.Pdu.TypeCode)
            {
                case SnmpType.TrapV2Pdu:
                    {
                        EventHandler<MessageReceivedEventArgs<TrapV2Message>> handler = TrapV2Received;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<TrapV2Message>(sender, (TrapV2Message)message, binding));
                        }

                        break;
                    }

                case SnmpType.InformRequestPdu:
                    {
                        InformRequestMessage inform = (InformRequestMessage)message;
                        binding.SendResponse(inform.GenerateResponse(), sender);

                        EventHandler<MessageReceivedEventArgs<InformRequestMessage>> handler = InformRequestReceived;
                        if (handler != null)
                        {
                            handler(this, new MessageReceivedEventArgs<InformRequestMessage>(sender, inform, binding));
                        }

                        break;
                    }
                    
                default:
                    break;
            }
        }
    }
}
