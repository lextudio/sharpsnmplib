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
    public sealed class SharpErrorException : SharpOperationException
    {	
		int _status;
		
		public int Status {
			get { return _status; }
            set { _status = value; }
		}
		int _index;
		
		public int Index {
			get { return _index; }
            set { _index = value; }
		}		
		        
        public SharpErrorException() { }
        public SharpErrorException(string message) : base(message) 
        {
		}
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
            _status = info.GetInt32("Status");
            _index = info.GetInt32("Index");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Status", _status);
            info.AddValue("Index", _index);
        }
	}
}
