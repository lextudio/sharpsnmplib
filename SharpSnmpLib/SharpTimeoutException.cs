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
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SharpSnmpLib
{
    [Serializable]
    public sealed class SharpTimeoutException : SharpOperationException
    {
        int _timeout;

        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public SharpTimeoutException() { }

        public SharpTimeoutException(string message) : base(message) { }

        public SharpTimeoutException(string message, Exception inner) : base(message, inner) { }

        private SharpTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            _timeout = info.GetInt32("Timeout");
        }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {    
            base.GetObjectData(info, context);
            info.AddValue("Timeout", _timeout);
        }
    }
}
