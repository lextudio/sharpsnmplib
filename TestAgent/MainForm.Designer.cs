/*
 * Created by SharpDevelop.
 * User: lexli
 * Date: 2008-12-14
 * Time: 14:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TestAgent
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
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
        	this.btnStart = new System.Windows.Forms.Button();
        	this.btnStop = new System.Windows.Forms.Button();
        	this.agent1 = new Lextm.SharpSnmpLib.Agent(this.components);
        	this.btnTrap = new System.Windows.Forms.Button();
        	this.btnTrap2 = new System.Windows.Forms.Button();
        	this.txtIP = new System.Windows.Forms.TextBox();
        	this.txtPort = new System.Windows.Forms.TextBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.btnInform = new System.Windows.Forms.Button();
        	this.listener1 = new Lextm.SharpSnmpLib.Listener(this.components);
        	this.label3 = new System.Windows.Forms.Label();
        	this.txtAgentPort = new System.Windows.Forms.TextBox();
        	this.SuspendLayout();
        	// 
        	// btnStart
        	// 
        	this.btnStart.Location = new System.Drawing.Point(30, 54);
        	this.btnStart.Name = "btnStart";
        	this.btnStart.Size = new System.Drawing.Size(75, 23);
        	this.btnStart.TabIndex = 0;
        	this.btnStart.Text = "Start";
        	this.btnStart.UseVisualStyleBackColor = true;
        	this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
        	// 
        	// btnStop
        	// 
        	this.btnStop.Location = new System.Drawing.Point(246, 54);
        	this.btnStop.Name = "btnStop";
        	this.btnStop.Size = new System.Drawing.Size(75, 23);
        	this.btnStop.TabIndex = 1;
        	this.btnStop.Text = "Stop";
        	this.btnStop.UseVisualStyleBackColor = true;
        	this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
        	// 
        	// btnTrap
        	// 
        	this.btnTrap.Location = new System.Drawing.Point(30, 167);
        	this.btnTrap.Name = "btnTrap";
        	this.btnTrap.Size = new System.Drawing.Size(93, 23);
        	this.btnTrap.TabIndex = 2;
        	this.btnTrap.Text = "Send Trap v1";
        	this.btnTrap.UseVisualStyleBackColor = true;
        	this.btnTrap.Click += new System.EventHandler(this.BtnTrapClick);
        	// 
        	// btnTrap2
        	// 
        	this.btnTrap2.Location = new System.Drawing.Point(129, 167);
        	this.btnTrap2.Name = "btnTrap2";
        	this.btnTrap2.Size = new System.Drawing.Size(93, 23);
        	this.btnTrap2.TabIndex = 3;
        	this.btnTrap2.Text = "Send Trap v2";
        	this.btnTrap2.UseVisualStyleBackColor = true;
        	this.btnTrap2.Click += new System.EventHandler(this.BtnTrap2Click);
        	// 
        	// txtIP
        	// 
        	this.txtIP.Location = new System.Drawing.Point(110, 93);
        	this.txtIP.Name = "txtIP";
        	this.txtIP.Size = new System.Drawing.Size(100, 20);
        	this.txtIP.TabIndex = 4;
        	this.txtIP.Text = "127.0.0.1";
        	// 
        	// txtPort
        	// 
        	this.txtPort.Location = new System.Drawing.Point(110, 128);
        	this.txtPort.Name = "txtPort";
        	this.txtPort.Size = new System.Drawing.Size(49, 20);
        	this.txtPort.TabIndex = 5;
        	this.txtPort.Text = "162";
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(30, 96);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(66, 23);
        	this.label1.TabIndex = 6;
        	this.label1.Text = "Manager IP";
        	// 
        	// label2
        	// 
        	this.label2.Location = new System.Drawing.Point(66, 131);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(38, 23);
        	this.label2.TabIndex = 7;
        	this.label2.Text = "Port";
        	// 
        	// btnInform
        	// 
        	this.btnInform.Location = new System.Drawing.Point(228, 167);
        	this.btnInform.Name = "btnInform";
        	this.btnInform.Size = new System.Drawing.Size(93, 23);
        	this.btnInform.TabIndex = 8;
        	this.btnInform.Text = "Send Inform";
        	this.btnInform.UseVisualStyleBackColor = true;
        	this.btnInform.Click += new System.EventHandler(this.BtnInformClick);
        	// 
        	// listener1
        	// 
        	this.listener1.GetRequestReceived += new System.EventHandler<Lextm.SharpSnmpLib.MessageReceivedEventArgs<Lextm.SharpSnmpLib.GetRequestMessage>>(this.agent1_GetRequestReceived);
        	this.listener1.ExceptionRaised += new System.EventHandler<Lextm.SharpSnmpLib.ExceptionRaisedEventArgs>(this.agent1_ExceptionRaised);
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(30, 13);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(57, 13);
        	this.label3.TabIndex = 9;
        	this.label3.Text = "Agent Port";
        	// 
        	// txtAgentPort
        	// 
        	this.txtAgentPort.Location = new System.Drawing.Point(110, 10);
        	this.txtAgentPort.Name = "txtAgentPort";
        	this.txtAgentPort.Size = new System.Drawing.Size(100, 20);
        	this.txtAgentPort.TabIndex = 10;
        	this.txtAgentPort.Text = "161";
        	// 
        	// MainForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(353, 208);
        	this.Controls.Add(this.txtAgentPort);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.btnInform);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.txtPort);
        	this.Controls.Add(this.txtIP);
        	this.Controls.Add(this.btnTrap2);
        	this.Controls.Add(this.btnTrap);
        	this.Controls.Add(this.btnStop);
        	this.Controls.Add(this.btnStart);
        	this.Name = "MainForm";
        	this.Text = "TestAgent";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Button btnInform;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnTrap2;
        private System.Windows.Forms.Button btnTrap;

        private Lextm.SharpSnmpLib.Agent agent1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private Lextm.SharpSnmpLib.Listener listener1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAgentPort;
    }
}
