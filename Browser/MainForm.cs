/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

            OutputPanel output = new OutputPanel();
            output.Show(dockPanel1, DockState.DockBottom);
            MibTreePanel tree = new MibTreePanel();
            tree.Show(dockPanel1, DockState.Document);
            ModuleListPanel modules = new ModuleListPanel();
            modules.Show(dockPanel1, DockState.DockLeft);
            
            AgentProfile first = new AgentProfile("127.0.0.1", VersionCode.V1, "public", "public");
            first.OnOperationCompleted += output.ReportMessage;
            ProfileRegistry.AddProfile(first);
            ProfileRegistry.Default = "127.0.0.1";

            foreach (string name in ProfileRegistry.Names)
            {
                tscbAgent.Items.Add(name);
            }
            tscbAgent.SelectedIndex = 0;
		}

        private void actExit_Execute(object sender, EventArgs e)
        {
            Close();
        }

        private void actConfigure_Execute(object sender, EventArgs e)
        {
            AgentProfilePanel agent = new AgentProfilePanel();
            agent.Show(dockPanel1, DockState.DockLeft);
        }
    }
}
