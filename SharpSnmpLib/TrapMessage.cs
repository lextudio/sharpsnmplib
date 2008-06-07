/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Trap message.
	/// </summary>
	public class TrapMessage: ISnmpMessage, ISnmpData
	{
        int _time;        
        string _community;
        ObjectIdentifier _enterprise;
        IPAddress _ip;
        GenericCode _generic;
        int _specific;
        IList<Variable> _varbinds;
        VersionCode _version;
        ISnmpPdu _pdu;		
		byte[] _bytes; 
		/// <summary>
		/// Creates a <see cref="TrapMessage"/> instance with a message body.
		/// </summary>
		/// <param name="body">Message body</param>
		public TrapMessage(Sequence body)
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
			_community = body.Items[1].ToString();
			_version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
			TrapV1Pdu trapPdu = (TrapV1Pdu)_pdu;
			_enterprise = trapPdu.Enterprise;
			_ip = trapPdu.AgentAddress.ToIPAddress();
			_generic = trapPdu.Generic;
			_specific = trapPdu.Specific;
			_time = trapPdu.TimeStamp.ToInt32();
			_varbinds = trapPdu.Variables;
		}
		/// <summary>
		/// Time stamp.
		/// </summary>
        public int TimeStamp
        {
            get
            {
                return _time;
            }
        }
        /// <summary>
        /// Community name.
        /// </summary>
        public string Community
        {
            get
            {
                return _community;
            }
        }
        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise
        {
            get
            {
                return _enterprise;
            }
        }
        /// <summary>
        /// Agent address.
        /// </summary>
        public IPAddress AgentAddress
        {
            get
            {
                return _ip;
            }
        }
       	/// <summary>
       	/// Generic type.
       	/// </summary>
        public GenericCode Generic
        {
            get
            {
                return _generic;
            }
        }
        /// <summary>
        /// Specific type.
        /// </summary>
        public int Specific
        {
            get
            {
                return _specific;
            }
        }     
		/// <summary>
		/// Variable binds.
		/// </summary>
		public IList<Variable> Variables
        {
            get
            {
                return _varbinds;
            }
        }
		/// <summary>
		/// Protocol version.
		/// </summary>
        public VersionCode Version
        {
        	get {
        		return _version;
        	}
        }		
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.TrapPDUv1;
			}
		}
		/// <summary>
		/// To byte format.
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			if (null == _bytes) 
			{
				_bytes = ((TrapV1Pdu)_pdu).ToMessageBody(_version, _community).ToBytes();
			}
			return _bytes;
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
		/// Returns a <see cref="String"/> that represents the current <see cref="TrapMessage"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                "SNMPv1 trap: agent address: {0}; time stamp: {1}; community: {2}; enterprise: {3}; generic: {4}; specific: {5}; varbind count: {6}",
                AgentAddress, TimeStamp, Community, Enterprise, Generic, Specific, Variables.Count);
        }
	}
}
