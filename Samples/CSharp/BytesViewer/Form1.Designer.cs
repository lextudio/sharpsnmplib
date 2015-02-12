namespace BytesViewer
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtBytes = new System.Windows.Forms.TextBox();
            this.tvMessage = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tstxtUser = new System.Windows.Forms.ToolStripTextBox();
            this.tscbAuthentication = new System.Windows.Forms.ToolStripComboBox();
            this.tstxtAuthentication = new System.Windows.Forms.ToolStripTextBox();
            this.tscbPrivacy = new System.Windows.Forms.ToolStripComboBox();
            this.tstxtPrivacy = new System.Windows.Forms.ToolStripTextBox();
            this.tsbtnAnalyze = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBytes
            // 
            this.txtBytes.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBytes.Location = new System.Drawing.Point(0, 25);
            this.txtBytes.Multiline = true;
            this.txtBytes.Name = "txtBytes";
            this.txtBytes.Size = new System.Drawing.Size(711, 75);
            this.txtBytes.TabIndex = 0;
            // 
            // tvMessage
            // 
            this.tvMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMessage.Location = new System.Drawing.Point(0, 100);
            this.tvMessage.Name = "tvMessage";
            this.tvMessage.Size = new System.Drawing.Size(711, 394);
            this.tvMessage.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tstxtUser,
            this.tscbAuthentication,
            this.tstxtAuthentication,
            this.tscbPrivacy,
            this.tstxtPrivacy,
            this.tsbtnAnalyze});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(711, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabel1.Text = "User";
            // 
            // tstxtUser
            // 
            this.tstxtUser.Name = "tstxtUser";
            this.tstxtUser.Size = new System.Drawing.Size(100, 25);
            // 
            // tscbAuthentication
            // 
            this.tscbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbAuthentication.Items.AddRange(new object[] {
            "None",
            "MD5",
            "SHA-1"});
            this.tscbAuthentication.Name = "tscbAuthentication";
            this.tscbAuthentication.Size = new System.Drawing.Size(121, 25);
            // 
            // tstxtAuthentication
            // 
            this.tstxtAuthentication.Name = "tstxtAuthentication";
            this.tstxtAuthentication.Size = new System.Drawing.Size(100, 25);
            // 
            // tscbPrivacy
            // 
            this.tscbPrivacy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbPrivacy.Items.AddRange(new object[] {
            "None",
            "DES",
            "AES"});
            this.tscbPrivacy.Name = "tscbPrivacy";
            this.tscbPrivacy.Size = new System.Drawing.Size(121, 25);
            // 
            // tstxtPrivacy
            // 
            this.tstxtPrivacy.Name = "tstxtPrivacy";
            this.tstxtPrivacy.Size = new System.Drawing.Size(100, 25);
            // 
            // tsbtnAnalyze
            // 
            this.tsbtnAnalyze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnAnalyze.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnAnalyze.Image")));
            this.tsbtnAnalyze.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnAnalyze.Name = "tsbtnAnalyze";
            this.tsbtnAnalyze.Size = new System.Drawing.Size(52, 22);
            this.tsbtnAnalyze.Text = "Analyze";
            this.tsbtnAnalyze.Click += new System.EventHandler(this.txtBytes_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 494);
            this.Controls.Add(this.tvMessage);
            this.Controls.Add(this.txtBytes);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBytes;
        private System.Windows.Forms.TreeView tvMessage;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tstxtUser;
        private System.Windows.Forms.ToolStripComboBox tscbAuthentication;
        private System.Windows.Forms.ToolStripTextBox tstxtAuthentication;
        private System.Windows.Forms.ToolStripComboBox tscbPrivacy;
        private System.Windows.Forms.ToolStripTextBox tstxtPrivacy;
        private System.Windows.Forms.ToolStripButton tsbtnAnalyze;
    }
}

