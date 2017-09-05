using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Windows.Forms;

namespace BytesViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tscbAuthentication.SelectedIndex = 0;
            tscbPrivacy.SelectedIndex = 0;
        }

        private void txtBytes_TextChanged(object sender, EventArgs e)
        {
            tvMessage.Nodes.Clear();
            var users = new UserRegistry();
            IAuthenticationProvider authen;
            if (tscbAuthentication.SelectedIndex == 0)
            {
                authen = DefaultAuthenticationProvider.Instance;
            }
            else if (tscbAuthentication.SelectedIndex == 1)
            {
                authen = new MD5AuthenticationProvider(new OctetString(tstxtAuthentication.Text));
            }
            else
            {
                authen = new SHA1AuthenticationProvider(new OctetString(tstxtAuthentication.Text));
            }

            IPrivacyProvider privacy;
            if (tscbPrivacy.SelectedIndex == 0)
            {
                privacy = new DefaultPrivacyProvider(authen);
            }
            else if (tscbPrivacy.SelectedIndex == 1)
            {
                privacy = new DESPrivacyProvider(new OctetString(tstxtPrivacy.Text), authen);
            }
            else
            {
                privacy = new AESPrivacyProvider(new OctetString(tstxtPrivacy.Text), authen);
            }

            users.Add(new User(new OctetString(tstxtUser.Text), privacy));

            try
            {
                var messages = MessageFactory.ParseMessages(ByteTool.Convert(txtBytes.Text.Replace("\"", null).Replace("+", null)), users);
                messages.Fill(tvMessage);
            }
            catch (Exception ex)
            {
                tvMessage.Nodes.Add(ex.Message);
            }
        }
    }
}
