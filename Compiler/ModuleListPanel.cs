/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/20
 * Time: 20:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Mib;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
    /// <summary>
    /// Description of ModuleListPanel.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal partial class ModuleListPanel : DockContent
    {
        private Assembler _assembler;
        private CompilerCore _compiler;

        public ModuleListPanel()
        {
            InitializeComponent();
        }
        
        internal void ModuleListPanel_Load(object sender, EventArgs e)
        {
            // TODO: bind this refresh to assembler event.
            RefreshPanel(this, EventArgs.Empty);
            if (Compiler != null)
            {
                Compiler.RunCompilerCompleted += RefreshPanel;
            }
        }

        [Dependency]
        public CompilerCore Compiler
        {
            get { return _compiler; }
            set { _compiler = value; }
        }

        public void RefreshPanel(object sender, EventArgs e)
        {
            SuspendLayout();
            listView1.Items.Clear();
            List<string> loaded = new List<string>(Assembler.Tree.LoadedModules);
            loaded.Sort();
            foreach (string module in loaded)
            {
                ListViewItem item = listView1.Items.Add(module);
                item.Group = listView1.Groups["lvgLoaded"];
            }
            
            List<string> pendings = new List<string>(Assembler.Tree.PendingModules);
            pendings.Sort();
            foreach (string pending in pendings)
            {
                ListViewItem item = listView1.Items.Add(pending);
                item.BackColor = Color.LightGray;
                item.Group = listView1.Groups["lvgPending"];
            }
            
            ResumeLayout();
            listView1.Groups["lvgLoaded"].Header = string.Format(CultureInfo.InvariantCulture, "Loaded ({0})", Assembler.Tree.LoadedModules.Count);
            listView1.Groups["lvgPending"].Header = string.Format(CultureInfo.InvariantCulture, "Pending ({0})", Assembler.Tree.PendingModules.Count);
            tslblCount.Text = "loaded: " + Assembler.Tree.LoadedModules.Count + "; unloaded: " + Assembler.Tree.PendingModules.Count;
        }

        [Dependency]
        public Assembler Assembler
        {
            get { return _assembler; }
            set { _assembler = value; }
        }

        private void ActRemoveExecute(object sender, EventArgs e)
        {
            string mib = listView1.SelectedItems[0].Text;
            File.Delete(Path.Combine(Assembler.Folder, mib + ".module"));
            TraceSource source = new TraceSource("Compiler");
            source.TraceInformation("The change will take effect when you restart compiler");
            source.Flush();
            source.Close();
            Assembler.Tree.Remove(mib);
        }

        private void ActRemoveUpdate(object sender, EventArgs e)
        {
            actRemove.Enabled = listView1.SelectedItems.Count > 0;
        }

        private void ListView1MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextModuleMenu.Show(listView1, e.Location);
            }
        }
    }
}
