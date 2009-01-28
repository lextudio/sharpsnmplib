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
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Mib;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form
	{
		private readonly ModuleListPanel modules;
		private readonly DocumentListPanel files;

		public MainForm()
		{
			InitializeComponent();

			files = (DocumentListPanel) Program.Container.Resolve<DockContent>("DocumentList");
			files.Show(dockPanel1, DockState.DockLeft);

			DockContent output = Program.Container.Resolve<DockContent>("Output");
			if (output != null)
			{
				output.Show(dockPanel1, DockState.DockBottom);
			}

			modules = (ModuleListPanel) Program.Container.Resolve<DockContent>("ModuleList");
			if (modules != null)
			{
				modules.Show(dockPanel1, DockState.DockRight);
			}
		}

		private void actExit_Execute(object sender, EventArgs e)
		{
			Close();
		}

		public static void CloseAllDocuments(DockPanel panel)
		{
			IDockContent[] documents = panel.DocumentsToArray();
			foreach (IDockContent content in documents)
			{
				content.DockHandler.Close();
			}
		}

		public void OpenDocument(string fileName)
		{
			if (FindDocument(fileName) != null)
			{
				TraceSource source = new TraceSource("Compiler");
				source.TraceInformation("The document: " + fileName + " has already opened!");
				source.Flush();
				source.Close();
				return;
			}
			
			DocumentPanel doc = new DocumentPanel(fileName);
			doc.Show(dockPanel1, DockState.Document);
		}

		private void actOpen_Execute(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			dockPanel1.SuspendLayout(true);
			foreach (string file in openFileDialog1.FileNames)
			{
				files.Add(file);
			}

			dockPanel1.ResumeLayout(true, true);
		}

		private void actCompile_Execute(object sender, EventArgs e)
		{
			IDockContent content = dockPanel1.ActiveDocument;
			DocumentPanel doc = (DocumentPanel)content;

			List<string> sList = new List<string>(1);
			sList.Add(doc.FileName);

			backgroundWorker1.RunWorkerAsync(sList);
		}

		private void actCompile_Update(object sender, EventArgs e)
		{
			actCompile.Enabled = dockPanel1.ActiveDocument != null && !backgroundWorker1.IsBusy;
		}

		private void actCompileAll_Execute(object sender, EventArgs e)
		{	
			backgroundWorker1.RunWorkerAsync(files.Files);
		}

		private void actCompileAll_Update(object sender, EventArgs e)
		{
			actCompileAll.Enabled = dockPanel1.DocumentsCount > 0 && !backgroundWorker1.IsBusy;
		}

		private IDockContent FindDocument(string fileName)
		{
			if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
			{
				foreach (Form form in MdiChildren)
				{
					if (form.Text == fileName)
					{
						return form as IDockContent;
					}
				}

				return null;
			}

			foreach (IDockContent content in dockPanel1.Documents)
			{
				if (content.DockHandler.TabText == fileName)
				{
					return content;
				}
			}

			return null;
		}

		private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			Parser parser = Program.Container.Resolve<Parser>();
			if (parser == null)
			{
				return;
			}

			IEnumerable<string> docs = (IEnumerable<string>)e.Argument;
			try
			{
				parser.ParseToModules(docs);
			}
			catch (SharpMibException ex)
			{
				// on compiling errors
				backgroundWorker1.ReportProgress(-1, ex);
			}
		}

		private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage != -1)
			{
				return;
			}

			TraceSource source = new TraceSource("Compiler");
			source.TraceInformation(((Exception)e.UserState).Message);
			source.Flush();
			source.Close();
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			if (modules != null)
			{
				modules.RefreshPanel(modules, EventArgs.Empty);
			}

			SystemSounds.Beep.Play();
		}
	}
}
