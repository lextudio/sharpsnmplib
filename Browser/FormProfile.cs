using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
    internal partial class FormProfile : Form
    {
        private readonly AgentProfile _profile;

        public FormProfile(AgentProfile agent)
        {
            InitializeComponent();
            _profile = agent;
        }

        internal IPAddress IP
        {
            get { return IPAddress.Parse(txtIP.Text); }
        }

        internal VersionCode VersionCode
        {
            get { return (VersionCode)cbVersionCode.SelectedIndex; }
        }

        internal string AgentName
        {
            get { return txtName.Text; }
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
            get { return int.Parse(txtPort.Text, CultureInfo.CurrentCulture); }
        }

        public string AuthenticationPassphrase
        {
            get { return txtAuthentication.Text; }
        }

        public string PrivacyPassphrase
        {
            get { return txtPrivacy.Text; }
        }

        public string AuthenticationMethod
        {
            get
            {
                return cbAuthentication.SelectedIndex == 0 ? "MD5" : "SHA1";
            }
        }

        public string PrivacyMethod
        {
            get { return "DES"; }
        }

        public string UserName
        {
            get { return txtUserName.Text; }
        }

        private void FormProfile_Load(object sender, EventArgs e)
        {
            if (_profile == null)
            {
                cbVersionCode.SelectedIndex = 1;
                cbAuthentication.SelectedIndex = 0;
                cbPrivacy.SelectedIndex = 0;
                return;
            }
            
            txtIP.Text = _profile.Agent.Address.ToString();
            txtIP.ReadOnly = true;
            txtGet.Text = _profile.GetCommunity;
            txtSet.Text = _profile.SetCommunity;
            txtName.Text = _profile.Name;
            cbVersionCode.SelectedIndex = (int)_profile.VersionCode;
            // TODO: load and save profile v3.
            txtAuthentication.Text = _profile.AuthenticationPassphrase;
            txtPrivacy.Text = _profile.PrivacyPassphrase;
            cbAuthentication.SelectedIndex = (_profile.AuthenticationMethod == "MD5") ? 0 : 1;
            cbPrivacy.SelectedIndex = 0;
            txtUserName.Text = _profile.UserName;
        }
        
        private void txtPort_Validating(object sender, CancelEventArgs e)
        {
            int result;
            bool isInt = int.TryParse(txtPort.Text, out result);
            if (isInt && result > 0)
            {
                return;
            }

            e.Cancel = true;
            txtPort.SelectAll();
            errorProvider1.SetError(txtPort, "Please provide a valid port number");
        }
        
        private void txtPort_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtPort, string.Empty);
        }
        
        private void txtSet_Validating(object sender, CancelEventArgs e)
        {
            ValidatingTextBox(txtSet, e);
        }
        
        private void txtSet_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtSet, string.Empty);
        }

        private void txtGet_Validating(object sender, CancelEventArgs e)
        {
            ValidatingTextBox(txtGet, e);
        }
        
        private void txtGet_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtGet, string.Empty);
        }

        private void txtIP_Validating(object sender, CancelEventArgs e)
        {
            IPAddress ip;
            if (AgentProfile.IsValid(txtIP.Text, out ip))
            {
                return;
            }

            e.Cancel = true;
            txtIP.SelectAll();
            errorProvider1.SetError(txtIP, "IP address is not valid");
        }
        
        private void txtIP_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtIP, string.Empty);
        }
        
        private void BtnOKClick(object sender, EventArgs e)
        {
            ValidateAllChildren(this);
        }
        
        private void ValidateAllChildren(Control parent)
        {
            if (DialogResult == DialogResult.None)
            {
                return;    
            }
            
            foreach (Control c in parent.Controls)
            {
                c.Focus();
                if (!Validate())
                {
                    DialogResult = DialogResult.None;
                    return;
                }
                
                ValidateAllChildren(c);
            }
        }

        private void CbVersionCodeSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateV3Controls(cbVersionCode.SelectedIndex == 2);
        }

        private void UpdateV3Controls(bool isV3)
        {
            cbAuthentication.Enabled = isV3;
            cbPrivacy.Enabled = isV3;
            txtAuthentication.Enabled = isV3;
            txtPrivacy.Enabled = isV3;
            txtUserName.Enabled = isV3;
            txtGet.Enabled = !isV3;
            txtSet.Enabled = !isV3;
        }

        private void txtAuthentication_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtAuthentication, string.Empty);
        }

        private void txtAuthentication_Validating(object sender, CancelEventArgs e)
        {
            ValidatingTextBox(txtAuthentication, e);
        }

        private void txtPrivacy_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtPrivacy, string.Empty);
        }

        private void txtPrivacy_Validating(object sender, CancelEventArgs e)
        {
            ValidatingTextBox(txtPrivacy, e);
        }

        private void txtUserName_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtUserName, string.Empty);
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            ValidatingTextBox(txtUserName, e);
        }

        private void ValidatingTextBox(TextBoxBase control, CancelEventArgs e)
        {
            if (!control.Enabled)
            {
                return;
            }

            if (control.Text.Length != 0)
            {
                return;
            }

            e.Cancel = true;
            control.SelectAll();
            errorProvider1.SetError(control, "Text box cannot be empty");
        }
    }
}
