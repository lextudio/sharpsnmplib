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

namespace Browser
{
    /// <summary>
    /// Description of ModuleListPanel.
    /// </summary>
    public partial class ModuleListPanel : DockContent
    {
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
            listView1.SuspendLayout();
            listView1.Items.Clear();
            foreach (string module in reg.Tree.LoadedModules)
            {
                listView1.Items.Add(module);
            }
            foreach (string pending in reg.Tree.PendingModules)
            {
                ListViewItem item = new ListViewItem(pending);
                item.BackColor = Color.LightGray;
                listView1.Items.Add(item);
            }
            listView1.ResumeLayout();
            tslblCount.Text = "module count: " + reg.Tree.LoadedModules.Count + "; pending count: " + reg.Tree.PendingModules.Count;
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
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
    }
}
