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
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Microsoft.Practices.Unity;

namespace Lextm.SharpSnmpLib.Compiler
{

	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form
	{
	    private CompilerCore _compiler;

		public MainForm()
		{
			InitializeComponent();

			DockContent files = Program.Container.Resolve<DockContent>("DocumentList");
			files.Show(dockPanel1, DockState.DockLeft);

			DockContent output = Program.Container.Resolve<DockContent>("Output");
    		output.Show(dockPanel1, DockState.DockBottom);

			DockContent modules = Program.Container.Resolve<DockContent>("ModuleList");
    		modules.Show(dockPanel1, DockState.DockRight);
		}

        [Dependency]
	    public CompilerCore Compiler
	    {
	        get { return _compiler; }
	        set { _compiler = value; }
	    }

	    private void actExit_Execute(object sender, EventArgs e)
		{
			Close();
		}

		private void actOpen_Execute(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			dockPanel1.SuspendLayout(true);
		    Compiler.Add(openFileDialog1.FileNames);

			dockPanel1.ResumeLayout(true, true);
		}

		private void actCompile_Execute(object sender, EventArgs e)
		{
			IDockContent content = dockPanel1.ActiveDocument;
			DocumentPanel doc = (DocumentPanel)content;

			List<string> sList = new List<string>(1);
			sList.Add(doc.FileName);

			Compiler.Compile(sList);
		}

		private void actCompile_Update(object sender, EventArgs e)
		{
			actCompile.Enabled = dockPanel1.ActiveDocument != null && !Compiler.IsBusy;
		}

		private void actCompileAll_Execute(object sender, EventArgs e)
		{
			Compiler.CompileAll();
		}

		private void actCompileAll_Update(object sender, EventArgs e)
		{
			actCompileAll.Enabled = dockPanel1.DocumentsCount > 0 && !Compiler.IsBusy;
		}
	}
}
