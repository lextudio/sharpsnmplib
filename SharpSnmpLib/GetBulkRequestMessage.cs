/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:13
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
    /// GETBULK request message.
    /// </summary>
    public class GetBulkRequestMessage : ISnmpMessage, IDisposable
    {
        private VersionCode _version;
        private IList<Variable> _variables;
        private UdpClient udp = new UdpClient();
        private byte[] _bytes;
        private IPAddress _agent;
        private string _community;
        private ISnmpPdu _pdu;
        private int _sequenceNumber;
        
        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="version">Protocol version.</param>
        /// <param name="agent">Agent address.</param>
        /// <param name="nonRepeaters">Non-repeaters.</param>
        /// <param name="maxRepetitions">Max repetitions.</param>
        /// <param name="community">Community name.</param>
        /// <param name="variables">Variables.</param>
        public GetBulkRequestMessage(VersionCode version, IPAddress agent, string community, int nonRepeaters, int maxRepetitions, IList<Variable> variables)
        {
            _version = version;
            _agent = agent;
            _community = community;
            _variables = variables;
            GetBulkRequestPdu pdu = new GetBulkRequestPdu(
                nonRepeaters,
                maxRepetitions,
                _variables);
            _sequenceNumber = pdu.SequenceNumber;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }
        
        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public GetBulkRequestMessage(Sequence body)
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
            
            _variables = _pdu.Variables;
            _bytes = body.ToBytes();
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
        /// Sends this <see cref="GetBulkRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        public IList<Variable> Send(int timeout, int port)
        {
            byte[] bytes = _bytes;
            IPEndPoint agent = new IPEndPoint(_agent, port);
            udp.Send(bytes, bytes.Length, agent);
            IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
            IAsyncResult result = udp.BeginReceive(null, this);
            result.AsyncWaitHandle.WaitOne(timeout, false);
            if (!result.IsCompleted)
            {
                throw SharpTimeoutException.Create(_agent, timeout);
            }
            
            bytes = udp.EndReceive(result, ref from);
            MemoryStream m = new MemoryStream(bytes, false);
            ISnmpMessage message = MessageFactory.ParseMessage(m);
            if (message.TypeCode != SnmpType.GetResponsePdu) 
            {
                throw SharpOperationException.Create("wrong response type", _agent);
            }
            
            GetResponseMessage response = (GetResponseMessage)message;
            if (response.SequenceNumber != SequenceNumber)
            {
                throw SharpOperationException.Create("wrong response sequence", _agent);
            }
            
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
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
        public ISnmpPdu Pdu
        {
            get 
            {
                return _pdu;
            }
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode 
        {
            get 
            {
                return SnmpType.GetBulkRequestPdu;
            }
        }
        
        private bool _disposed;
        
        /// <summary>
        /// Finalizer of <see cref="GetBulkRequestMessage"/>.
        /// </summary>
        ~GetBulkRequestMessage()
        {
            Dispose(false);
        }
        
        /// <summary>
        /// Releases all resources used by the <see cref="GetBulkRequestMessage"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="GetBulkRequestMessage"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                (udp as IDisposable).Dispose();
            }
            
            _disposed = true;
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetBulkRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GETBULK request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}