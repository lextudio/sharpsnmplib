/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/9
 * Time: 19:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a GET BULK request received event.
    /// </summary>
    public sealed class GetBulkRequestReceivedEventArgs : EventArgs
    {
        private readonly GetBulkRequestMessage _request;
        private readonly IPEndPoint _sender;

        /// <summary>
        /// Creates a <see cref="GetBulkRequestReceivedEventArgs"/>. 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="request">GET BULK request message.</param>
        public GetBulkRequestReceivedEventArgs(IPEndPoint sender, GetBulkRequestMessage request)
        {
            _sender = sender;
            _request = request;
        }

        /// <summary>
        /// GET BULK request message.
        /// </summary>
        public GetBulkRequestMessage Request
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
        /// Returns a <see cref="String"/> that represents this <see cref="GetBulkRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET BULK received event args: GET BULK request message: " + _request + "; sender: " + _sender;
        }
    }
}