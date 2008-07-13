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
namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// GET response message.
	/// </summary>
	public class GetResponseMessage: ISnmpMessage
	{
		ISnmpPdu _pdu;
		int _sequenceNumber;
        ErrorCode _errorStatus;
        int _errorIndex;
		IList<Variable> _variables;
        byte[] _bytes;
        VersionCode _version;
        string _community;
		/// <summary>
		/// Creates a <see cref="GetResponseMessage"/> with a specific <see cref="Sequence"/>.
		/// </summary>
		/// <param name="body">Message body</param>
		public GetResponseMessage(Sequence body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (body.Items.Count != 3)
			{
				throw new ArgumentException("wrong message body");
			}
            _community = body.Items[1].ToString();
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();	
			_pdu = (ISnmpPdu)body.Items[2];
			if (_pdu.TypeCode != TypeCode)
			{
				throw new ArgumentException("wrong message type");
			}
			GetResponsePdu pdu = (GetResponsePdu)_pdu;
			_sequenceNumber = pdu.SequenceNumber;
            _errorStatus = pdu.ErrorStatus;
            _errorIndex = pdu.ErrorIndex;
			_variables = _pdu.Variables;
            _bytes = body.ToBytes();
		}
        /// <summary>
        /// Error status.
        /// </summary>
        public ErrorCode ErrorStatus
        {
            get
            {
                return _errorStatus;
            }
        }
        /// <summary>
        /// Error index.
        /// </summary>
        public int ErrorIndex
        {
            get
            {
                return _errorIndex;
            }
        }
		
		internal int SequenceNumber
		{
			get
			{
				return _sequenceNumber;
			}
		}
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }
		/// <summary>
		/// PDU.
		/// </summary>
		public ISnmpPdu Pdu {
			get {
				return _pdu;
			}
		}
		/// <summary>
		/// Type code
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.GetResponsePdu;
			}
		}
		/// <summary>
		/// Converts to byte format.
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			return _bytes;
		}
		/// <summary>
		/// Returns a <see cref="String"/> that represents this <see cref="GetResponseMessage"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "GET response message: version: " + _version + "; " + _community + "; " + _pdu;
		}
	}
}
