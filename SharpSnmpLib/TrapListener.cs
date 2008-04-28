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

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class TrapListener: Component
	{
		public TrapListener()
		{
			InitializeComponent();
		}
		
		Socket _watcher;
		IPEndPoint _sender;
        int _port = DEFAULTPORT;
        private BackgroundWorker worker;
        const int DEFAULTPORT = 162;

        public event EventHandler<TrapReceivedEventArgs> TrapReceived;

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

        public void Start()
        {
            Start(DEFAULTPORT);
        }
		
		public void Start(int port)
		{
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
		
		public void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			EndPoint senderRemote = (EndPoint)_sender;	
			byte[] msg = new Byte[_watcher.ReceiveBufferSize];				
			Console.WriteLine("Waiting to receive datagrams from client...");
			while (!((BackgroundWorker)sender).CancellationPending)
			{				
				int number = _watcher.Available;
				if (number != 0) 
				{
					Console.WriteLine("receive data..." + number);
					// This call blocks.
					_watcher.ReceiveFrom(msg, ref senderRemote);
                    if (TrapReceived != null)
                    {
                        TrapReceived(this, new TrapReceivedEventArgs(new TrapMessage(msg, number)));
                    }
                    DebugHelper(msg, number);
				}
			}
		}

        [Conditional("DEBUG")]
        private static void DebugHelper(byte[] msg, int number)
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite("fivevarbinds.dat")))
            {
                writer.BaseStream.Write(msg, 0, number);
                writer.Close();
            }
            for (int i = 0; i < number; i++)
            {
                Console.Write((char)msg[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < number; i++)
            {
                Console.Write(msg[i].ToString("X2") + string.Format("({0}) ", i));
            }
            Console.WriteLine();
            for (int i = 0; i < number; i++)
            {
                Console.Write(msg[i].ToString("D") + string.Format("({0}) ", i));
            }
            Console.WriteLine();
        }
		
		public void Stop()
		{
			if (_watcher == null) {
				return;
			}
			_watcher.Close();
		}

        private void InitializeComponent()
        {
            this.worker = new System.ComponentModel.BackgroundWorker();
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);

        }
	}
}
