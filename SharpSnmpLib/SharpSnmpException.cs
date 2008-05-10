/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib
{	
	/// <summary>
	/// Base exception type of #SNMP.
	/// </summary>
    [Serializable]
	public class SharpSnmpException: Exception
	{
		/// <summary>
		/// Creates a <see cref="SharpSnmpException"/>.
		/// </summary>
        public SharpSnmpException() { }
        /// <summary>
        /// Creates a <see cref="SharpSnmpException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SharpSnmpException(string message) : base(message) 
        {
		}
        /// <summary>
        /// Creates a <see cref="SharpSnmpException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SharpSnmpException(string message, Exception inner) 
        	: base(message, inner) 
        {
        }
		/// <summary>
		/// Creates a <see cref="SharpSnmpException"/> instance.
		/// </summary>
		/// <param name="info">Info</param>
		/// <param name="context">Context</param>
        protected SharpSnmpException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}