/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/20
 * Time: 20:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Lextm.SharpSnmpLib.Mib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
    /// <summary>
    /// Description of ModuleListPanel.
    /// </summary>
    public partial class ModuleListPanel : DockContent
    {
        private IOutput _output;
        
        public ModuleListPanel(IOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            
            InitializeComponent();
            _output = output;
            ObjectRegistry.Instance.OnChanged += RefreshPanel;
        }
        
        void ModuleListPanel_Load(object sender, EventArgs e)
        {
            RefreshPanel(ObjectRegistry.Instance, EventArgs.Empty);
        }

        private void RefreshPanel(object sender, EventArgs e)
        {
            ObjectRegistry reg = (ObjectRegistry)sender;
            SuspendLayout();
            listView1.Items.Clear();
            foreach (string module in reg.Tree.LoadedModules)
            {
                ListViewItem item = listView1.Items.Add(module);
                item.Group = listView1.Groups["lvgLoaded"];
            }
            foreach (string pending in reg.Tree.PendingModules)
            {
                ListViewItem item = listView1.Items.Add(pending);
                item.BackColor = Color.LightGray;
                item.Group = listView1.Groups["lvgPending"];
            }
            ResumeLayout();
            listView1.Groups["lvgLoaded"].Header = string.Format("Loaded ({0})", reg.Tree.LoadedModules.Count);
            listView1.Groups["lvgPending"].Header = string.Format("Pending ({0})", reg.Tree.PendingModules.Count);
            tslblCount.Text = "loaded count: " + reg.Tree.LoadedModules.Count + "; pending count: " + reg.Tree.PendingModules.Count;
        }

        private void actAdd_Execute(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    try
                    {
                        ObjectRegistry.Instance.LoadFile(file);
                    }
                    catch (SharpMibException ex)
                    {
                        _output.ReportMessage(ex.ToString());
                    }
                }
            }
        }

        private void actRemove_Execute(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1 && ObjectRegistry.Instance.Tree.NewModules.Contains(listView1.SelectedItems[0].Text))
            {
                string mib = listView1.SelectedItems[0].Text;
                ObjectRegistry.Instance.RemoveMib(mib, listView1.SelectedItems[0].Group.Name);
            }
            else
            {
                //
                // What error msg do we put here? or just an exception?
                //
            }
        }

        private void actRemove_Update(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 1)
            {
                string mib = listView1.SelectedItems[0].Text;
                actRemove.Enabled = ObjectRegistry.Instance.Tree.NewModules.Contains(mib);
            }
            else
            {
                actRemove.Enabled = false;
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextModuleMenu.Show(listView1, e.Location);
            }
        }
    }
}
