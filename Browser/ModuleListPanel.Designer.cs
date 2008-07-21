/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/20
 * Time: 20:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Browser
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.actionList1 = new Crad.Windows.Forms.Actions.ActionList();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnAdd = new System.Windows.Forms.ToolStripButton();
            this.actAdd = new Crad.Windows.Forms.Actions.Action();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tslblCount = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.actionList1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(465, 251);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // actionList1
            // 
            this.actionList1.Actions.Add(this.actAdd);
            this.actionList1.ContainerControl = this;
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
            // tsbtnAdd
            // 
            this.actionList1.SetAction(this.tsbtnAdd, this.actAdd);
            this.tsbtnAdd.Image = global::Browser.Properties.Resources.list_add;
            this.tsbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnAdd.Name = "tsbtnAdd";
            this.tsbtnAdd.Size = new System.Drawing.Size(52, 22);
            this.tsbtnAdd.Text = "Add";
            // 
            // actAdd
            // 
            this.actAdd.Image = global::Browser.Properties.Resources.list_add;
            this.actAdd.Text = "Add";
            this.actAdd.ToolTipText = "Add";
            this.actAdd.Execute += new System.EventHandler(this.actAdd_Execute);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Text files (*.txt)|*.txt|MIB files (*.mib)|*.mib|All files (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            // 
            // tslblCount
            // 
            this.tslblCount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslblCount.Name = "tslblCount";
            this.tslblCount.Size = new System.Drawing.Size(12, 22);
            this.tslblCount.Text = " ";
            // 
            // ModuleListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 276);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ModuleListPanel";
            this.TabText = "Module List";
            this.Load += new System.EventHandler(this.ModuleListPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.actionList1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
    }
}
