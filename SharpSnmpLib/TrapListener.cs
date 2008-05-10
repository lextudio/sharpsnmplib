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
	public class TrapListener: Component
	{
		Socket _watcher;
		IPEndPoint _sender;
        int _port = DEFAULTPORT;
        private BackgroundWorker worker;
        const int DEFAULTPORT = 162;
		/// <summary>
		/// Creates a <see cref="TrapListener" /> instance.
		/// </summary>
		public TrapListener()
		{
			InitializeComponent();
		}
    	/// <summary>
    	/// Occurs when a <see cref="TrapMessage" /> is received.
    	/// </summary>
        public event EventHandler<TrapReceivedEventArgs> TrapReceived;
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
			if (worker.IsBusy) {
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
			if (_watcher == null) {
				return;
			}
			_watcher.Close();
			if (worker.IsBusy) {
				worker.CancelAsync();
			}
		}

        private void InitializeComponent()
        {
            this.worker = new System.ComponentModel.BackgroundWorker();
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_DoWork);
        }
        
        void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			EndPoint senderRemote = (EndPoint)_sender;	
			byte[] msg = new Byte[_watcher.ReceiveBufferSize];				
			//Console.WriteLine("Waiting to receive datagrams from client...");
			while (!((BackgroundWorker)sender).CancellationPending)
			{		
				int number = _watcher.Available;
				if (number != 0) 
				{
					//Console.WriteLine("receive data..." + number);
					// This call blocks.
					_watcher.ReceiveFrom(msg, ref senderRemote);
					ISnmpMessage message = MessageFactory.ParseMessage(msg, 0, number);
                    if (TrapReceived != null 
					    && message.TypeCode == SnmpType.TrapPDUv1)
                    {
						TrapReceived(this, new TrapReceivedEventArgs((TrapMessage)message));
                    }
				}
			}
		}
	}
}
