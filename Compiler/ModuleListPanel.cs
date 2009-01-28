/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/20
 * Time: 20:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Diagnostics;
using System;
using System.Drawing;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Mib;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Lextm.SharpSnmpLib.Compiler
{
    /// <summary>
    /// Description of ModuleListPanel.
    /// </summary>
    internal partial class ModuleListPanel : DockContent
    {
        private Assembler _assembler;

        public ModuleListPanel()
        {
            InitializeComponent();
        }
        
        internal void ModuleListPanel_Load(object sender, EventArgs e)
        {
            // TODO: bind this refresh to assembler event.
            RefreshPanel(this, EventArgs.Empty);
        }

        public void RefreshPanel(object sender, EventArgs e)
        {
            SuspendLayout();
            listView1.Items.Clear();
            foreach (string module in Assembler.Tree.LoadedModules)
            {
                ListViewItem item = listView1.Items.Add(module);
                item.Group = listView1.Groups["lvgLoaded"];
            }
            foreach (string pending in Assembler.Tree.PendingModules)
            {
                ListViewItem item = listView1.Items.Add(pending);
                item.BackColor = Color.LightGray;
                item.Group = listView1.Groups["lvgPending"];
            }
            ResumeLayout();
            listView1.Groups["lvgLoaded"].Header = string.Format("Loaded ({0})", Assembler.Tree.LoadedModules.Count);
            listView1.Groups["lvgPending"].Header = string.Format("Pending ({0})", Assembler.Tree.PendingModules.Count);
            tslblCount.Text = "loaded count: " + Assembler.Tree.LoadedModules.Count + "; pending count: " + Assembler.Tree.PendingModules.Count;
        }

        [Dependency]
        public Assembler Assembler
        {
            get { return _assembler; }
            set { _assembler = value; }
        }

        private void actRemove_Execute(object sender, EventArgs e)
        {
            string mib = listView1.SelectedItems[0].Text;
            File.Delete(Path.Combine(Assembler.Folder, mib + ".module"));
            TraceSource source = new TraceSource("Compiler");
            source.TraceInformation("The change will take effect when you restart compiler");
            source.Flush();
            source.Close();
            Assembler.Tree.Remove(mib);
        }

        private void actRemove_Update(object sender, EventArgs e)
        {
            actRemove.Enabled = listView1.SelectedItems.Count > 0;
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
