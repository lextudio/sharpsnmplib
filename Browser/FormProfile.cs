using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Lextm.SharpSnmpLib.Browser
{
    internal partial class FormProfile : Form
    {
        private AgentProfile _agent;

        public FormProfile(AgentProfile agent)
        {
            InitializeComponent();
            _agent = agent;
        }

        internal IPAddress IP
        {
            get { return IPAddress.Parse(txtIP.Text); }
        }

        internal VersionCode VersionCode
        {
            get { return (VersionCode)cbVersionCode.SelectedIndex; }
        }

        internal string GetCommunity
        {
            get { return txtGet.Text; }
        }

        internal string SetCommunity
        {
            get { return txtSet.Text; }
        }
        
        internal int Port
        {
            get { return int.Parse(txtPort.Text); }
        }

        private void txtSet_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = txtSet.Text.Length != 0;
        }

        private void txtGet_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = txtGet.Text.Length != 0;
        }

        private void txtIP_Validating(object sender, CancelEventArgs e)
        {
            IPAddress ip;
            e.Cancel = AgentProfile.IsValidIPAddress(txtIP.Text, out ip);
        }

        private void FormProfile_Load(object sender, EventArgs e)
        {
            if (_agent != null)
            {
                txtIP.Text = _agent.IP.ToString();
                txtGet.Text = _agent.GetCommunity;
                txtSet.Text = _agent.SetCommunity;
                cbVersionCode.SelectedIndex = (int)_agent.VersionCode;
            }
        }
        
        void TxtPortValidating(object sender, CancelEventArgs e)
        {
            int result;
            e.Cancel = int.TryParse(txtPort.Text, out result);
        }
    }
}
