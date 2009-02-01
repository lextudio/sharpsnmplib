namespace Lextm.SharpSnmpLib.Browser
{
    partial class AgentProfilePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("SNMP v1", System.Windows.Forms.HorizontalAlignment.Left);
        	System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("SNMP v2c", System.Windows.Forms.HorizontalAlignment.Left);
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentProfilePanel));
        	this.listView1 = new System.Windows.Forms.ListView();
        	this.chName = new System.Windows.Forms.ColumnHeader();
        	this.chIP = new System.Windows.Forms.ColumnHeader();
        	this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        	this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
        	this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
        	this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
        	this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        	this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
        	this.actionList1 = new Crad.Windows.Forms.Actions.ActionList();
        	this.actAdd = new Crad.Windows.Forms.Actions.Action();
        	this.actDelete = new Crad.Windows.Forms.Actions.Action();
        	this.actEdit = new Crad.Windows.Forms.Actions.Action();
        	this.actDefault = new Crad.Windows.Forms.Actions.Action();
        	this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.setDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.contextAgentMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        	this.tslblDefault = new System.Windows.Forms.ToolStripLabel();
        	this.toolStrip1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.actionList1)).BeginInit();
        	this.contextAgentMenu.SuspendLayout();
        	this.statusStrip1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// listView1
        	// 
        	this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        	        	        	this.chName,
        	        	        	this.chIP});
        	this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.listView1.FullRowSelect = true;
        	listViewGroup1.Header = "SNMP v1";
        	listViewGroup1.Name = "lvgV1";
        	listViewGroup2.Header = "SNMP v2c";
        	listViewGroup2.Name = "lvgV2";
        	this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
        	        	        	listViewGroup1,
        	        	        	listViewGroup2});
        	this.listView1.Location = new System.Drawing.Point(0, 25);
        	this.listView1.MultiSelect = false;
        	this.listView1.Name = "listView1";
        	this.listView1.ShowItemToolTips = true;
        	this.listView1.Size = new System.Drawing.Size(366, 246);
        	this.listView1.TabIndex = 0;
        	this.listView1.UseCompatibleStateImageBehavior = false;
        	this.listView1.View = System.Windows.Forms.View.Details;
        	this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
        	this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
        	// 
        	// chName
        	// 
        	this.chName.Text = "Name";
        	this.chName.Width = 159;
        	// 
        	// chIP
        	// 
        	this.chIP.Text = "IP/Port";
        	this.chIP.Width = 148;
        	// 
        	// toolStrip1
        	// 
        	this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.toolStripButton1,
        	        	        	this.toolStripButton2,
        	        	        	this.toolStripButton3,
        	        	        	this.toolStripSeparator1,
        	        	        	this.toolStripButton4});
        	this.toolStrip1.Location = new System.Drawing.Point(0, 0);
        	this.toolStrip1.Name = "toolStrip1";
        	this.toolStrip1.Size = new System.Drawing.Size(366, 25);
        	this.toolStrip1.TabIndex = 1;
        	this.toolStrip1.Text = "toolStrip1";
        	// 
        	// toolStripButton1
        	// 
        	this.actionList1.SetAction(this.toolStripButton1, this.actAdd);
        	this.toolStripButton1.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.list_add;
        	this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.toolStripButton1.Name = "toolStripButton1";
        	this.toolStripButton1.Size = new System.Drawing.Size(49, 22);
        	this.toolStripButton1.Text = "Add";
        	// 
        	// toolStripButton2
        	// 
        	this.actionList1.SetAction(this.toolStripButton2, this.actDelete);
        	this.toolStripButton2.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.list_remove;
        	this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.toolStripButton2.Name = "toolStripButton2";
        	this.toolStripButton2.Size = new System.Drawing.Size(60, 22);
        	this.toolStripButton2.Text = "Delete";
        	// 
        	// toolStripButton3
        	// 
        	this.actionList1.SetAction(this.toolStripButton3, this.actEdit);
        	this.toolStripButton3.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.document_properties;
        	this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.toolStripButton3.Name = "toolStripButton3";
        	this.toolStripButton3.Size = new System.Drawing.Size(47, 22);
        	this.toolStripButton3.Text = "Edit";
        	// 
        	// toolStripSeparator1
        	// 
        	this.toolStripSeparator1.Name = "toolStripSeparator1";
        	this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
        	// 
        	// toolStripButton4
        	// 
        	this.actionList1.SetAction(this.toolStripButton4, this.actDefault);
        	this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.toolStripButton4.Name = "toolStripButton4";
        	this.toolStripButton4.Size = new System.Drawing.Size(68, 22);
        	this.toolStripButton4.Text = "Set Default";
        	// 
        	// actionList1
        	// 
        	this.actionList1.Actions.Add(this.actAdd);
        	this.actionList1.Actions.Add(this.actDelete);
        	this.actionList1.Actions.Add(this.actEdit);
        	this.actionList1.Actions.Add(this.actDefault);
        	this.actionList1.ContainerControl = this;
        	this.actionList1.Update += new System.EventHandler(this.actionList1_Update);
        	// 
        	// actAdd
        	// 
        	this.actAdd.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.list_add;
        	this.actAdd.Text = "Add";
        	this.actAdd.ToolTipText = "Add Profile";
        	this.actAdd.Execute += new System.EventHandler(this.actAdd_Execute);
        	// 
        	// actDelete
        	// 
        	this.actDelete.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.list_remove;
        	this.actDelete.Text = "Delete";
        	this.actDelete.ToolTipText = "Delete Profile";
        	this.actDelete.Update += new System.EventHandler(this.actDelete_Update);
        	this.actDelete.Execute += new System.EventHandler(this.actDelete_Execute);
        	// 
        	// actEdit
        	// 
        	this.actEdit.Image = global::Lextm.SharpSnmpLib.Browser.Properties.Resources.document_properties;
        	this.actEdit.Text = "Edit";
        	this.actEdit.ToolTipText = "Edit Profile";
        	this.actEdit.Update += new System.EventHandler(this.actEdit_Update);
        	this.actEdit.Execute += new System.EventHandler(this.actEdit_Execute);
        	// 
        	// actDefault
        	// 
        	this.actDefault.Text = "Set Default";
        	this.actDefault.ToolTipText = "Set Default Profile";
        	this.actDefault.Update += new System.EventHandler(this.actDefault_Update);
        	this.actDefault.Execute += new System.EventHandler(this.actDefault_Execute);
        	// 
        	// editToolStripMenuItem
        	// 
        	this.actionList1.SetAction(this.editToolStripMenuItem, this.actEdit);
        	this.editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripMenuItem.Image")));
        	this.editToolStripMenuItem.Name = "editToolStripMenuItem";
        	this.editToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
        	this.editToolStripMenuItem.Text = "Edit";
        	// 
        	// setDefaultToolStripMenuItem
        	// 
        	this.actionList1.SetAction(this.setDefaultToolStripMenuItem, this.actDefault);
        	this.setDefaultToolStripMenuItem.Name = "setDefaultToolStripMenuItem";
        	this.setDefaultToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
        	this.setDefaultToolStripMenuItem.Text = "Set Default";
        	// 
        	// deleteToolStripMenuItem
        	// 
        	this.actionList1.SetAction(this.deleteToolStripMenuItem, this.actDelete);
        	this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
        	this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
        	this.deleteToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
        	this.deleteToolStripMenuItem.Text = "Delete";
        	// 
        	// contextAgentMenu
        	// 
        	this.contextAgentMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.editToolStripMenuItem,
        	        	        	this.setDefaultToolStripMenuItem,
        	        	        	this.deleteToolStripMenuItem});
        	this.contextAgentMenu.Name = "contextAgentMenu";
        	this.contextAgentMenu.Size = new System.Drawing.Size(132, 70);
        	// 
        	// statusStrip1
        	// 
        	this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.tslblDefault});
        	this.statusStrip1.Location = new System.Drawing.Point(0, 271);
        	this.statusStrip1.Name = "statusStrip1";
        	this.statusStrip1.Size = new System.Drawing.Size(366, 22);
        	this.statusStrip1.TabIndex = 2;
        	this.statusStrip1.Text = "statusStrip1";
        	// 
        	// tslblDefault
        	// 
        	this.tslblDefault.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        	this.tslblDefault.Name = "tslblDefault";
        	this.tslblDefault.Size = new System.Drawing.Size(10, 20);
        	this.tslblDefault.Text = " ";
        	// 
        	// AgentProfilePanel
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(366, 293);
        	this.Controls.Add(this.listView1);
        	this.Controls.Add(this.toolStrip1);
        	this.Controls.Add(this.statusStrip1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "AgentProfilePanel";
        	this.TabText = "Agent Profiles";
        	this.Load += new System.EventHandler(this.AgentProfilePanel_Load);
        	this.toolStrip1.ResumeLayout(false);
        	this.toolStrip1.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.actionList1)).EndInit();
        	this.contextAgentMenu.ResumeLayout(false);
        	this.statusStrip1.ResumeLayout(false);
        	this.statusStrip1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private Crad.Windows.Forms.Actions.ActionList actionList1;
        private Crad.Windows.Forms.Actions.Action actAdd;
        private Crad.Windows.Forms.Actions.Action actDelete;
        private Crad.Windows.Forms.Actions.Action actEdit;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private Crad.Windows.Forms.Actions.Action actDefault;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ContextMenuStrip contextAgentMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader chIP;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripLabel tslblDefault;
    }
}
