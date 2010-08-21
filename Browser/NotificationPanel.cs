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
using Lextm.SharpSnmpLib.Pipeline;
using RemObjects.Mono.Helpers;
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
            if (PlatformSupport.Platform == PlatformType.Windows)
            {
                Icon = Properties.Resources.dialog_information;
                actEnabled.Image = Properties.Resources.media_playback_start;
            }
            
            tstxtPort.Text = @"162";
            tscbIP.Items.Add(StrAllUnassigned);
            foreach (IPAddress address in Dns.GetHostEntry(string.Empty).AddressList.Where(address => !address.IsIPv6LinkLocal))
            {
                tscbIP.Items.Add(address);
            }

            tscbIP.SelectedIndex = 0;
        }

        public SnmpDemon Demon { get; set; }

        public IObjectRegistry Objects { get; set; }

        private void ListenerInformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
        {
            LogMessage(string.Format(StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void ListenerTrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
        {
            LogMessage(string.Format(StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void ListenerTrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
        {
            LogMessage(string.Format(StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void ListenerExceptionRaised(object sender, ExceptionRaisedEventArgs e)
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
            if (Demon.Listener.Active)
            {
                StopListeners();
                return;
            }

            if (Helper.IsRunningOnMono() && PlatformSupport.Platform != PlatformType.Windows &&
                Mono.Unix.Native.Syscall.getuid() != 0 && int.Parse(tstxtPort.Text) < 1024)
            {
                MessageBox.Show(@"On Linux this application must be run as root for port < 1024.");
                return;
            }

            try
            {
                StartListeners();
            }
            catch (PortInUseException ex)
            {
                MessageBox.Show(@"Port is already in use: " + ex.Endpoint, @"Error");
            }
        }

        private void StopListeners()
        {
            Demon.Listener.Stop();
        }

        private void StartListeners()
        {
            Demon.Listener.ClearBindings();
            int port = int.Parse(tstxtPort.Text);
            if (tscbIP.Text == StrAllUnassigned)
            {
                if (Socket.SupportsIPv4)
                {
                    Demon.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));
                }

                if (Socket.OSSupportsIPv6)
                {
                    Demon.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
                }

                Demon.Listener.Start();
                return;
            }

            IPAddress address = IPAddress.Parse(tscbIP.Text);
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                if (!Socket.SupportsIPv4)
                {
                    LogMessage(Listener.ErrorIPv4NotSupported);
                    return;
                }

                Demon.Listener.AddBinding(new IPEndPoint(address, port));
                Demon.Listener.Start();
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                LogMessage(Listener.ErrorIPv6NotSupported);
                return;
            }

            Demon.Listener.AddBinding(new IPEndPoint(address, port));
            Demon.Listener.Start();
        }

        private void ActEnabledAfterExecute(object sender, EventArgs e)
        {
            actEnabled.Text = Demon.Listener.Active ? @"Stop Listening" : @"Start Listening";
            actEnabled.Image = Demon.Listener.Active
                                   ? Properties.Resources.media_playback_stop
                                   : Properties.Resources.media_playback_start;
            tscbIP.Enabled = !Demon.Listener.Active;
            tstxtPort.Enabled = !Demon.Listener.Active;
        }
    }
}
