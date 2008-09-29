namespace Lextm.SharpSnmpLib.Browser
{
    partial class FormProfile
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
        	this.components = new System.ComponentModel.Container();
        	this.label1 = new System.Windows.Forms.Label();
        	this.txtIP = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.label3 = new System.Windows.Forms.Label();
        	this.txtSet = new System.Windows.Forms.TextBox();
        	this.txtGet = new System.Windows.Forms.TextBox();
        	this.cbVersionCode = new System.Windows.Forms.ComboBox();
        	this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
        	this.txtPort = new System.Windows.Forms.TextBox();
        	this.btnOK = new System.Windows.Forms.Button();
        	this.btnCancel = new System.Windows.Forms.Button();
        	this.label5 = new System.Windows.Forms.Label();
        	this.label6 = new System.Windows.Forms.Label();
        	this.txtName = new System.Windows.Forms.TextBox();
        	this.groupBox1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(32, 74);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(17, 12);
        	this.label1.TabIndex = 0;
        	this.label1.Text = "IP";
        	// 
        	// txtIP
        	// 
        	this.txtIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.txtIP.Location = new System.Drawing.Point(93, 71);
        	this.txtIP.Name = "txtIP";
        	this.txtIP.Size = new System.Drawing.Size(100, 21);
        	this.txtIP.TabIndex = 1;
        	this.txtIP.Text = "127.0.0.1";
        	this.txtIP.Validated += new System.EventHandler(this.txtIP_Validated);
        	this.txtIP.Validating += new System.ComponentModel.CancelEventHandler(this.txtIP_Validating);
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(32, 14);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(29, 12);
        	this.label2.TabIndex = 2;
        	this.label2.Text = "SNMP";
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.groupBox1.Controls.Add(this.label4);
        	this.groupBox1.Controls.Add(this.label3);
        	this.groupBox1.Controls.Add(this.txtSet);
        	this.groupBox1.Controls.Add(this.txtGet);
        	this.groupBox1.Location = new System.Drawing.Point(12, 139);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(200, 86);
        	this.groupBox1.TabIndex = 3;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Community Names";
        	// 
        	// label4
        	// 
        	this.label4.AutoSize = true;
        	this.label4.Location = new System.Drawing.Point(20, 53);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(23, 12);
        	this.label4.TabIndex = 3;
        	this.label4.Text = "SET";
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(20, 29);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(23, 12);
        	this.label3.TabIndex = 2;
        	this.label3.Text = "GET";
        	// 
        	// txtSet
        	// 
        	this.txtSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.txtSet.Location = new System.Drawing.Point(81, 50);
        	this.txtSet.Name = "txtSet";
        	this.txtSet.Size = new System.Drawing.Size(100, 21);
        	this.txtSet.TabIndex = 1;
        	this.txtSet.Text = "private";
        	this.txtSet.Validated += new System.EventHandler(this.txtSet_Validated);
        	this.txtSet.Validating += new System.ComponentModel.CancelEventHandler(this.txtSet_Validating);
        	// 
        	// txtGet
        	// 
        	this.txtGet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.txtGet.Location = new System.Drawing.Point(81, 26);
        	this.txtGet.Name = "txtGet";
        	this.txtGet.Size = new System.Drawing.Size(100, 21);
        	this.txtGet.TabIndex = 0;
        	this.txtGet.Text = "public";
        	this.txtGet.Validated += new System.EventHandler(this.txtGet_Validated);
        	this.txtGet.Validating += new System.ComponentModel.CancelEventHandler(this.txtGet_Validating);
        	// 
        	// cbVersionCode
        	// 
        	this.cbVersionCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.cbVersionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.cbVersionCode.FormattingEnabled = true;
        	this.cbVersionCode.Items.AddRange(new object[] {
        	        	        	"v1",
        	        	        	"v2c",
        	        	        	"v3"});
        	this.cbVersionCode.Location = new System.Drawing.Point(93, 11);
        	this.cbVersionCode.Name = "cbVersionCode";
        	this.cbVersionCode.Size = new System.Drawing.Size(100, 20);
        	this.cbVersionCode.TabIndex = 0;
        	// 
        	// errorProvider1
        	// 
        	this.errorProvider1.ContainerControl = this;
        	// 
        	// txtPort
        	// 
        	this.txtPort.Location = new System.Drawing.Point(93, 101);
        	this.txtPort.Name = "txtPort";
        	this.txtPort.Size = new System.Drawing.Size(100, 21);
        	this.txtPort.TabIndex = 2;
        	this.txtPort.Text = "161";
        	this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtPort_Validating);
        	// 
        	// btnOK
        	// 
        	this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.btnOK.Location = new System.Drawing.Point(24, 231);
        	this.btnOK.Name = "btnOK";
        	this.btnOK.Size = new System.Drawing.Size(75, 23);
        	this.btnOK.TabIndex = 3;
        	this.btnOK.Text = "OK";
        	this.btnOK.UseVisualStyleBackColor = true;
        	this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
        	// 
        	// btnCancel
        	// 
        	this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.btnCancel.CausesValidation = false;
        	this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btnCancel.Location = new System.Drawing.Point(121, 231);
        	this.btnCancel.Name = "btnCancel";
        	this.btnCancel.Size = new System.Drawing.Size(75, 23);
        	this.btnCancel.TabIndex = 4;
        	this.btnCancel.Text = "Cancel";
        	this.btnCancel.UseVisualStyleBackColor = true;
        	// 
        	// label5
        	// 
        	this.label5.Location = new System.Drawing.Point(32, 103);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(49, 23);
        	this.label5.TabIndex = 7;
        	this.label5.Text = "Port";
        	// 
        	// label6
        	// 
        	this.label6.AutoSize = true;
        	this.label6.Location = new System.Drawing.Point(32, 44);
        	this.label6.Name = "label6";
        	this.label6.Size = new System.Drawing.Size(29, 12);
        	this.label6.TabIndex = 8;
        	this.label6.Text = "Name";
        	// 
        	// txtName
        	// 
        	this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.txtName.Location = new System.Drawing.Point(93, 42);
        	this.txtName.Name = "txtName";
        	this.txtName.Size = new System.Drawing.Size(100, 21);
        	this.txtName.TabIndex = 9;
        	// 
        	// FormProfile
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(225, 265);
        	this.Controls.Add(this.txtName);
        	this.Controls.Add(this.label6);
        	this.Controls.Add(this.txtPort);
        	this.Controls.Add(this.label5);
        	this.Controls.Add(this.btnCancel);
        	this.Controls.Add(this.btnOK);
        	this.Controls.Add(this.cbVersionCode);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.txtIP);
        	this.Controls.Add(this.label1);
        	this.Name = "FormProfile";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        	this.Text = "Profile Editor";
        	this.Load += new System.EventHandler(this.FormProfile_Load);
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSet;
        private System.Windows.Forms.TextBox txtGet;
        private System.Windows.Forms.ComboBox cbVersionCode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
    }
}