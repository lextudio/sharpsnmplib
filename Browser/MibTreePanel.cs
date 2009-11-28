/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 15:25
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Mib;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
    /// <summary>
    /// Description of MibTreePanel.
    /// </summary>
    internal partial class MibTreePanel : DockContent
    {
        private IObjectRegistry _objects;
        private IProfileRegistry _profiles;
        private Manager _manager;

        public MibTreePanel()
        {
            InitializeComponent();
        }

        [Dependency]
        public IObjectRegistry Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        private void RefreshPanel(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate { RefreshPanel(sender, e); });
                return;
            }

            ReloadableObjectRegistry repository = (ReloadableObjectRegistry)sender;
            treeView1.Nodes.Clear();
            TreeNode root = Wrap(repository.Tree.Root);
            foreach (TreeNode node in root.Nodes)
            {
                treeView1.Nodes.Add(node);
            }
        }

        private TreeNode Wrap(IDefinition definition)
        {
            string name = _showNumber ? string.Format("{0}({1})", definition.Name, definition.Value) : definition.Name;
            TreeNode node = new TreeNode(name);
            node.Tag = definition;
            node.ImageIndex = (int)definition.Type;
            node.SelectedImageIndex = (int)definition.Type;
            node.ToolTipText = definition.TextualForm + Environment.NewLine + definition.Value;

            List<IDefinition> list = new List<IDefinition>(definition.Children);
            list.Sort(new DefinitionComparer());
            foreach (IDefinition def in list)
            {
                node.Nodes.Add(Wrap(def));
            }

            return node;
        }
        
        private class DefinitionComparer: IComparer<IDefinition>
        {      	        	
			public int Compare(IDefinition x, IDefinition y)
			{
				return x.Value.CompareTo(y.Value);
			}
        }

        [Dependency]
        public Manager Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }

        [Dependency]
        public IProfileRegistry Profiles
        {
            get { return _profiles; }
            set { _profiles = value; }
        }

        private static string GetTextualForm(IDefinition def)
        {
            if (def.Type == DefinitionType.Scalar)
            {
                return def.TextualForm + ".0";
            }

            int index;
            using (FormIndex form = new FormIndex())
            {
                form.ShowDialog();
                index = form.Index;
            }

            return def.TextualForm + "." + index;
        }

        private void ActGetExecute(object sender, EventArgs e)
        {
            TraceSource source = new TraceSource("Browser");
            try
            {
                source.TraceInformation("==== Begin GET ====");
                Profiles.DefaultProfile.Get(Manager, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition));
            }
            catch (Exception ex)
            {
                source.TraceInformation(ex.ToString());
            }
            finally
            {
                source.TraceInformation("==== End GET ====");
                source.Flush();
                source.Close();
            }
        }

        private void ActGetUpdate(object sender, EventArgs e)
        {
            actGet.Enabled = Validate(treeView1.SelectedNode);
        }

        private void ActSetExecute(object sender, EventArgs e)
        {
            TraceSource source = new TraceSource("Browser");
            try
            {
                ISnmpData data;
                using (FormSet form = new FormSet())
                {
                    form.OldVal = Profiles.DefaultProfile.GetValue(Manager,
                                                                   GetTextualForm(
                                                                       treeView1.SelectedNode.Tag as IDefinition));
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    
                    if (form.IsString)
                    {
                        data = new OctetString(form.NewVal);
                    }
                    else
                    {
                        int result;
                        if (!int.TryParse(form.NewVal, out result))
                        {
                            MessageBox.Show("Value entered was not an Integer!", "SNMP Set Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        data = new Integer32(result);
                    }
                }

                source.TraceInformation("==== Begin SET ====");
                Profiles.DefaultProfile.Set(Manager,
                                                GetTextualForm(treeView1.SelectedNode.Tag as IDefinition),
                                                data);
            }
            catch (Exception ex)
            {
                source.TraceInformation(ex.ToString());
            }
            finally
            {
                source.TraceInformation("==== End SET ====");
                source.Flush();
                source.Close();
            }
        }

        private void ActSetUpdate(object sender, EventArgs e)
        {
            actSet.Enabled = Validate(treeView1.SelectedNode);
        }

        private static bool Validate(TreeNode treeNode)
        {
            if (treeNode == null)
            {
                return false;
            }

            //  Scalar or Column. (see DefinitionType.cs)
            return treeNode.ImageIndex == 2 || treeNode.ImageIndex == 5;
        }

        private void ActGetTableExecute(object sender, EventArgs e)
        {
            try
            {
                Profiles.DefaultProfile.GetTable(Manager, treeView1.SelectedNode.Tag as IDefinition);
            }
            catch (Exception ex)
            {
                TraceSource source = new TraceSource("Browser");
                source.TraceInformation(ex.ToString());
                source.Flush();
                source.Close();
            }
        }

        private void ActGetTableUpdate(object sender, EventArgs e)
        {
            actGetTable.Enabled = treeView1.SelectedNode != null && treeView1.SelectedNode.ImageIndex == 3;
        }

        private void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
        {
            tslblOID.Text = ObjectIdentifier.Convert(((IDefinition) e.Node.Tag).GetNumericalForm());
            if (Validate(e.Node))
            {
                ActGetExecute(sender, e);
            }
            else if (e.Node.ImageIndex == 3)
            {
                ActGetTableExecute(sender, e);
            }
        }

        private void ActGetNextExecute(object sender, EventArgs e)
        {
            TraceSource source = new TraceSource("Browser");
            try
            {
                source.TraceInformation("==== Begin GET NEXT ====");
                Profiles.DefaultProfile.GetNext(Manager, GetTextualForm(treeView1.SelectedNode.Tag as IDefinition));
            }
            catch (Exception ex)
            {
                source.TraceInformation(ex.ToString());
            }
            finally
            {
                source.TraceInformation("==== End GET NEXT ====");
                source.Flush();
                source.Close();
            }
        }

        private void ActGetNextUpdate(object sender, EventArgs e)
        {
            actGetNext.Enabled = treeView1.SelectedNode != null && Validate(treeView1.SelectedNode);
        }


/*
        private void ManualWalk(TreeNode node, bool first)
        {
            if (node != null)
            {
                try
                {
                    switch (node.ImageIndex)
                    {
                        case 3:
                            Profiles.DefaultProfile.GetTable(Manager, node.Tag as IDefinition);
                            break;
                        default:
                            if (Validate(node))
                            {
                                Profiles.DefaultProfile.Get(Manager, GetTextualForm(node.Tag as IDefinition));
                            }
                            else
                            {
                                //
                                // TODO: I would like to be able to put headings for the parent of the child nodes
                                //
                                ManualWalk(node.Nodes[0], false);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    TraceSource source = new TraceSource("Browser");
                    source.TraceInformation(ex.ToString());
                    source.Flush();
                    source.Close();
                }

                if (!first)
                {
                    ManualWalk(node.NextNode, false);
                }
            }
        }
*/

        private void MibTreePanel_Load(object sender, EventArgs e)
        {
            RefreshPanel(Objects, EventArgs.Empty);
            Objects.OnChanged += RefreshPanel;
        }

        private bool _showNumber;

        private void ActNumberExecute(object sender, EventArgs e)
        {
            _showNumber = !_showNumber;
            RefreshPanel(Objects, EventArgs.Empty);
        }

        private void ActWalkExecute(object sender, EventArgs e)
        {
            TraceSource source = new TraceSource("Browser");
            try
            {
                source.TraceInformation("==== Begin WALK ====");
                Profiles.DefaultProfile.Walk(Manager, (treeView1.SelectedNode.Tag as IDefinition));                
            }
            catch (Exception ex)
            {
                source.TraceInformation(ex.ToString());
            }
            finally
            {
                source.TraceInformation("==== End WALK ====");
                source.Flush();
                source.Close();
            }
        }

        private void ActWalkUpdate(object sender, EventArgs e)
        {
            actWalk.Enabled = treeView1.SelectedNode != null && treeView1.SelectedNode.Level > 0;
        }
    }
}
