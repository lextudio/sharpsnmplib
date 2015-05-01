/*
 * Created by SharpDevelop.
 * User: lexli
 * Date: 2008-12-14
 * Time: 14:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using RemObjects.Mono.Helpers;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly SnmpEngine _engine;
        private const string StrAllUnassigned = "All Unassigned";

        public MainForm()
        {
            // TODO: this is a hack. review it later.
            var store = new ObjectStore();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());
            store.Add(new IfNumber());
            store.Add(new IfTable());

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));

            var getv1 = new GetV1MessageHandler();
            var getv1Mapping = new HandlerMapping("v1", "GET", getv1);

            var getv23 = new GetMessageHandler();
            var getv23Mapping = new HandlerMapping("v2,v3", "GET", getv23);

            var setv1 = new SetV1MessageHandler();
            var setv1Mapping = new HandlerMapping("v1", "SET", setv1);

            var setv23 = new SetMessageHandler();
            var setv23Mapping = new HandlerMapping("v2,v3", "SET", setv23);
            
            var getnextv1 = new GetNextV1MessageHandler();
            var getnextv1Mapping = new HandlerMapping("v1", "GETNEXT", getnextv1);

            var getnextv23 = new GetNextMessageHandler();
            var getnextv23Mapping = new HandlerMapping("v2,v3", "GETNEXT", getnextv23);

            var getbulk = new GetBulkMessageHandler();
            var getbulkMapping = new HandlerMapping("v2,v3", "GETBULK", getbulk);
            
            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1, v2, v3 });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                getv1Mapping, 
                getv23Mapping, 
                setv1Mapping,
                setv23Mapping,
                getnextv1Mapping,
                getnextv23Mapping,
                getbulkMapping
            });

            var pipelineFactory = new SnmpApplicationFactory(new RollingLogger(), store, membership, handlerFactory);
            _engine = new SnmpEngine(pipelineFactory, new Listener { Users = users }, new EngineGroup());
            _engine.ExceptionRaised += (sender, e) => MessageBox.Show(e.Exception.ToString());

            InitializeComponent();
            if (PlatformSupport.Platform == PlatformType.Windows)
            {
                // FIXME: work around a Mono WinForms issue.
                Icon = Properties.Resources.network_server;                
            }
            
            actEnabled.Image = Properties.Resources.media_playback_start;
            tstxtPort.Text = @"161";
            tscbIP.Items.Add(StrAllUnassigned);
            foreach (IPAddress address in Dns.GetHostEntry(string.Empty).AddressList.Where(address => !address.IsIPv6LinkLocal))
            {
                tscbIP.Items.Add(address);
            }

            tscbIP.SelectedIndex = 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void StartListeners()
        {
            _engine.Listener.ClearBindings();
            int port = int.Parse(tstxtPort.Text, CultureInfo.InvariantCulture);
            if (tscbIP.Text == StrAllUnassigned)
            {
#if !__MonoCS__
                if (Socket.OSSupportsIPv4)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));
                }
#endif
                if (Socket.OSSupportsIPv6)
                {
                    _engine.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
                }

                _engine.Start();
                return;
            }

            IPAddress address = IPAddress.Parse(tscbIP.Text);
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
#if !__MonoCS__
                if (!Socket.OSSupportsIPv4)
                {
                    MessageBox.Show(Listener.ErrorIPv4NotSupported);
                    return;
                }
#endif
                _engine.Listener.AddBinding(new IPEndPoint(address, port));
                _engine.Start();
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                MessageBox.Show(Listener.ErrorIPv6NotSupported);
                return;
            }

            _engine.Listener.AddBinding(new IPEndPoint(address, port));
            _engine.Start();
        }

        private void StopListeners()
        {
            _engine.Stop();
        }

        private void BtnTrapClick(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            Messenger.SendTrapV1(
                new IPEndPoint(ip, int.Parse(txtPort.Text, CultureInfo.InvariantCulture)),
                IPAddress.Loopback, // here should be IP of the current machine.
                new OctetString("public"),
                new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                GenericCode.ColdStart,
                0,
                0,
                new List<Variable>());
        }

        private void BtnTrap2Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            Messenger.SendTrapV2(
                0,
                VersionCode.V2,
                new IPEndPoint(ip, int.Parse(txtPort.Text, CultureInfo.InvariantCulture)),
                new OctetString("public"),
                new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                0,
                new List<Variable>());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void BtnInformV2Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            try
            {
                Messenger.SendInform(
                    0,
                    VersionCode.V2,
                    new IPEndPoint(ip, int.Parse(txtPort.Text, CultureInfo.InvariantCulture)),
                    new OctetString("public"),
                    new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                    0,
                    new List<Variable>(), 
                    2000,
                    null,
                    null);
            }
            catch (SnmpException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void BtnInformV3Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            try
            {
                IPEndPoint receiver = new IPEndPoint(ip, int.Parse(txtPort.Text, CultureInfo.InvariantCulture));
                Discovery discovery = Messenger.GetNextDiscovery(SnmpType.InformRequestPdu);
                ReportMessage report = discovery.GetResponse(2000, receiver);

                Messenger.SendInform(
                    0,
                    VersionCode.V3,
                    receiver,
                    new OctetString("neither"),
                    new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                    0,
                    new List<Variable>(),
                    2000, 
                    DefaultPrivacyProvider.DefaultPair,
                    report);
            }
            catch (SnmpException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void ActEnabledExecute(object sender, EventArgs e)
        {
            if (_engine.Active)
            {
                StopListeners();
                return;
            }

            if (SnmpMessageExtension.IsRunningOnMono && PlatformSupport.Platform != PlatformType.Windows &&
                Mono.Unix.Native.Syscall.getuid() != 0 && int.Parse(txtPort.Text, CultureInfo.InvariantCulture) < 1024)
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

        private void MainFormLoad(object sender, EventArgs e)
        {
            Text = string.Format(CultureInfo.CurrentUICulture, "{0} (Version: {1})", Text, Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void ActEnabledAfterExecute(object sender, EventArgs e)
        {
            actEnabled.Text = _engine.Listener.Active ? @"Stop Listening" : @"Start Listening";
            actEnabled.Image = _engine.Listener.Active
                                   ? Properties.Resources.media_playback_stop
                                   : Properties.Resources.media_playback_start;
            tscbIP.Enabled = !_engine.Listener.Active;
            tstxtPort.Enabled = !_engine.Listener.Active;
        }
    }
}
