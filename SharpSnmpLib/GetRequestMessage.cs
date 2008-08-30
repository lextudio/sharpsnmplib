namespace Lextm.SharpSnmpLib
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// GET request message.
    /// </summary>
    public class GetRequestMessage : ISnmpMessage
    {
        private VersionCode _version;
        private IList<Variable> _variables;
        private byte[] _bytes;
        private IPAddress _agent;
        private string _community;
        private ISnmpPdu _pdu;
        private int _sequenceNumber;
        
        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public GetRequestMessage(VersionCode version, IPAddress agent, string community, IList<Variable> variables)
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
        /// Broadcasts request for new agents.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        public IDictionary<IPEndPoint, Variable> Broadcast(int timeout, int port)
        {
            byte[] bytes = _bytes;
            IPEndPoint agent = new IPEndPoint(_agent, port);
            IDictionary<IPEndPoint, Variable> result;
            using (UdpClient udp = new UdpClient())
            {
                udp.EnableBroadcast = true;
                udp.Send(bytes, bytes.Length, agent);
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
                    Socket _watcher = ((UdpClient)e.Argument).Client;
                    IPEndPoint source = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint senderRemote = (EndPoint)source;
                    byte[] msg = new byte[_watcher.ReceiveBufferSize];
                    while (!((BackgroundWorker)sender).CancellationPending)
                    {
                        int number = _watcher.Available;
                        Thread.Sleep(100);
                        if (number == 0)
                        {
                            continue;
                        }
                        
                        _watcher.ReceiveFrom(msg, ref senderRemote);
                        ISnmpMessage message = MessageFactory.ParseMessages(msg)[0];
                        if (message.TypeCode != SnmpType.GetResponsePdu)
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
                        
                        result.Add((IPEndPoint)senderRemote, ((GetResponseMessage)message).Variables[0]);
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
        
        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        public IList<Variable> Send(int timeout, int port)
        {
            byte[] bytes = _bytes;
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
                return SnmpType.GetRequestPdu;
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}
