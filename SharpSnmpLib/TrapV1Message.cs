/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Trap message.
	/// </summary>
	public class TrapV1Message: ISnmpMessage, ISnmpData, IDisposable
	{
        int _time;   
        UdpClient udp = new UdpClient();
        string _community;
        ObjectIdentifier _enterprise;
        IPAddress _agent;
        GenericCode _generic;
        int _specific;
        IList<Variable> _variables;
        VersionCode _version;
        ISnmpPdu _pdu;		
		byte[] _bytes; 
		
		/// <summary>
		/// Creates a <see cref="TrapV1Message"/> with all content.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="enterprise">Enterprise</param>
		/// <param name="generic">Generic</param>
		/// <param name="specific">Specific</param>
		/// <param name="time">Time</param>
		/// <param name="variables">Variables</param>
		public TrapV1Message(VersionCode version, IPAddress agent, string community, ObjectIdentifier enterprise, GenericCode generic, int specific, int time, IList<Variable> variables)
		{
			_version = version;
			_agent = agent;
			_community = community;
			_variables = variables;
			_enterprise = enterprise;
			_generic = generic;
			_specific = specific;
			_time = time;
			TrapV1Pdu pdu = new TrapV1Pdu(_enterprise, new IP(_agent), _generic, new Integer32(_specific), new TimeTicks(_time),
							_variables);
			_bytes = pdu.ToMessageBody(_version, _community).ToBytes();
		}
		/// <summary>
		/// Creates a <see cref="TrapV1Message"/> instance with a message body.
		/// </summary>
		/// <param name="body">Message body</param>
		public TrapV1Message(Sequence body)
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
			_agent = trapPdu.AgentAddress.ToIPAddress();
			_generic = trapPdu.Generic;
			_specific = trapPdu.Specific;
			_time = trapPdu.TimeStamp.ToInt32();
			_variables = trapPdu.Variables;
		}		

		/// <summary>
		/// Sends this <see cref="TrapV1Message"/>.
		/// </summary>
		/// <param name="manager">Manager address</param>
		/// <param name="port">Port number</param>
		public void Send(IPAddress manager, int port)
		{
			byte[] bytes = _bytes;
			IPEndPoint agent = new IPEndPoint(manager, port);
			udp.Send(bytes,bytes.Length,agent);
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
                return _agent;
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
                return _variables;
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
				return SnmpType.TrapV1Pdu;
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
		/// Returns a <see cref="String"/> that represents the current <see cref="TrapV1Message"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                "SNMPv1 trap: agent address: {0}; time stamp: {1}; community: {2}; enterprise: {3}; generic: {4}; specific: {5}; varbind count: {6}",
                AgentAddress, TimeStamp, Community, Enterprise, Generic, Specific, Variables.Count);
        }
		
		private bool _disposed;
		/// <summary>
		/// Finalizer of <see cref="TrapV1Message"/>.
		/// </summary>
		~TrapV1Message()
		{
			Dispose(false);
		}
		/// <summary>
		/// Releases all resources used by the <see cref="TrapV1Message"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Disposes of the resources (other than memory) used by the <see cref="TrapV1Message"/>.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) {
				return;
			}
			if (disposing) {
				(udp as IDisposable).Dispose();
			}
			_disposed = true;
		}
	}
}
