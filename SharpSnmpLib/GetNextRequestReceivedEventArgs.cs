/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/9
 * Time: 19:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a GET NEXT request received event.
    /// </summary>
    public sealed class GetNextRequestReceivedEventArgs : EventArgs
    {
        private readonly GetNextRequestMessage _request;
        private readonly IPEndPoint _sender;

        /// <summary>
        /// Creates a <see cref="GetNextRequestReceivedEventArgs"/>. 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="request">GET NEXT request message.</param>
        public GetNextRequestReceivedEventArgs(IPEndPoint sender, GetNextRequestMessage request)
        {
            _sender = sender;
            _request = request;
        }

        /// <summary>
        /// GET NEXT request message.
        /// </summary>
        public GetNextRequestMessage Request
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
        /// Returns a <see cref="String"/> that represents this <see cref="GetNextRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET NEXT received event args: GET NEXT request message: " + _request + "; sender: " + _sender;
        }
    }
}