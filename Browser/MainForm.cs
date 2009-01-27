/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

		    OutputPanel output = (OutputPanel) Program.OutputPanel;
            output.Show(dockPanel1, DockState.DockBottom);

            MibTreePanel tree = new MibTreePanel();
            tree.Show(dockPanel1, DockState.Document);
            ModuleListPanel modules = new ModuleListPanel();
            modules.Show(dockPanel1, DockState.DockLeft);

            AgentProfilePanel agent = new AgentProfilePanel();
            agent.Show(dockPanel1, DockState.DockLeft);            
		}

        private void actExit_Execute(object sender, EventArgs e)
        {
            Close();
        }
    }
}
