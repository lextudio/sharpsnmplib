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
using System.Drawing;
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
            UpdateButtons(false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            agent1.Monitor.Start(161);
            UpdateButtons(true);
        }

        private void UpdateButtons(bool started)
        {
            btnStart.Enabled = !started;
            btnStop.Enabled = started;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            agent1.Monitor.Stop();
            UpdateButtons(false);
        }

        private void agent1_GetRequestReceived(object sender, Lextm.SharpSnmpLib.GetRequestReceivedEventArgs e)
        {
            GetRequestMessage message = e.Request;
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

        private static ObjectIdentifier sysDescr = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");

        private void agent1_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}
