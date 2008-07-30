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
using System.Net;
using System.Windows.Forms;

using Lextm.SharpSnmpLib;
using WeifenLuo.WinFormsUI.Docking;

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
            
            IPAddress def = IPAddress.Parse("127.0.0.1");
            AgentProfile first = new AgentProfile(def, VersionCode.V1, "public", "public");
            first.OnOperationCompleted += output.ReportMessage;
            ProfileRegistry.AddProfile(first);
            ProfileRegistry.Default = def;

            foreach (IPAddress name in ProfileRegistry.Names)
            {
                tscbAgent.Items.Add(name.ToString());
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
