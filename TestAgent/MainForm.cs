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
using Lextm.SharpSnmpLib.Messaging;

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
            listener1.ExceptionRaised += Agent1ExceptionRaised;
            DefaultListenerAdapter adapter = new DefaultListenerAdapter(listener1);
            listener1.Adapters.Add(adapter);
            adapter.GetRequestReceived += Agent1GetRequestReceived;
            adapter.GetNextRequestReceived += AdapterGetNextRequestReceived;
		    adapter.GetBulkRequestReceived += AdapterGetBulkRequestReceived;
		    adapter.SetRequestReceived += AdapterSetRequestReceived;
            Application.Idle += Application_Idle;
		}

	    private void AdapterSetRequestReceived(object sender, MessageReceivedEventArgs<SetRequestMessage> e)
	    {
            SetRequestMessage message = e.Message;
            // you may validate message version number and/or community name here.
            if (message.Variables.Count != 1)
            {
                return;
            }

            if (message.Variables[0].Id != SysDescr)
            {
                return;
            }

            GetResponseMessage response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, message.Variables);
            listener1.SendResponse(response, e.Sender);
	    }

	    private void AdapterGetBulkRequestReceived(object sender, MessageReceivedEventArgs<GetBulkRequestMessage> e)
	    {
            GetBulkRequestMessage message = e.Message;
            // you may validate message version number and/or community name here.
            if (message.Variables.Count != 1)
            {
                return;
            }

            if (message.Variables[0].Id != SysDescr)
            {
                return;
            }

            List<Variable> list = new List<Variable>();
            list.Add(new Variable(SysDescr, new EndOfMibView()));

            GetResponseMessage response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, list);
            listener1.SendResponse(response, e.Sender);
	    }

	    private void AdapterGetNextRequestReceived(object sender, MessageReceivedEventArgs<GetNextRequestMessage> e)
        {
            GetNextRequestMessage message = e.Message;
            // you may validate message version number and/or community name here.
            if (message.Variables.Count != 1)
            {
                return;
            }

            if (message.Variables[0].Id != SysDescr)
            {
                return;
            }

            List<Variable> list = new List<Variable>();
            list.Add(new Variable(SysDescr, new OctetString("Test Description")));

            GetResponseMessage response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, list);
            listener1.SendResponse(response, e.Sender);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            btnStart.Enabled = !listener1.Active;
            btnStop.Enabled = listener1.Active;
        }

		private void BtnStartClick(object sender, EventArgs e)
		{
			listener1.Start(int.Parse(txtAgentPort.Text));
		}

        private void BtnStopClick(object sender, EventArgs e)
		{
			listener1.Stop();
		}

		private void Agent1GetRequestReceived(object sender, MessageReceivedEventArgs<GetRequestMessage> e)
		{
			GetRequestMessage message = e.Message;
			// you may validate message version number and/or community name here.
			if (message.Variables.Count != 1)
			{
				return;
			}

			if (message.Variables[0].Id != SysDescr)
			{
				return;
			}

			List<Variable> list = new List<Variable>();
			list.Add(new Variable(SysDescr, new OctetString("Test Description")));

			GetResponseMessage response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, list);
			listener1.SendResponse(response, e.Sender);
		}

		private static readonly ObjectIdentifier SysDescr = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");

		private static void Agent1ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
		{
			MessageBox.Show(e.Exception.Message);
		}
		
		private void BtnTrapClick(object sender, EventArgs e)
		{
		    IPAddress ip = IPAddress.Parse(txtIP.Text);
		    if (ip == null)
		    {
		        return;
		    }

		    Messenger.SendTrapV1(new IPEndPoint(ip, int.Parse(txtPort.Text)),
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

			Messenger.SendTrapV2(0, VersionCode.V2,
			                 new IPEndPoint(ip, int.Parse(txtPort.Text)),
			                 new OctetString("public"),
			                 new ObjectIdentifier(new uint[] { 1, 3, 6 }),
			                 0,
			                 new List<Variable>());
		}
		
		private void BtnInformClick(object sender, EventArgs e)
		{
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            if (ip == null)
            {
                return;
            }

			try
			{
				Messenger.SendInform(0, VersionCode.V2, 
				                 new IPEndPoint(ip, int.Parse(txtPort.Text)),
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
