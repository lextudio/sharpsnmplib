/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Trap listener component.
    /// </summary>
    /// <remarks>
    /// <para>Drag this component into your form in designer, or create an instance in code.</para>
    /// <para>Use <see cref="Manager"></see> component if you need to do all SNMP operations.</para>
    /// <para>This component is for TRAP operation only.</para>
    /// <para>Currently only SNMP v1 TRAP is supported.</para>
    /// </remarks>
    public class TrapListener : Component
    {
        private Socket _watcher;
        private IPEndPoint _sender;
        private int _port = DEFAULTPORT;
        private BackgroundWorker worker;
        private const int DEFAULTPORT = 162;
       
        /// <summary>
        /// Creates a <see cref="TrapListener" /> instance.
        /// </summary>
        public TrapListener()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Occurs when a <see cref="TrapV1Message" /> is received.
        /// </summary>
        public event EventHandler<TrapV1ReceivedEventArgs> TrapV1Received;
      
        /// <summary>
        /// Occurs when a <see cref="TrapV2Message"/> is received.
        /// </summary>
        public event EventHandler<TrapV2ReceivedEventArgs> TrapV2Received;
      
        /// <summary>
        /// Port number.
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            
            set
            {
                _port = value;
            }
        }
        
        /// <summary>
        /// Starts.
        /// </summary>
        public void Start()
        {
            Start(DEFAULTPORT);
        }
        
        /// <summary>
        /// Starts on a specific port.
        /// </summary>
        /// <param name="port">Port number</param>
        public void Start(int port)
        {
            if (worker.IsBusy) 
            {
                return;
            }
            
            _port = port;
            _sender = new IPEndPoint(IPAddress.Any, _port);
            _watcher = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                _watcher.Bind(_sender);
                worker.RunWorkerAsync();
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10048)
                {
                    throw new SharpSnmpException("Port is already used: " + _port, ex);
                }
            }
        }
        
        /// <summary>
        /// Stops.
        /// </summary>
        public void Stop()
        {
            if (_watcher == null)
            {
                return;
            }
            
            _watcher.Close();
            if (worker.IsBusy) 
            {
                worker.CancelAsync();
            }
        }

        private void InitializeComponent()
        {
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_DoWork);
            this.worker.ProgressChanged += new ProgressChangedEventHandler(TrapListener_ProgressChanged);
        }

        private void TrapListener_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != -1)
            {
                return;
            }
            
            throw new Exception("thread exception", (Exception)e.UserState);
        }
        
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IPEndPoint agent = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderRemote = (EndPoint)agent;
            byte[] msg = new byte[_watcher.ReceiveBufferSize];
            while (!((BackgroundWorker)sender).CancellationPending)
            {
                int number = _watcher.Available;
                if (number != 0)
                {
                    _watcher.ReceiveFrom(msg, ref senderRemote);
                    try 
                    {
                        HandleMessage(msg, number, (IPEndPoint)senderRemote);
                    } 
                    catch (Exception ex) 
                    {
                        worker.ReportProgress(-1, ex);
                    }
                }
            }
        }
        
        private void HandleMessage(byte[] buffer, int number, IPEndPoint agent)
        {
            ISnmpMessage message = MessageFactory.ParseMessage(buffer, 0, number);
            switch (message.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    {
                        if (TrapV1Received != null)
                        {
                            TrapV1Received(this, new TrapV1ReceivedEventArgs(agent, (TrapV1Message)message));
                        }
                        
                        break;
                    }
                    
                case SnmpType.TrapV2Pdu:
                    {
                        if (TrapV2Received != null)
                        {
                            TrapV2Received(this, new TrapV2ReceivedEventArgs(agent, (TrapV2Message)message));
                        }
                        
                        break;
                    }
                    
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents a <see cref="TrapListener"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trap listener: port: " + _port;
        }
    }
}
