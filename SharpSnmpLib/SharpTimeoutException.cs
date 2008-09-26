/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Timeout exception type of #SNMP.
    /// </summary>
    [Serializable]
    public sealed class SharpTimeoutException : SharpOperationException
    {
        private int _timeout;
        
        /// <summary>
        /// Timeout.
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
        }
        
        /// <summary>
        /// Creates a <see cref="SharpTimeoutException"/> instance.
        /// </summary>
        public SharpTimeoutException() 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpTimeoutException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SharpTimeoutException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpTimeoutException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/> instance.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SharpTimeoutException(string message, Exception inner) : base(message, inner)
        {
        }

        private SharpTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            
            _timeout = info.GetInt32("Timeout");
        }
        
        /// <summary>
        /// Gets object data.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {    
            base.GetObjectData(info, context);
            info.AddValue("Timeout", _timeout);
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SharpTimeoutException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SharpTimeoutException: timeout: " + _timeout.ToString(CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Creates a <see cref="SharpTimeoutException"/>.
        /// </summary>
        /// <param name="agent">Agent address</param>
        /// <param name="timeout">Timeout</param>
        /// <returns></returns>
        public static SharpTimeoutException Create(IPAddress agent, int timeout)
        {
            SharpTimeoutException ex = new SharpTimeoutException();
            ex.Agent = agent;
            ex._timeout = timeout;
            return ex;
        }
    }
}