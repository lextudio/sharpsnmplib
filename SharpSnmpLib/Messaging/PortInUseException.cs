using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Exception raised when an IP endpoint is already in use.
    /// </summary>
    [Serializable]
    public class PortInUseException : SnmpException
    {
        /// <summary>
        /// Creates a <see cref="PortInUseException"/>.
        /// </summary>
        public PortInUseException()
        {
        }

        /// <summary>
        /// Creates a <see cref="PortInUseException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public PortInUseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a <see cref="PortInUseException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public PortInUseException(string message, Exception inner)
            : base(message, inner)
        {
        }

#if (!SILVERLIGHT && !CF)
        /// <summary>
        /// Creates a <see cref="PortInUseException"/> instance.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        protected PortInUseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Endpoint = (IPEndPoint)info.GetValue("Endpoint", typeof(IPEndPoint));
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
            info.AddValue("Endpoint", Endpoint);
        }
#endif        
        /// <summary>
        /// The endpoint already in use.
        /// </summary>
        public IPEndPoint Endpoint { get; set; }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="PortInUseException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "PortInUseException: " + Message;
        }
    }
}