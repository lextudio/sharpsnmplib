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
        private Inventory _inventory = new Inventory(ObjectRegistry.Instance);
        
        public ModuleListPanel()
        {            
            InitializeComponent();        
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
                _inventory.AddFiles(openFileDialog1.FileNames);
            }
        }

        private void actRemove_Execute(object sender, EventArgs e)
        {
            string mib = listView1.SelectedItems[0].Text;
            string fileName = _inventory[mib];
            _inventory.RemoveFile(fileName);
        }

        private void actRemove_Update(object sender, EventArgs e)
        {
            actRemove.Enabled = listView1.SelectedItems.Count == 1 && _inventory.Contains(listView1.SelectedItems[0].Text);
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
