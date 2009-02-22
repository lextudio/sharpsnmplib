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
using System.Net;
using System.Windows.Forms;

using Lextm.SharpSnmpLib;

namespace TestAgent
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
            Application.Idle += Application_Idle;
		}

        void Application_Idle(object sender, EventArgs e)
        {
            btnStart.Enabled = !listener1.Active;
            btnStop.Enabled = listener1.Active;
        }

		private void btnStart_Click(object sender, EventArgs e)
		{
			listener1.Start(int.Parse(txtAgentPort.Text));
		}

        private void btnStop_Click(object sender, EventArgs e)
		{
			listener1.Stop();
		}

		private void agent1_GetRequestReceived(object sender, Lextm.SharpSnmpLib.MessageReceivedEventArgs<GetRequestMessage> e)
		{
			GetRequestMessage message = e.Message;
			// you may validate message version number and/or community name here.
			if (message.Variables.Count != 1)
			{
				return;
			}
			if (message.Variables[0].Id != sysDescr)
			{
				return;
			}

			List<Variable> list = new List<Variable>();
			list.Add(new Variable(sysDescr, new OctetString("Test Description")));

			GetResponseMessage response = new GetResponseMessage(message.SequenceNumber, message.Version, e.Sender.Address, message.Community, list);
			response.Send(e.Sender.Port);
		}

		private static readonly ObjectIdentifier sysDescr = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");

		private void agent1_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
		{
			MessageBox.Show(e.Exception.Message);
		}
		
		private void BtnTrapClick(object sender, EventArgs e)
		{
			Agent.SendTrapV1(new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text)),
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
			Agent.SendTrapV2(0, VersionCode.V2,
			                 new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text)),
			                 new OctetString("public"),
			                 new ObjectIdentifier(new uint[] { 1, 3, 6 }),
			                 0,
			                 new List<Variable>());
		}
		
		private void BtnInformClick(object sender, EventArgs e)
		{
			try
			{
				Agent.SendInform(0, VersionCode.V2, 
				                 new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text)),
				                 new OctetString("public"),
				                 new ObjectIdentifier(new uint[] { 1, 3, 6 }),
				                 0,
				                 new List<Variable>(), 2000);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
