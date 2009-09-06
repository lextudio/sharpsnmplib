/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 9/6/2009
 * Time: 4:53 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Description of MessageFactoryException.
    /// </summary>
    public class SharpMessageFactoryException : SharpSnmpException
    {
        private byte[] _bytes;
        /// <summary>
        /// Creates a <see cref="SharpMessageFactoryException"/>.
        /// </summary>
        public SharpMessageFactoryException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpMessageFactoryException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SharpMessageFactoryException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpMessageFactoryException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SharpMessageFactoryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        #if (!SILVERLIGHT)
        /// <summary>
        /// Creates a <see cref="SharpMessageFactoryException"/> instance.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        protected SharpMessageFactoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endif
        
        /// <summary>
        /// Bytes.
        /// </summary>
        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SharpMessageFactoryException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SharpMessageFactoryInnerException: " + Message;
        }
    }
}
