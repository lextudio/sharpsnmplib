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
using System.IO;
using System.Media;
using System.Net;
using System.Windows.Forms;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form, IMediator
	{
        private OutputPanel _output;
        private ModuleListPanel _modules; 
        private Assembler _assembler;
        private string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules");

		public MainForm()
		{
			InitializeComponent();

            _assembler = new Assembler(root);

            _output = new OutputPanel();
            _output.Show(dockPanel1, DockState.DockBottom);
                        
            _modules = new ModuleListPanel(this);
            _modules.Show(dockPanel1, DockState.DockLeft);
		}

        private void actExit_Execute(object sender, EventArgs e)
        {
            Close();
        }

        #region IMediator Members

        public void CloseAllDocuments()
        {
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                IDockContent[] documents = dockPanel1.DocumentsToArray();
                foreach (IDockContent content in documents)
                    content.DockHandler.Close();
            }
        }

        public void OpenDocument(string fileName)
        {
            DocumentPanel doc = new DocumentPanel(this, fileName);
            doc.Show(dockPanel1, DockState.Document);
        }

        public IOutput Output
        {
            get { return _output; }
        }

        public IObjectTree Tree
        {
            get { return _assembler.Tree; }
        }

        public string Root
        {
            get { return root; }
        }

        #endregion

        private void actOpen_Execute(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dockPanel1.SuspendLayout(true);
                int limit = 100;
                foreach (string file in openFileDialog1.FileNames)
                {
                    if (FindDocument(file) != null)
                    {
                        Output.ReportMessage("The document: " + file + " has already opened!");
                        continue;
                    }

                    OpenDocument(file);
                    limit--;
                    if (limit == 0) 
                    {
                        Output.ReportMessage("Only 100 files can be opened at a time.");
                        SystemSounds.Asterisk.Play();
                        break;
                    }
                }
                dockPanel1.ResumeLayout(true, true);
            }
        }

        private void actCompile_Execute(object sender, EventArgs e)
        {
            IDockContent content = dockPanel1.ActiveDocument;
            DocumentPanel doc = (DocumentPanel)content;

            backgroundWorker1.RunWorkerAsync(new List<string>(1) { doc.FileName });
        }

        private void actCompile_Update(object sender, EventArgs e)
        {
            actCompile.Enabled = dockPanel1.ActiveDocument != null && !backgroundWorker1.IsBusy;
        }

        private void actCompileAll_Execute(object sender, EventArgs e)
        {
            var docs = new List<string>(dockPanel1.DocumentsCount);
            foreach (IDockContent content in dockPanel1.Documents)
            {
                DocumentPanel doc = (DocumentPanel)content;
                docs.Add(doc.FileName);
            }           
            
            backgroundWorker1.RunWorkerAsync(docs);
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
                    if (form.Text == fileName)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel1.Documents)
                    if (content.DockHandler.TabText == fileName)
                        return content;

                return null;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var docs = (IEnumerable<string>)e.Argument;
            Parser parser = new Parser(_assembler);
            parser.ParseToModules(docs);
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _modules.RefreshPanel(_modules, EventArgs.Empty);
            SystemSounds.Beep.Play();
        }
    }
}
