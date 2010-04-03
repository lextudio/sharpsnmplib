/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 16:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Mib;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
    /// <summary>
    /// Description of NotificationPanel.
    /// </summary>
    internal partial class NotificationPanel : DockContent
    {
        private const string StrAllUnassigned = "All Unassigned";
        private const string StrSends = "[{1}] [{0}] {2}";

        public NotificationPanel()
        {
            InitializeComponent();
            tstxtPort.Text = "162";
            tscbIP.Items.Add(StrAllUnassigned);
            foreach (IPAddress address in Dns.GetHostEntry("").AddressList.Where(address => !address.IsIPv6LinkLocal))
            {
                tscbIP.Items.Add(address);
            }

            tscbIP.SelectedIndex = 0;
        }

        public Listener Listener { get; set; }

        public Listener ListenerV6 { get; set; }

        public IObjectRegistry Objects { get; set; }

        private void NotificationPanel_Load(object sender, EventArgs e)
        {
            Listener.ExceptionRaised += Listener_ExceptionRaised;
            DefaultListenerAdapter adapter = new DefaultListenerAdapter(Listener);
            Listener.Adapters.Add(adapter);
            adapter.TrapV1Received += Listener_TrapV1Received;
            adapter.TrapV2Received += Listener_TrapV2Received;
            adapter.InformRequestReceived += Listener_InformRequestReceived;
            
            ListenerV6.ExceptionRaised += Listener_ExceptionRaised;
            ListenerV6.Adapters.Add(adapter);
        }

        private void Listener_InformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
        {
            LogMessage(string.Format(StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void Listener_TrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
        {
            LogMessage(string.Format(StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void Listener_TrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
        {
            LogMessage(string.Format(StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void Listener_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            LogMessage(e.Exception.ToString());
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() => LogMessage(message)));
                return;
            }
            
            txtLog.AppendText(message);
            txtLog.AppendText(Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        private void ActEnabledExecute(object sender, EventArgs e)
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
            if (tscbIP.Text == StrAllUnassigned)
            {
                Listener.Start(new IPEndPoint(IPAddress.Any, port));
                if (Socket.OSSupportsIPv6)
                {
                    ListenerV6.Start(new IPEndPoint(IPAddress.IPv6Any, port));
                }
                 
                return;
            }

            IPAddress address = IPAddress.Parse(tscbIP.Text);
            if (address == null)
            {
                return;
            }

            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                Listener.Start(new IPEndPoint(address, port)); 
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                LogMessage(Listener.ErrorIPv6NotSupported);
                return;
            }
                
            ListenerV6.Start(new IPEndPoint(address, port));          
        }

        private void ActEnabledUpdate(object sender, EventArgs e)
        {
            tscbIP.Enabled = !actEnabled.Checked;
            tstxtPort.Enabled = !actEnabled.Checked;
        }
    }
}
