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
	/// Error exception of #SNMP.
	/// </summary>
    [Serializable]
    public sealed class SharpErrorException : SharpOperationException
    {	
		ErrorCode _status;
		int _index;
        ObjectIdentifier _id;
		/// <summary>
		/// Error status.
		/// </summary>
		public ErrorCode Status {
			get { return _status; }
            set { _status = value; }
		}
		/// <summary>
		/// Error index.
		/// </summary>
		public int Index {
			get { return _index; }
            set { _index = value; }
		}
        /// <summary>
        /// Error item OID.
        /// </summary>
        public ObjectIdentifier Id
        {
            get { return _id; }
            set { _id = value; }
        }
		/// <summary>
		/// Creates a <see cref="SharpErrorException"/> instance.
		/// </summary>
        public SharpErrorException() { }
        /// <summary>
        /// Creates a <see cref="SharpErrorException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SharpErrorException(string message) : base(message) 
        {
		}
        /// <summary>
        /// Creates a <see cref="SharpErrorException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SharpErrorException(string message, Exception inner) 
        	: base(message, inner) 
        {
        }

        private SharpErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            _status = (ErrorCode)info.GetValue("Status", typeof(ErrorCode));
            _index = info.GetInt32("Index");
            _id = (ObjectIdentifier)info.GetValue("Id", typeof(ObjectIdentifier));
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
            info.AddValue("Status", _status);
            info.AddValue("Index", _index);
            info.AddValue("Id", _id);
        }
        /// <summary>
        /// Details on error.
        /// </summary>
        public override string Details
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "{0}. {1}. Index: {2}. Errored Object ID: {3}",
                    Message, Status, Index, Id);
            }
        }
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SharpErrorException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SharpErrorException: " + Details;
        }
	}
}
