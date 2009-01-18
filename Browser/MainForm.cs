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
    internal interface IMediator
    {
        ProfileRegistry Profiles
        {
            get;
        }
    }
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form, IMediator
	{
	    AgentProfilePanel agent = new AgentProfilePanel();
	    
	    public ProfileRegistry Profiles
	    {
	        get { return agent.Profiles; }
	    }
	    
		public MainForm()
		{
			InitializeComponent();

            OutputPanel output = new OutputPanel();
            output.Show(dockPanel1, DockState.DockBottom);
            OutputPanelTraceListener.Panel = output;

            MibTreePanel tree = new MibTreePanel();
            tree.Show(dockPanel1, DockState.Document);
            ModuleListPanel modules = new ModuleListPanel();
            modules.Show(dockPanel1, DockState.DockLeft);
            
            agent.Show(dockPanel1, DockState.DockLeft);            
		}

        private void actExit_Execute(object sender, EventArgs e)
        {
            Close();
        }
    }
}
