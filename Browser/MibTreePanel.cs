/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 15:25
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Mib;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
    /// <summary>
    /// Description of MibTreePanel.
    /// </summary>
    partial class MibTreePanel : DockContent
    {
        public MibTreePanel()
        {
            InitializeComponent();
            RefreshPanel(ObjectRegistry.Instance, EventArgs.Empty);
            ObjectRegistry.Instance.OnChanged += RefreshPanel;
        }
        
        private void RefreshPanel(object sender, EventArgs e)
        {
            ObjectRegistry repository = (ObjectRegistry)sender;
            treeView1.Nodes.Clear();
            TreeNode root = Wrap(repository.Tree.Root);
            foreach (TreeNode node in root.Nodes)
            {
                treeView1.Nodes.Add(node);
            }
        }

        private static TreeNode Wrap(IDefinition definition)
        {
            TreeNode node = new TreeNode(definition.Name)
            {
                Tag = definition,
                ImageIndex = (int)definition.Type,
                SelectedImageIndex = (int)definition.Type,
                ToolTipText = definition.TextualForm + Environment.NewLine + definition.Value
            };
            foreach (IDefinition def in definition.Children)
            {
                node.Nodes.Add(Wrap(def));
            }
            return node;
        }

        private void actGet_Execute(object sender, EventArgs e)
        {
            try
            {
                ProfileRegistry.Instance.DefaultProfile.Get(manager1, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetTextualForm(IDefinition def)
        {
            if (def.Type == DefinitionType.Scalar)
            {
                return def.TextualForm + ".0";
            }
            else
            {
                int index;
                using (FormIndex form = new FormIndex())
                {
                    form.ShowDialog();
                    index = form.Index;
                }
                return def.TextualForm + "." + index;
            }
        }

        private void actGet_Update(object sender, EventArgs e)
        {
            actGet.Enabled = Validate(treeView1.SelectedNode);
        }

        private void actSet_Execute(object sender, EventArgs e)
        {
            try
            {
                ProfileRegistry.Instance.DefaultProfile.Set(manager1, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void actSet_Update(object sender, EventArgs e)
        {
            actSet.Enabled = Validate(treeView1.SelectedNode);
        }

        static Regex ipRegex = new Regex(
            "^(?<First>2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.(?<Second>2[0-4]" +
            "\\d|25[0-5]|[01]?\\d\\d?)\\.(?<Third>2[0-4]\\d|25[0-5]|[01]?" +
            "\\d\\d?)\\.(?<Fourth>2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.CultureInvariant
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
           );

        private static bool Validate(string ip)
        {
            return ipRegex.IsMatch(ip);
        }

        private static bool Validate(TreeNode treeNode)
        {
            if (treeNode == null)
            {
                return false;
            }
            return treeNode.ImageIndex == 2 || treeNode.ImageIndex == 5;
        }

        private void actWalk_Execute(object sender, EventArgs e)
        {
            try
            {
                ProfileRegistry.Instance.DefaultProfile.Walk(manager1, treeView1.SelectedNode.Tag as IDefinition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tslblOID.Text = ObjectIdentifier.Convert((e.Node.Tag as IDefinition).GetNumericalForm());
        }
    }
}
