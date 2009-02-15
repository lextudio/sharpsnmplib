/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/9
 * Time: 19:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a SET request received event.
    /// </summary>
    [Obsolete("Use MessageReceivedEventArgs instead.")]
    public sealed class SetRequestReceivedEventArgs : EventArgs
    {
        private readonly SetRequestMessage _request;
        private readonly IPEndPoint _sender;

        /// <summary>
        /// Creates a <see cref="SetRequestReceivedEventArgs"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="request">SET request message.</param>
        public SetRequestReceivedEventArgs(IPEndPoint sender, SetRequestMessage request)
        {
            _sender = sender;
            _request = request;
        }

        /// <summary>
        /// SET request message.
        /// </summary>
        public SetRequestMessage Request
        {
            get
            {
                return _request;
            }
        }

        /// <summary>
        /// Sender.
        /// </summary>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SetRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SET request received event args: SET request message: " + _request + "; sender: " + _sender;
        }
    }
}