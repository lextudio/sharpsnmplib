using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET request message.
    /// </summary>
    public class GetRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly IList<Variable> _variables;
        private readonly byte[] _bytes;
        
        // TODO: [Obsolete]
        private readonly IPAddress _agent;
        private readonly OctetString _community;
        private readonly ISnmpPdu _pdu;
        private readonly int _sequenceNumber;
        
        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        [Obsolete("Please use the overload version.")]
        public GetRequestMessage(VersionCode version, IPAddress agent, OctetString community, IList<Variable> variables)
        {
            _version = version;
            _agent = agent;
            _community = community;
            _variables = variables;
            GetRequestPdu pdu = new GetRequestPdu(
                ErrorCode.NoError,
                0,
                _variables);
            _sequenceNumber = pdu.SequenceNumber;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }

        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public GetRequestMessage(VersionCode version, OctetString community, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _variables = variables;
            GetRequestPdu pdu = new GetRequestPdu(
                ErrorCode.NoError,
                0,
                _variables);
            _sequenceNumber = pdu.SequenceNumber;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }

        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public GetRequestMessage(Sequence body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }
            
            if (body.Items.Count != 3)
            {
                throw new ArgumentException("wrong message body");
            }
            
            _community = (OctetString)body.Items[1];
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
            _pdu = (ISnmpPdu)body.Items[2];
            if (_pdu.TypeCode != SnmpType.GetRequestPdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            _sequenceNumber = ((GetRequestPdu)_pdu).SequenceNumber;
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
        #region broadcast
        /// <summary>
        /// Broadcasts request for new agents.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        [Obsolete("Please use the overload version. Otherwise, make sure you called the obsolete constructor for this object.")]
        public IDictionary<IPEndPoint, Variable> Broadcast(int timeout, int port)
        {
            byte[] bytes = _bytes;
            IPEndPoint agent = new IPEndPoint(_agent, port);
            IDictionary<IPEndPoint, Variable> result;
            using (UdpClient udp = new UdpClient())
            {
                #if (!CF)
                udp.EnableBroadcast = true;
                #endif
                udp.Send(bytes, bytes.Length, agent);
                result = ReceiveResponses(udp, timeout);
                udp.Close();
            }
            
            return result;
        }

        /// <summary>
        /// Broadcasts request for new agents.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agents.</param>
        /// <returns></returns>
        public IDictionary<IPEndPoint, Variable> Broadcast(int timeout, IPEndPoint receiver)
        {
            byte[] bytes = _bytes;
            IDictionary<IPEndPoint, Variable> result;
            using (UdpClient udp = new UdpClient())
            {
                #if (!CF)
                udp.EnableBroadcast = true;
                #endif
                udp.Send(bytes, bytes.Length, receiver);
                result = ReceiveResponses(udp, timeout);
                udp.Close();
            }

            return result;
        }

        private IDictionary<IPEndPoint, Variable> ReceiveResponses(UdpClient udp, int timeout)
        {
            IDictionary<IPEndPoint, Variable> result = new Dictionary<IPEndPoint, Variable>();
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += delegate(object sender, DoWorkEventArgs e)
                {
                    Socket watcher = ((UdpClient)e.Argument).Client;
                    IPEndPoint source = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint senderRemote = source;
                    #if CF
                    byte[] msg = new byte[8192];
                    #else
                    byte[] msg = new byte[watcher.ReceiveBufferSize];
                    #endif
                    uint loops = 0;
                    while (!((BackgroundWorker)sender).CancellationPending)
                    {
                        int number = watcher.Available;
                        if (number == 0)
                        {
                            if (Environment.ProcessorCount == 1 || unchecked(++loops % 100) == 0)
                            {
                                Thread.Sleep(1);
                            }
                            else
                            {
                                Thread.SpinWait(20);
                            }
                            
                            continue;
                        }
                        
                        watcher.ReceiveFrom(msg, ref senderRemote);
                        ISnmpMessage message = MessageFactory.ParseMessages(msg)[0];
                        if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
                        {
                            continue;
                        }
                        
                        GetResponseMessage response = (GetResponseMessage)message;
                        if (response.SequenceNumber != SequenceNumber)
                        {
                            continue;
                        }
                        
                        if (response.ErrorStatus != ErrorCode.NoError)
                        {
                            continue;
                        }
                        
                        result.Add((IPEndPoint)senderRemote, response.Variables[0]);
                    }
                };
                worker.RunWorkerAsync(udp);
                Thread.Sleep(timeout);
                worker.CancelAsync();
                while (worker.IsBusy)
                {
                    Thread.Sleep(100);
                }
            }
            
            return result;
        }
        #endregion
        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        [Obsolete("Please use GetResponse instead. Otherwise, make sure you called the obsolete constructor for this object.")]
        public IList<Variable> Send(int timeout, int port)
        {
            byte[] bytes = _bytes;
            ByteTool.Capture(bytes); // log request
            IPEndPoint agent = new IPEndPoint(_agent, port);
            using (UdpClient udp = new UdpClient())
            {
                udp.Send(bytes, bytes.Length, agent);
                IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
                IAsyncResult result = udp.BeginReceive(null, this);
                result.AsyncWaitHandle.WaitOne(timeout, false);
                if (!result.IsCompleted)
                {
                    throw SharpTimeoutException.Create(_agent, timeout);
                }
                
                bytes = udp.EndReceive(result, ref from);
                udp.Close();
            }
            
            MemoryStream m = new MemoryStream(bytes, false);
            ISnmpMessage message = MessageFactory.ParseMessages(m)[0];
            if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
            {
                throw SharpOperationException.Create("wrong response type", _agent);
            }
            
            GetResponseMessage response = (GetResponseMessage)message;
            if (response.SequenceNumber != SequenceNumber)
            {
                throw SharpOperationException.Create("wrong response sequence", _agent);
            }
            
            ByteTool.Capture(bytes); // log response
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

        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return ByteTool.GetResponse(this, receiver, _bytes, SequenceNumber, timeout);
        }

        /// <summary>
        /// Version.
        /// </summary>
        public VersionCode Version
        {
            get { return _version; }
        }
        
        /// <summary>
        /// Sequence number.
        /// </summary>
        public int SequenceNumber
        {
            get { return _sequenceNumber; }
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get { return _community; }
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
        /// Returns a <see cref="string"/> that represents this <see cref="GetRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}
