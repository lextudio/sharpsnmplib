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
using Microsoft.Practices.Unity;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly SnmpDemon _demon;
        private const string StrAllUnassigned = "All Unassigned";

        public MainForm()
        {
            _demon = Program.Container.Resolve<SnmpDemon>();
            InitializeComponent();
			if (!Program.IsRunningOnMono())
			{
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
				Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			}
			
            tstxtPort.Text = "161";
            tscbIP.Items.Add(StrAllUnassigned);
            foreach (IPAddress address in Dns.GetHostEntry(string.Empty).AddressList.Where(address => !address.IsIPv6LinkLocal))
            {
                tscbIP.Items.Add(address);
            }

            tscbIP.SelectedIndex = 0;
            Text = Text + " (Version: " + Assembly.GetExecutingAssembly().GetName().Version + ")";
        }

        private void StartListeners()
        {
            _demon.Listener.ClearBindings();
            int port = int.Parse(tstxtPort.Text);
            if (tscbIP.Text == StrAllUnassigned)
            {
                _demon.Listener.AddBinding(new IPEndPoint(IPAddress.Any, port));
                if (Socket.OSSupportsIPv6)
                {
                    _demon.Listener.AddBinding(new IPEndPoint(IPAddress.IPv6Any, port));
                }

                _demon.Start();
                return;
            }

            IPAddress address = IPAddress.Parse(tscbIP.Text);
            if (address == null)
            {
                return;
            }

            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                _demon.Listener.AddBinding(new IPEndPoint(address, port));
                _demon.Start();
                return;
            }

            if (!Socket.OSSupportsIPv6)
            {
                MessageBox.Show(Listener.ErrorIPv6NotSupported);
                return;
            }

            _demon.Listener.AddBinding(new IPEndPoint(address, port));
            _demon.Start();
        }

        private void StopListeners()
        {
            _demon.Stop();
        }

        private void BtnTrapClick(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            if (ip == null)
            {
                return;
            }

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
            if (ip == null)
            {
                return;
            }

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
        private void BtnInformClick(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            if (ip == null)
            {
                return;
            }

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
                    2000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void alNotification_Update(object sender, EventArgs e)
        {
            tscbIP.Enabled = !actEnabled.Checked;
            tstxtPort.Enabled = !actEnabled.Checked;
        }
    }
}
