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

namespace SharpSnmpLib
{
    [Serializable]
	public class SharpOperationException: SharpSnmpException
	{
		IPAddress _agent;
		
		public IPAddress Agent {
			get { return _agent; }
            set { _agent = value; }
		}
	        
        public SharpOperationException() { }

        public SharpOperationException(string message) : base(message) { }

        public SharpOperationException(string message, Exception inner) : base(message, inner) { }

        protected SharpOperationException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            _agent = (IPAddress)info.GetValue("Agent", typeof(IPAddress));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Agent", _agent);
        }
	}
}
