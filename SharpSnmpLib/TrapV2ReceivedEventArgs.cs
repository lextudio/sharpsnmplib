using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a trap received event.
    /// </summary>
    [Obsolete("Use MessageReceivedEventArgs instead.")]
    public sealed class TrapV2ReceivedEventArgs : EventArgs
    {
        private readonly TrapV2Message _trap;
        private readonly IPEndPoint _sender;
      
        /// <summary>
        /// Creates a <see cref="TrapV2ReceivedEventArgs"/>. 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="trap">Trap message.</param>
        public TrapV2ReceivedEventArgs(IPEndPoint sender, TrapV2Message trap)
        {
            _sender = sender;
            _trap = trap;
        }
       
        /// <summary>
        /// Trap message.
        /// </summary>
        public TrapV2Message Trap
        {
            get
            {
                return _trap;
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
        /// Returns a <see cref="String"/> that represents this <see cref="TrapV2ReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trap received event args: trap message: " + _trap + "; sender: " + _sender;
        }
    }
}

