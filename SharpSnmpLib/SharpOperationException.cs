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
#if (!SILVERLIGHT)
using System.Runtime.Serialization;
using System.Security.Permissions; 
#endif

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Operation exception of #SNMP.
    /// </summary>
    [Serializable]
    public class SharpOperationException : SharpSnmpException
    {
        /// <summary>
        /// Agent address.
        /// </summary>
        private IPAddress agentAddress;
        
        /// <summary>
        /// Agent address.
        /// </summary>
        public IPAddress Agent
        {
            get { return agentAddress; }
            protected set { agentAddress = value; }
        }
        
        /// <summary>
        /// Creates a <see cref="SharpOperationException"/> instance.
        /// </summary>
        public SharpOperationException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpOperationException"/> instance with a specific <see cref="string"/>.
        /// </summary>
        /// <param name="message"></param>
        public SharpOperationException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpOperationException"/> instance with a specific <see cref="string"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SharpOperationException(string message, Exception inner) : base(message, inner) 
        { 
        }
#if (!SILVERLIGHT)    
        /// <summary>
        /// Creates a <see cref="SharpOperationException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SharpOperationException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            
            agentAddress = (IPAddress)info.GetValue("Agent", typeof(IPAddress));
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
            info.AddValue("Agent", agentAddress);
        }
#endif
        /// <summary>
        /// Details on operation.
        /// </summary>
        public override string Details
        {
            get
            {
                return Message + ". Agent: " + Agent;
            }
        }
     
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SharpOperationException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SharpOperationException: " + Details;
        }
     
        /// <summary>
        /// Creates a <see cref="SharpOperationException"/> with a specific <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="agent">Agent address</param>
        public static SharpOperationException Create(string message, IPAddress agent)
        {
            SharpOperationException ex = new SharpOperationException(message);
            ex.agentAddress = agent;
            return ex;
        }
    }
}