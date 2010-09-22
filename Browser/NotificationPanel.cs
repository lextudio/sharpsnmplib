/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 16:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using Microsoft.Practices.Unity;
using RemObjects.Mono.Helpers;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
    /// <summary>
    /// Description of NotificationPanel.
    /// </summary>
    internal partial class NotificationPanel : DockContent
    {
        private readonly SnmpDemon _demon;
        private const string StrAllUnassigned = "All Unassigned";
        private const string StrSends = "[{1}] [{0}] {2}";

        public NotificationPanel()
        {
            var trapv1 = Program.Container.Resolve<TrapV1MessageHandler>("TrapV1Handler");
            trapv1.MessageReceived += ListenerTrapV1Received;
            var trapv2 = Program.Container.Resolve<TrapV2MessageHandler>("TrapV2Handler");
            trapv2.MessageReceived += ListenerTrapV2Received;
            var inform = Program.Container.Resolve<InformMessageHandler>("InformHandler");
            inform.MessageReceived += ListenerInformRequestReceived;
            _demon = Program.Container.Resolve<SnmpDemon>();
            _demon.Listener.ExceptionRaised += ListenerExceptionRaised;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public IObjectRegistry Objects { get; set; }

        private void ListenerInformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
        {
            LogMessage(string.Format(CultureInfo.InvariantCulture, StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void ListenerTrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
        {
            LogMessage(string.Format(CultureInfo.InvariantCulture, StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void ListenerTrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
        {
            LogMessage(string.Format(CultureInfo.InvariantCulture, StrSends, DateTime.Now, e.Sender, e.Message.ToString(Objects)));
        }

        private void ListenerExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            LogMessage(e.Exception.ToString());
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => LogMessage(message)));
                return;
            }
            
            txtLog.AppendText(message);
            txtLog.AppendText(Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void ActEnabledExecute(object sender, EventArgs e)
        {
            if (_demon.Listener.Active)
            {
                StopListeners();
                return;
            }

            if (Helper.IsRunningOnMono && PlatformSupport.Platform != PlatformType.Windows &&
                Mono.Unix.Native.Syscall.getuid() != 0 && int.Parse(tstxtPort.Text, CultureInfo.InvariantCulture) < 1024)
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
            _demon.Stop();
        }

        private void StartListeners()
        {
            _demon.Listener.ClearBindings();
            int port = int.Parse(tstxtPort.Text, CultureInfo.InvariantCulture);
            if (tscbIP.Text == StrAllUnassigned)
            {
                if (Socket.SupportsIPv4)
                {
                    _demon.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));
                }

                if (Socket.OSSupportsIPv6)
                {
                    _demon.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
                }

                _demon.Start();
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

                _demon.Listener.AddBinding(new IPEndPoint(address, port));
                _demon.Start();
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                LogMessage(Listener.ErrorIPv6NotSupported);
                return;
            }

            _demon.Listener.AddBinding(new IPEndPoint(address, port));
            _demon.Start();
        }

        private void ActEnabledAfterExecute(object sender, EventArgs e)
        {
            actEnabled.Text = _demon.Listener.Active ? @"Stop Listening" : @"Start Listening";
            actEnabled.Image = _demon.Listener.Active
                                   ? Properties.Resources.media_playback_stop
                                   : Properties.Resources.media_playback_start;
            tscbIP.Enabled = !_demon.Listener.Active;
            tstxtPort.Enabled = !_demon.Listener.Active;
        }
    }
}
