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

        internal string IP
        {
            get { return null; }
        }

        internal VersionCode VersionCode
        {
            get { return VersionCode.V1; }
        }

        internal string GetCommunity
        {
            get { return null; }
        }

        internal string SetCommunity
        {
            get { return null; }
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
                txtIP.Text = _agent.IP;
                txtGet.Text = _agent.GetCommunity;
                txtSet.Text = _agent.SetCommunity;
                cbVersionCode.SelectedIndex = (int)_agent.VersionCode;
            }
        }
    }
}
