/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 15:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Lextm.SharpSnmpLib.Browser
{
	partial class MibTreePanel
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MibTreePanel));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.getToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.actionList1 = new Crad.Windows.Forms.Actions.ActionList();
			this.actGet = new Crad.Windows.Forms.Actions.Action();
			this.actSet = new Crad.Windows.Forms.Actions.Action();
			this.actWalk = new Crad.Windows.Forms.Actions.Action();
			this.manager1 = new Lextm.SharpSnmpLib.Manager();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.actionList1)).BeginInit();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.ImageIndex = 0;
			this.treeView1.ImageList = this.imageList1;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = 0;
			this.treeView1.ShowNodeToolTips = true;
			this.treeView1.Size = new System.Drawing.Size(284, 264);
			this.treeView1.TabIndex = 0;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.getToolStripMenuItem,
									this.setToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(97, 48);
			// 
			// getToolStripMenuItem
			// 
			this.actionList1.SetAction(this.getToolStripMenuItem, this.actGet);
			this.getToolStripMenuItem.Name = "getToolStripMenuItem";
			this.getToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
			this.getToolStripMenuItem.Text = "Get";
			// 
			// setToolStripMenuItem
			// 
			this.actionList1.SetAction(this.setToolStripMenuItem, this.actSet);
			this.setToolStripMenuItem.Name = "setToolStripMenuItem";
			this.setToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
			this.setToolStripMenuItem.Text = "Set";
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "text-x-generic-template.png");
			this.imageList1.Images.SetKeyName(1, "package-x-generic.png");
			this.imageList1.Images.SetKeyName(2, "text-x-generic.png");
			this.imageList1.Images.SetKeyName(3, "x-office-spreadsheet.png");
			this.imageList1.Images.SetKeyName(4, "application-x-executable.png");
			this.imageList1.Images.SetKeyName(5, "text-x-script.png");
			// 
			// actionList1
			// 
			this.actionList1.Actions.Add(this.actGet);
			this.actionList1.Actions.Add(this.actSet);
			this.actionList1.Actions.Add(this.actWalk);
			this.actionList1.ContainerControl = this;
			// 
			// actGet
			// 
			this.actGet.Text = "Get";
			this.actGet.ToolTipText = "Get";
			this.actGet.Update += new System.EventHandler(this.actGet_Update);
			this.actGet.Execute += new System.EventHandler(this.actGet_Execute);
			// 
			// actSet
			// 
			this.actSet.Text = "Set";
			this.actSet.ToolTipText = "Set";
			this.actSet.Update += new System.EventHandler(this.actSet_Update);
			this.actSet.Execute += new System.EventHandler(this.actSet_Execute);
			// 
			// actWalk
			// 
			this.actWalk.Text = "Walk";
			this.actWalk.ToolTipText = "Walk";
			this.actWalk.Execute += new System.EventHandler(this.actWalk_Execute);
			// 
			// manager1
			// 
			this.manager1.DefaultVersion = Lextm.SharpSnmpLib.VersionCode.V1;
			this.manager1.Timeout = 5000;
			// 
			// MibTreePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 264);
			this.Controls.Add(this.treeView1);
			this.Name = "MibTreePanel";
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.actionList1)).EndInit();
			this.ResumeLayout(false);
		}
		private Lextm.SharpSnmpLib.Manager manager1;
		private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem getToolStripMenuItem;
        private Crad.Windows.Forms.Actions.ActionList actionList1;
        private Crad.Windows.Forms.Actions.Action actGet;
        private Crad.Windows.Forms.Actions.Action actSet;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private Crad.Windows.Forms.Actions.Action actWalk;
	}
}
