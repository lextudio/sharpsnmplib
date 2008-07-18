/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a TRAP v1 received event.
    /// </summary>
    public sealed class TrapV1ReceivedEventArgs : EventArgs
    {      
        private TrapV1Message _trap;
        private IPEndPoint _agent;
        
        /// <summary>
        /// Creates a <see cref="TrapV1ReceivedEventArgs"/> 
        /// </summary>
        /// <param name="trap">Trap message</param>
        /// <param name="agent">Agent</param>
        public TrapV1ReceivedEventArgs(IPEndPoint agent, TrapV1Message trap)
        {
            _agent = agent;
            _trap = trap;
        }
        
        /// <summary>
        /// Trap message.
        /// </summary>
        public TrapV1Message Trap
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
        /// Returns a <see cref="String"/> that represents this <see cref="TrapV1ReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trap received event args: trap message: " + _trap + "; agent: " + _agent;
        }
    }
}
