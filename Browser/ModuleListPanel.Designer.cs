/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/20
 * Time: 20:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Lextm.SharpSnmpLib.Browser
{
    partial class ModuleListPanel
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Loaded", System.Windows.Forms.HorizontalAlignment.Left);
        	System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Pending", System.Windows.Forms.HorizontalAlignment.Left);
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModuleListPanel));
        	this.listView1 = new System.Windows.Forms.ListView();
        	this.chName = new System.Windows.Forms.ColumnHeader();
        	this.actionList1 = new Crad.Windows.Forms.Actions.ActionList();
        	this.actAdd = new Crad.Windows.Forms.Actions.Action();
        	this.actRemove = new Crad.Windows.Forms.Actions.Action();
        	this.tsbtnAdd = new System.Windows.Forms.ToolStripButton();
        	this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        	this.tslblCount = new System.Windows.Forms.ToolStripLabel();
        	this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        	this.contextModuleMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        	((System.ComponentModel.ISupportInitialize)(this.actionList1)).BeginInit();
        	this.toolStrip1.SuspendLayout();
        	this.contextModuleMenu.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// listView1
        	// 
        	this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        	        	        	this.chName});
        	this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.listView1.FullRowSelect = true;
        	listViewGroup1.Header = "Loaded";
        	listViewGroup1.Name = "lvgLoaded";
        	listViewGroup2.Header = "Pending";
        	listViewGroup2.Name = "lvgPending";
        	this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
        	        	        	listViewGroup1,
        	        	        	listViewGroup2});
        	this.listView1.Location = new System.Drawing.Point(0, 25);
        	this.listView1.MultiSelect = false;
        	this.listView1.Name = "listView1";
        	this.listView1.Size = new System.Drawing.Size(465, 274);
        	this.listView1.TabIndex = 0;
        	this.listView1.UseCompatibleStateImageBehavior = false;
        	this.listView1.View = System.Windows.Forms.View.Details;
        	this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
        	// 
        	// chName
        	// 
        	this.chName.Text = "Name";
        	this.chName.Width = 381;
        	// 
        	// actionList1
        	// 
        	this.actionList1.Actions.Add(this.actAdd);
        	this.actionList1.Actions.Add(this.actRemove);
        	this.actionList1.ContainerControl = this;
        	// 
        	// actAdd
        	// 
        	this.actAdd.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.list_add;
        	this.actAdd.Text = "Add";
        	this.actAdd.ToolTipText = "Add";
        	this.actAdd.Execute += new System.EventHandler(this.actAdd_Execute);
        	// 
        	// actRemove
        	// 
        	this.actRemove.Text = "Remove";
        	this.actRemove.ToolTipText = "Remove Mib File";
        	this.actRemove.Update += new System.EventHandler(this.actRemove_Update);
        	this.actRemove.Execute += new System.EventHandler(this.actRemove_Execute);
        	// 
        	// tsbtnAdd
        	// 
        	this.actionList1.SetAction(this.tsbtnAdd, this.actAdd);
        	this.tsbtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnAdd.Image")));
        	this.tsbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.tsbtnAdd.Name = "tsbtnAdd";
        	this.tsbtnAdd.Size = new System.Drawing.Size(49, 22);
        	this.tsbtnAdd.Text = "Add";
        	// 
        	// removeToolStripMenuItem
        	// 
        	this.actionList1.SetAction(this.removeToolStripMenuItem, this.actRemove);
        	this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
        	this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
        	this.removeToolStripMenuItem.Text = "Remove";
        	// 
        	// toolStrip1
        	// 
        	this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.tsbtnAdd,
        	        	        	this.tslblCount});
        	this.toolStrip1.Location = new System.Drawing.Point(0, 0);
        	this.toolStrip1.Name = "toolStrip1";
        	this.toolStrip1.Size = new System.Drawing.Size(465, 25);
        	this.toolStrip1.TabIndex = 1;
        	this.toolStrip1.Text = "toolStrip1";
        	// 
        	// tslblCount
        	// 
        	this.tslblCount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        	this.tslblCount.Name = "tslblCount";
        	this.tslblCount.Size = new System.Drawing.Size(10, 22);
        	this.tslblCount.Text = " ";
        	// 
        	// openFileDialog1
        	// 
        	this.openFileDialog1.Filter = "Text files (*.txt)|*.txt|MIB files (*.mib)|*.mib|All files (*.*)|*.*";
        	this.openFileDialog1.Multiselect = true;
        	// 
        	// contextModuleMenu
        	// 
        	this.contextModuleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.removeToolStripMenuItem});
        	this.contextModuleMenu.Name = "contextModuleMenu";
        	this.contextModuleMenu.Size = new System.Drawing.Size(118, 26);
        	// 
        	// ModuleListPanel
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(465, 299);
        	this.Controls.Add(this.listView1);
        	this.Controls.Add(this.toolStrip1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "ModuleListPanel";
        	this.TabText = "Module List";
        	this.Load += new System.EventHandler(this.ModuleListPanel_Load);
        	((System.ComponentModel.ISupportInitialize)(this.actionList1)).EndInit();
        	this.toolStrip1.ResumeLayout(false);
        	this.toolStrip1.PerformLayout();
        	this.contextModuleMenu.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.ListView listView1;
        private Crad.Windows.Forms.Actions.ActionList actionList1;
        private Crad.Windows.Forms.Actions.Action actAdd;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnAdd;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripLabel tslblCount;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ContextMenuStrip contextModuleMenu;
        private Crad.Windows.Forms.Actions.Action actRemove;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}
