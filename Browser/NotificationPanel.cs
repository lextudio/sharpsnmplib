/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 16:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;
using System.Net;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of NotificationPanel.
	/// </summary>
	internal partial class NotificationPanel : DockContent
	{
        private const string STR_AllUnassigned = "All Unassigned";
        private const string STR_Sends = "{0} sends {1}";
		private Listener _listener;
		
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
		
		private void NotificationPanel_Load(object sender, EventArgs e)
		{
            Listener.ExceptionRaised += Listener_ExceptionRaised;
			Listener.TrapV1Received += new EventHandler<MessageReceivedEventArgs<TrapV1Message>>(Listener_TrapV1Received);;
			Listener.TrapV2Received += new EventHandler<MessageReceivedEventArgs<TrapV2Message>>(Listener_TrapV2Received);
			Listener.InformRequestReceived += new EventHandler<MessageReceivedEventArgs<InformRequestMessage>>(Listener_InformRequestReceived);
		}

		private void Listener_InformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
		{
		    LogMessage(string.Format(STR_Sends, e.Sender, e.Message.ToString()));
		}

		private void Listener_TrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
		{
			LogMessage(string.Format(STR_Sends, e.Sender, e.Message.ToString()));
		}

		private void Listener_TrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
		{
			LogMessage(string.Format(STR_Sends, e.Sender, e.Message.ToString()));
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
                Listener.Start(GetIPEndPoint());
                actEnabled.Text = "Enabled";
            }
            else
            {
                Listener.Stop();
                actEnabled.Text = "Disabled";
            }
        }

        private IPEndPoint GetIPEndPoint()
        {
            if (tscbIP.Text == STR_AllUnassigned)
            {
                return new IPEndPoint(IPAddress.Any, int.Parse(tstxtPort.Text));
            }

            return new IPEndPoint(IPAddress.Parse(tscbIP.Text), int.Parse(tstxtPort.Text));
        }

        private void actEnabled_Update(object sender, EventArgs e)
        {
            tscbIP.Enabled = !actEnabled.Checked;
            tstxtPort.Enabled = !actEnabled.Checked;
        }	    
	}
}
