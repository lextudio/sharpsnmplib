/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 18:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
namespace SharpSnmpLib
{
	/// <summary>
	/// Description of GetResponseMessage.
	/// </summary>
	public class GetResponseMessage: ISnmpMessage
	{
		ISnmpPdu _pdu;
		IList<Variable> _variables;
		
		public GetResponseMessage(SnmpArray body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (body.Items.Count != 3)
			{
				throw new ArgumentException("wrong message body");
			}
			_pdu = (ISnmpPdu)body.Items[2];
			if (_pdu.TypeCode != TypeCode)
			{
				throw new ArgumentException("wrong message type");
			}
			GetResponsePdu pdu = (GetResponsePdu)_pdu;
			_variables = pdu.Variables;
		}

        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }
		
		public ISnmpPdu Pdu {
			get {
				return _pdu;
			}
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.GetResponsePDU;
			}
		}
		
		public byte[] ToBytes()
		{
			throw new NotImplementedException();
		}
	}
}
