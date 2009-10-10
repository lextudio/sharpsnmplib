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
using System.Security.Permissions;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Description of MessageFactoryException.
    /// </summary>
    [Serializable]
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
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            
            _bytes = (byte[])info.GetValue("Bytes", typeof(byte[]));
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
            info.AddValue("Bytes", _bytes);
        }
        #endif
        
        /// <summary>
        /// Bytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
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
