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

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of Trap.
	/// </summary>
	public class TrapMessage: ISnmpMessage, ISnmpData
	{
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                "SNMPv1 trap: agent address: {0}; time stamp: {1}; community: {2}; enterprise: {3}; generic: {4}; specific: {5}; varbind count: {6}",
                AgentAddress, TimeStamp, Community, Enterprise, GenericId, SpecificId, Variables.Count);
        }
        int _time;
        public int TimeStamp
        {
            get
            {
                return _time;
            }
        }
        string _community;
        public string Community
        {
            get
            {
                return _community;
            }
        }
        ObjectIdentifier _enterprise;
        public ObjectIdentifier Enterprise
        {
            get
            {
                return _enterprise;
            }
        }
        IPAddress _ip;
        public IPAddress AgentAddress
        {
            get
            {
                return _ip;
            }
        }
        int _generic;
        public int GenericId
        {
            get
            {
                return _generic;
            }
        }
        int _specific;
        public int SpecificId
        {
            get
            {
                return _specific;
            }
        }
        IList<Variable> _varbinds;
        public IList<Variable> Variables
        {
            get
            {
                return _varbinds;
            }
        }
        VersionCode _version;
        public VersionCode Version
        {
        	get {
        		return _version;
        	}
        }
		
		public TrapMessage(SnmpArray body)
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
			_version = (VersionCode)((Int)body.Items[0]).ToInt32();
			TrapV1Pdu trapPdu = (TrapV1Pdu)_pdu;
			_enterprise = trapPdu.Enterprise;
			_ip = trapPdu.Agent.ToIPAddress();
			_generic = trapPdu.Generic;
			_specific = trapPdu.Specific;
			_time = trapPdu.TimeStamp.ToInt32();
			_varbinds = trapPdu.Variables;
		}
		
		ISnmpPdu _pdu;
		
		public SnmpType TypeCode {
			get {
				return SnmpType.TrapPDUv1;
			}
		}
		
		byte[] _bytes;
		
		public byte[] ToBytes()
		{
			if (null == _bytes) 
			{
				_bytes = ((TrapV1Pdu)_pdu).ToMessageBody(_version, _community).ToBytes();
			}
			return _bytes;
		}
		
		public ISnmpPdu Pdu {
			get {
				return _pdu;
			}
		}
	}
}
