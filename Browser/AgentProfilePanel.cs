using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
    public partial class AgentProfilePanel : DockContent
    {
        public AgentProfilePanel()
        {
            InitializeComponent();
        }

        private void AgentProfilePanel_Load(object sender, System.EventArgs e)
        {
            foreach (AgentProfile agent in ProfileRegistry.Profiles)
            {
                ListViewItem item = listView1.Items.Add(agent.IP);
                item.Tag = agent;
                switch (agent.VersionCode)
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
            ProfileRegistry.Default = listView1.SelectedItems[0].Text;
        }

        private void actionList1_Update(object sender, System.EventArgs e)
        {
            tslblDefault.Text = ProfileRegistry.Default;
        }

        private void actDelete_Execute(object sender, System.EventArgs e)
        {
            ProfileRegistry.DeleteProfile(listView1.SelectedItems[0].Text);
        }

        private void actAdd_Execute(object sender, System.EventArgs e)
        {
            using (FormProfile editor = new FormProfile(null))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    ProfileRegistry.AddProfile(new AgentProfile(editor.IP, editor.VersionCode, editor.GetCommunity, editor.SetCommunity));
                }
            }
        }

        private void actEdit_Execute(object sender, System.EventArgs e)
        {
            AgentProfile profile = ProfileRegistry.GetProfile(listView1.SelectedItems[0].Text);
            using (FormProfile editor = new FormProfile(profile))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    profile.Update(editor.IP, editor.VersionCode, editor.GetCommunity, editor.SetCommunity);
                }
            }
        }
    }
}
