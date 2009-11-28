/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/28/2009
 * Time: 7:41 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Read-only exception. Raised when SET operation is performed on a read-only object.
    /// </summary>
    [Serializable]
    public class ReadOnlyException : Exception
    {
        /// <summary>
        /// Creates a <see cref="ReadOnlyException"/>.
        /// </summary>
        public ReadOnlyException() 
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="ReadOnlyException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public ReadOnlyException(string message) : base(message) 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="ReadOnlyException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public ReadOnlyException(string message, Exception inner) 
            : base(message, inner) 
        {
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Creates a <see cref="ReadOnlyException"/> instance.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        protected ReadOnlyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        } 
#endif
       
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="ReadOnlyException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ReadOnlyException: " + Message;
        }        
     
        /// <summary>
        /// Details on operation.
        /// </summary>
        public virtual string Details
        {
            get
            {
                return Message;
            }
        }
    }
}
