using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a trap received event.
    /// </summary>
    public sealed class TrapV2ReceivedEventArgs : EventArgs
    {
        TrapV2Message _trap;
        IPEndPoint _agent;
        /// <summary>
        /// Creates a <see cref="TrapV2ReceivedEventArgs"/> 
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <param name="trap">Trap message</param>
        public TrapV2ReceivedEventArgs(EndPoint agent, TrapV2Message trap)
        {
            _agent = (IPEndPoint)agent;
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
        /// Agent.
        /// </summary>
        public IPEndPoint Agent
        {
            get { return _agent; }
        }
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="TrapV2ReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trap received event args: trap message: " + _trap + "; agent: " + _agent;
        }
    }
}

