using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

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

        private void FormProfile_Load(object sender, EventArgs e)
        {
            if (_agent != null)
            {
                txtIP.Text = _agent.IP.ToString();
                txtGet.Text = _agent.GetCommunity;
                txtSet.Text = _agent.SetCommunity;
                cbVersionCode.SelectedIndex = (int)_agent.VersionCode;
            }
            else
            {
                cbVersionCode.SelectedIndex = 0;
            }
        }
        
        private void txtPort_Validating(object sender, CancelEventArgs e)
        {
            int result;
            if (!int.TryParse(txtPort.Text, out result))
            {
                e.Cancel = true;
                txtPort.SelectAll();
                errorProvider1.SetError(txtPort, "Please provide a valid port number");
            }
        }
        
        private void txtPort_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtPort, string.Empty);
        }
        
        private void txtSet_Validating(object sender, CancelEventArgs e)
        {
            if (txtSet.Text.Length == 0)
            {
                e.Cancel = true;
                txtSet.SelectAll();
                errorProvider1.SetError(txtSet, "Community name cannot be empty");
            }
        }
        
        private void txtSet_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtSet, string.Empty);
        }

        private void txtGet_Validating(object sender, CancelEventArgs e)
        {
            if (txtGet.Text.Length ==0)
            {
                e.Cancel = true;
                txtGet.SelectAll();
                errorProvider1.SetError(txtGet, "Community name cannot be empty");
            }
        }
        
        private void txtGet_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtGet, string.Empty);
        }

        private void txtIP_Validating(object sender, CancelEventArgs e)
        {
            IPAddress ip;
            if (!AgentProfile.IsValidIPAddress(txtIP.Text, out ip))
            {
                e.Cancel = true;
                txtIP.SelectAll();
                errorProvider1.SetError(txtIP, "IP address is not valid");
            }
        }
        
        private void txtIP_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtIP, string.Empty);
        }
        
        private void btnOK_Validating(object sender, CancelEventArgs e)
        {
            txtGet_Validating(txtGet, e);
            if (e.Cancel)
            {
                return;
            }
            txtSet_Validating(txtSet, e);
            if (e.Cancel)
            {
                return;
            }
            txtPort_Validating(txtPort, e);
            if (e.Cancel)
            {
                return;
            }
            txtIP_Validating(txtIP, e);
            if (e.Cancel)
            {
                return;
            }
        }
    }
}
