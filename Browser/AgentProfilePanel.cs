using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
    public partial class AgentProfilePanel : DockContent
    {
        public AgentProfilePanel()
        {
            InitializeComponent();
            ProfileRegistry.Instance.OnChanged += UpdateView;
        }

        private void AgentProfilePanel_Load(object sender, System.EventArgs e)
        {
            UpdateView(this, e);
        }

        private void UpdateView(object sender, System.EventArgs e)
        {
            listView1.Items.Clear();
            foreach (AgentProfile profile in ProfileRegistry.Instance.Profiles)
            {
                ListViewItem item = listView1.Items.Add(profile.Agent.ToString());
                item.Tag = profile;
                switch (profile.VersionCode)
                {
                    case VersionCode.V1:
                        {
                            item.Group = listView1.Groups["lvgV1"];
                            break;
                        }
                    case VersionCode.V2:
                        {
                            item.Group = listView1.Groups["lvgV2"];
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void actDelete_Update(object sender, System.EventArgs e)
        {
            actDelete.Enabled = listView1.SelectedItems.Count == 1;
        }

        private void actEdit_Update(object sender, System.EventArgs e)
        {
            actEdit.Enabled = listView1.SelectedItems.Count == 1;
        }

        private void actDefault_Update(object sender, System.EventArgs e)
        {
            actDefault.Enabled = listView1.SelectedItems.Count == 1;
        }

        private void actDefault_Execute(object sender, System.EventArgs e)
        {
            ProfileRegistry.Instance.Default = (listView1.SelectedItems[0].Tag as AgentProfile).Agent;
        }

        private void actionList1_Update(object sender, System.EventArgs e)
        {
            tslblDefault.Text = "Default agent is " + ProfileRegistry.Instance.DefaultString;
        }

        private void actDelete_Execute(object sender, System.EventArgs e)
        {
            ProfileRegistry.Instance.DeleteProfile((listView1.SelectedItems[0].Tag as AgentProfile).Agent);
        }

        private void actAdd_Execute(object sender, System.EventArgs e)
        {
            using (FormProfile editor = new FormProfile(null))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ProfileRegistry.Instance.AddProfile(new AgentProfile(editor.VersionCode, editor.IP, editor.Port, editor.GetCommunity, editor.SetCommunity));
                    }
                    catch (MibBrowserException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void actEdit_Execute(object sender, System.EventArgs e)
        {
            AgentProfile profile = ProfileRegistry.Instance.GetProfile((listView1.SelectedItems[0].Tag as AgentProfile).Agent);
            using (FormProfile editor = new FormProfile(profile))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    ProfileRegistry.Instance.ReplaceProfile(new AgentProfile(editor.VersionCode, editor.IP, editor.Port, editor.GetCommunity, editor.SetCommunity));
                }
            }
        }

        private void listView1_DoubleClick(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                actEdit.DoExecute();
            }
        }
    }
}
