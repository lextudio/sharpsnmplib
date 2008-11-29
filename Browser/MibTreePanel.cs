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
            TreeNode node = new TreeNode(definition.Name);
            node.Tag = definition;
            node.ImageIndex = (int)definition.Type;
            node.SelectedImageIndex = (int)definition.Type;
            node.ToolTipText = definition.TextualForm + Environment.NewLine + definition.Value;

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
                using (FormSet form = new FormSet())
                {
                    form.OldVal = ProfileRegistry.Instance.DefaultProfile.GetValue(manager1, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition));
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        //
                        // If its a string we will send the value as an OctetString, otherwise an Integer32... we might have to update this
                        // to support other data types later
                        //
                        if (form.IsString)
                        {
                            OctetString newVal = new OctetString(form.NewVal);
                            ProfileRegistry.Instance.DefaultProfile.Set(manager1, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition), newVal);
                        }
                        else
                        {
                            int result;

                            if (!int.TryParse(form.NewVal, out result))
                            {
                                MessageBox.Show("Value entered was not an Integer!", "SNMP Set Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            Integer32 newVal = new Integer32(result);
                            ProfileRegistry.Instance.DefaultProfile.Set(manager1, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition), newVal);
                        }

                        //
                        // For now so we can see the change occured
                        //
                        actGet_Execute(null, null);
                    }
                }
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
                if (actGet.Enabled == false)
                {
                    ManualWalk(treeView1.SelectedNode, true);
                }
                else
                {
                    ProfileRegistry.Instance.DefaultProfile.Walk(manager1, treeView1.SelectedNode.Tag as IDefinition);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void actWalk_Update(object sender, EventArgs e)
        {
            actWalk.Enabled = !Validate(treeView1.SelectedNode);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tslblOID.Text = ObjectIdentifier.Convert((e.Node.Tag as IDefinition).GetNumericalForm());
            if (Validate(e.Node))
            {
                actGet_Execute(sender, e);
            }
            else if(e.Node.ImageIndex == 3)
            {
                actWalk_Execute(sender, e);
            }
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {/*
            if (e.Node.Tag.ToString() != toolTip1.GetToolTip(this.treeView1))
            {
                try
                {
                    toolTip1.SetToolTip(this.treeView1, ProfileRegistry.Instance.DefaultProfile.Get(manager1, GetTextualForm(e.Node.Tag as IDefinition)));
                }
                catch (Exception ex)
                {
                }
            }*/
        }

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {/*
            TreeNode theNode = this.treeView1.GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if ((theNode != null))
            {
                // Verify that the tag property is not "null".
                if (theNode.Tag != null)
                {
                    // Change the ToolTip only if the pointer moved to a new node.
                    if (theNode.Tag.ToString() != this.toolTip1.GetToolTip(this.treeView1))
                    {
                        this.toolTip1.SetToolTip(this.treeView1, theNode.Tag.ToString());
                    }
                }
                else
                {
                    this.toolTip1.SetToolTip(this.treeView1, "");
                }
            }
            else     // Pointer is not over a node so clear the ToolTip.
            {
                this.toolTip1.SetToolTip(this.treeView1, "");
            }*/
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode theNode = this.treeView1.GetNodeAt(e.X, e.Y);

            if (e.Button == MouseButtons.Left && theNode != null && !Validate(theNode))
            {
                actWalk_Execute(sender, e);
            }
        }

        private void actGetNext_Execute(object sender, EventArgs e)
        {
            try
            {
                ProfileRegistry.Instance.DefaultProfile.Get(manager1, GetTextualForm(treeView1.SelectedNode.NextNode.Tag as IDefinition));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

	private void actGetNext_Update(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                actGetNext.Enabled = Validate(treeView1.SelectedNode);
            }
            else
            {
                actGetNext.Enabled = false;
            }
        }
        

        private void ManualWalk(TreeNode node, bool first)
        {
            if (node != null)
            {
                try
                {
                    switch (node.ImageIndex)
                    {
                        case 3:
                            ProfileRegistry.Instance.DefaultProfile.Walk(manager1, node.Tag as IDefinition);
                            break;
                        default:
                            if (Validate(node))
                            {
                                ProfileRegistry.Instance.DefaultProfile.Get(manager1, GetTextualForm(node.Tag as IDefinition));
                            }
                            else
                            {
                                //
                                // TODO: I would like to be ablet to put headings for the parent of the child nodes
                                //
                                ManualWalk(node.Nodes[0], false);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (!first)
                {
                    ManualWalk(node.NextNode, false);
                }
            }
        }
    }
}
