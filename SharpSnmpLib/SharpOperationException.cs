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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Operation exception of #SNMP.
	/// </summary>
    [Serializable]
	public class SharpOperationException: SharpSnmpException
	{
		IPAddress _agent;
		/// <summary>
		/// Agent address.
		/// </summary>
		public IPAddress Agent {
			get { return _agent; }
            set { _agent = value; }
		}
	    
		/// <summary>
		/// Creates a <see cref="SharpOperationException"/> instance.
		/// </summary>
        public SharpOperationException() { }
		/// <summary>
		/// Creates a <see cref="SharpOperationException"/> instance with a specific <see cref="String"/>.
		/// </summary>
		/// <param name="message"></param>
        public SharpOperationException(string message) : base(message) { }
		/// <summary>
		/// Creates a <see cref="SharpOperationException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
        public SharpOperationException(string message, Exception inner) : base(message, inner) { }
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
            _agent = (IPAddress)info.GetValue("Agent", typeof(IPAddress));
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
            info.AddValue("Agent", _agent);
        }
        /// <summary>
        /// Details on operation.
        /// </summary>
        public virtual string Details
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
	}
}
