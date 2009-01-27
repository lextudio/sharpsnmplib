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
using System.IO;
using System.Media;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Mib;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form, IMediator
	{
        private readonly OutputPanel _output;
        private readonly ModuleListPanel _modules; 
        private readonly Assembler _assembler;
        private readonly string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules");

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
            List<string> docs = new List<string>(dockPanel1.DocumentsCount);
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
            IEnumerable<string> docs = (IEnumerable<string>)e.Argument;
            Parser parser = new Parser(_assembler);
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
            if (e.ProgressPercentage == -1)
            {
                _output.ReportMessage(((Exception)e.UserState).Message);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _modules.RefreshPanel(_modules, EventArgs.Empty);
            SystemSounds.Beep.Play();
        }
    }
}
