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
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    internal partial class MainForm : Form
    {
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

        public CompilerCore Compiler { get; set; }

        private void ActExitExecute(object sender, EventArgs e)
        {
            Close();
        }

        private void ActOpenExecute(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dockPanel1.SuspendLayout(true);
            Compiler.Add(openFileDialog1.FileNames);

            dockPanel1.ResumeLayout(true, true);
        }

        private void ActCompileExecute(object sender, EventArgs e)
        {
            IDockContent content = dockPanel1.ActiveDocument;
            DocumentPanel doc = (DocumentPanel)content;

            List<string> fileList = new List<string>(1) {doc.FileName};

            Compiler.Compile(fileList);
        }

        private void ActCompileUpdate(object sender, EventArgs e)
        {
            actCompile.Enabled = dockPanel1.ActiveDocument != null && !Compiler.IsBusy;
        }

        private void ActCompileAllExecute(object sender, EventArgs e)
        {
            Compiler.CompileAll();
        }

        private void ActCompileAllUpdate(object sender, EventArgs e)
        {
            actCompileAll.Enabled = dockPanel1.DocumentsCount > 0 && !Compiler.IsBusy;
        }
        
        private void ActAboutExecute(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "http://sharpsnmplib.codeplex.com");
        }
    }
}
