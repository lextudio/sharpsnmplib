using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a INFORM request received event.
    /// </summary>
    public sealed class InformRequestReceivedEventArgs : EventArgs
    {
        private readonly InformRequestMessage _inform;
        private readonly IPEndPoint _sender;
      
        /// <summary>
        /// Creates a <see cref="InformRequestReceivedEventArgs"/> 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="inform">INFORM message</param>
        public InformRequestReceivedEventArgs(IPEndPoint sender, InformRequestMessage inform)
        {
            _sender = sender;
            _inform = inform;
        }
       
        /// <summary>
        /// INFORM message.
        /// </summary>
        public InformRequestMessage InformRequest
        {
            get
            {
                return _inform;
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
        /// Returns a <see cref="String"/> that represents this <see cref="InformRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "INFORM request received event args: INFORM message: " + _inform + "; sender: " + _sender;
        }
    }
}

