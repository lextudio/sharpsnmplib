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

namespace Lextm.SharpSnmpLib.Compiler
{
    /// <summary>
    /// Description of ModuleListPanel.
    /// </summary>
    internal partial class ModuleListPanel : DockContent
    {
        private IMediator _mediator;
        
        public ModuleListPanel(IMediator mediator)
        {
            if (mediator == null)
            {
                throw new ArgumentNullException("mediator");
            }
            
            InitializeComponent();
            _mediator = mediator;            
        }
        
        void ModuleListPanel_Load(object sender, EventArgs e)
        {
            RefreshPanel(this, EventArgs.Empty);
        }

        public void RefreshPanel(object sender, EventArgs e)
        {
            SuspendLayout();
            listView1.Items.Clear();
            foreach (string module in _mediator.Tree.LoadedModules)
            {
                ListViewItem item = listView1.Items.Add(module);
                item.Group = listView1.Groups["lvgLoaded"];
            }
            foreach (string pending in _mediator.Tree.PendingModules)
            {
                ListViewItem item = listView1.Items.Add(pending);
                item.BackColor = Color.LightGray;
                item.Group = listView1.Groups["lvgPending"];
            }
            ResumeLayout();
            listView1.Groups["lvgLoaded"].Header = string.Format("Loaded ({0})", _mediator.Tree.LoadedModules.Count);
            listView1.Groups["lvgPending"].Header = string.Format("Pending ({0})", _mediator.Tree.PendingModules.Count);
            tslblCount.Text = "loaded count: " + _mediator.Tree.LoadedModules.Count + "; pending count: " + _mediator.Tree.PendingModules.Count;
        }

        private void actRemove_Execute(object sender, EventArgs e)
        {
            string mib = listView1.SelectedItems[0].Text;

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
