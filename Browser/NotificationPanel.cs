/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 16:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
    /// <summary>
    /// Description of NotificationPanel.
    /// </summary>
    internal partial class NotificationPanel : DockContent
    {
        private const string STR_AllUnassigned = "All Unassigned";
        private const string STR_Sends = "[{1}] [{0}] {2}";
        private Listener _listener;
        private Listener _listenerV6;
        
        public NotificationPanel()
        {
            InitializeComponent();
            tstxtPort.Text = "162";
            tscbIP.Items.Add(STR_AllUnassigned);
            foreach (IPAddress address in Dns.GetHostEntry("").AddressList)
            {
                if (address.IsIPv6LinkLocal)
                {
                    continue;
                }

                tscbIP.Items.Add(address);
            }

            tscbIP.SelectedIndex = 0;
        }
        
        [Dependency]
        public Listener Listener
        {
            get { return _listener; }
            set { _listener = value; }
        }
        
        [Dependency]
        public Listener ListenerV6
        {
            get { return _listenerV6; }
            set { _listenerV6 = value; }
        }
        
        private void NotificationPanel_Load(object sender, EventArgs e)
        {
            Listener.ExceptionRaised += Listener_ExceptionRaised;
            Listener.TrapV1Received += new EventHandler<MessageReceivedEventArgs<TrapV1Message>>(Listener_TrapV1Received);;
            Listener.TrapV2Received += new EventHandler<MessageReceivedEventArgs<TrapV2Message>>(Listener_TrapV2Received);
            Listener.InformRequestReceived += new EventHandler<MessageReceivedEventArgs<InformRequestMessage>>(Listener_InformRequestReceived);
            
            ListenerV6.ExceptionRaised += Listener_ExceptionRaised;
            ListenerV6.TrapV1Received += new EventHandler<MessageReceivedEventArgs<TrapV1Message>>(Listener_TrapV1Received);;
            ListenerV6.TrapV2Received += new EventHandler<MessageReceivedEventArgs<TrapV2Message>>(Listener_TrapV2Received);
            ListenerV6.InformRequestReceived += new EventHandler<MessageReceivedEventArgs<InformRequestMessage>>(Listener_InformRequestReceived);
        }

        private void Listener_InformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
        {
            LogMessage(string.Format(STR_Sends, DateTime.Now, e.Sender, e.Message.ToString()));
        }

        private void Listener_TrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
        {
            LogMessage(string.Format(STR_Sends, DateTime.Now, e.Sender, e.Message.ToString()));
        }

        private void Listener_TrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
        {
            LogMessage(string.Format(STR_Sends, DateTime.Now, e.Sender, e.Message.ToString()));
        }

        private void Listener_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            LogMessage(e.Exception.ToString());
        }

        public void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { LogMessage(message); });
                return;
            }
            
            txtLog.AppendText(message);
            txtLog.AppendText(Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        private void actEnabled_Execute(object sender, EventArgs e)
        {
            if (actEnabled.Checked)
            {
                StartListeners();
                actEnabled.Text = "Enabled";
            }
            else
            {
                StopListeners();
                actEnabled.Text = "Disabled";
            }
        }

        private void StopListeners()
        {
            Listener.Stop();
            ListenerV6.Stop();
        }

        private void StartListeners()
        {
            int port = int.Parse(tstxtPort.Text);
            if (tscbIP.Text == STR_AllUnassigned)
            {
                Listener.Start(new IPEndPoint(IPAddress.Any, port));
                if (Socket.OSSupportsIPv6)
                {
                    ListenerV6.Start(new IPEndPoint(IPAddress.IPv6Any, port));
                }
                 
                return;
            }

            IPAddress address = IPAddress.Parse(tscbIP.Text);
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                Listener.Start(new IPEndPoint(address, port)); 
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                LogMessage(Listener.STR_CannotUseIPV6AsTheOSDoesNotSupportIt);
                return;
            }
                
            ListenerV6.Start(new IPEndPoint(address, port));          
        }

        private void actEnabled_Update(object sender, EventArgs e)
        {
            tscbIP.Enabled = !actEnabled.Checked;
            tstxtPort.Enabled = !actEnabled.Checked;
        }
    }
}
