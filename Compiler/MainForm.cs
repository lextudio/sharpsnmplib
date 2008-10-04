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

        public void OpenDocument(string fileName)
        {
            DocumentPanel doc = new DocumentPanel(fileName);
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
            Parser parser = new Parser(_assembler);
            parser.ParseToModules(new List<string>(1) { doc.FileName });
            _modules.RefreshPanel(_modules, EventArgs.Empty);
            SystemSounds.Beep.Play();
        }

        private void actCompile_Update(object sender, EventArgs e)
        {
            actCompile.Enabled = dockPanel1.ActiveDocument != null;
        }

        private void actCompileAll_Execute(object sender, EventArgs e)
        {
            var docs = new List<string>(dockPanel1.DocumentsCount);
            foreach (IDockContent content in dockPanel1.Documents)
            {
                DocumentPanel doc = (DocumentPanel)content;
                docs.Add(doc.FileName);
            }
            
            Parser parser = new Parser(_assembler);
            parser.ParseToModules(docs);
            _modules.RefreshPanel(_modules, EventArgs.Empty);
            SystemSounds.Beep.Play();
        }

        private void actCompileAll_Update(object sender, EventArgs e)
        {
            actCompileAll.Enabled = dockPanel1.DocumentsCount > 0;
        }

        private void actCloseAll_Execute(object sender, EventArgs e)
        {
            CloseAllDocuments();
        }

        private void CloseAllDocuments()
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

    }
}
