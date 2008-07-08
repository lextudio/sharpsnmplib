/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// GETNEXT request message.
	/// </summary>
	public class GetNextRequestMessage: ISnmpMessage, IDisposable
	{
		VersionCode _version;
		IList<Variable> _variables;
		UdpClient udp = new UdpClient();
		byte[] _bytes;
		IPAddress _agent;
		string _community;
		ISnmpPdu _pdu;
		int _sequenceNumber;
		/// <summary>
		/// Creates a <see cref="GetNextRequestMessage"/> with all contents.
		/// </summary>
		/// <param name="version">Protocol version</param>
		/// <param name="agent">Agent address</param>
		/// <param name="community">Community name</param>
		/// <param name="variables">Variables</param>
		public GetNextRequestMessage(VersionCode version, IPAddress agent, string community, IList<Variable> variables)
		{
			_version = version;
			_agent = agent;
			_community = community;
			_variables = variables;
			GetNextRequestPdu pdu = new GetNextRequestPdu(
				ErrorCode.NoError,
				0,
				_variables);
			_sequenceNumber = pdu.SequenceNumber;
			_bytes = pdu.ToMessageBody(_version, _community).ToBytes();
		}
		/// <summary>
		/// Creates a <see cref="GetNextRequestMessage"/> with a specific <see cref="Sequence"/>.
		/// </summary>
		/// <param name="body">Message body</param>
		public GetNextRequestMessage(Sequence body)
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
			GetRequestPdu pdu = (GetRequestPdu)_pdu;
			_variables = pdu.Variables;
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
		/// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
		/// </summary>
		/// <param name="timeout">Timeout</param>
		/// <returns></returns>
		public IList<Variable> Send(int timeout)
		{
			byte[] bytes = _bytes;
			IPEndPoint agent = new IPEndPoint(_agent, 161);
			udp.Send(bytes,bytes.Length,agent);
			IPEndPoint from = new IPEndPoint(IPAddress.Any,0);
			IAsyncResult result = udp.BeginReceive(null, this);
			result.AsyncWaitHandle.WaitOne(timeout, false);
			if (!result.IsCompleted)
			{
				throw SharpTimeoutException.Create(_agent, timeout);
			}
			bytes = udp.EndReceive(result, ref from);
			MemoryStream m = new MemoryStream(bytes, false);
			ISnmpMessage message = MessageFactory.ParseMessage(m);
			if (message.TypeCode != SnmpType.GetResponsePdu) {
				throw SharpOperationException.Create("wrong response type", _agent);
			}
			GetResponseMessage response = (GetResponseMessage)message;
			if (response.SequenceNumber != SequenceNumber) {
				throw SharpOperationException.Create("wrong response sequence", _agent);
			}
			if (response.ErrorStatus != ErrorCode.NoError)
			{
				throw SharpErrorException.Create("error in response", 
				                                 _agent,
				                                 response.ErrorStatus, 
				                                 response.ErrorIndex,
				                                 response.Variables[response.ErrorIndex - 1].Id);
			}
			return response.Variables;
		}
				
		internal int SequenceNumber
		{
			get
			{
				return _sequenceNumber;
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
		/// PDU.
		/// </summary>
		public ISnmpPdu Pdu {
			get {
				return _pdu;
			}
		}
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.GetRequestPdu;
			}
		}
		
		private bool _disposed;
		/// <summary>
		/// Finalizer of <see cref="GetNextRequestMessage"/>.
		/// </summary>
		~GetNextRequestMessage()
		{
			Dispose(false);
		}
		/// <summary>
		/// Releases all resources used by the <see cref="GetNextRequestMessage"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Disposes of the resources (other than memory) used by the <see cref="GetNextRequestMessage"/>.
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
		/// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetNextRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET NEXT request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
	}
}
