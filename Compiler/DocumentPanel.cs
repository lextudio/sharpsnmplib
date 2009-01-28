using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
    internal partial class DocumentPanel : DockContent
    {
        private readonly string _fileName;

        public DocumentPanel(string fileName)
        {
            InitializeComponent();
            _fileName = fileName;
            TabText = _fileName;
            txtDocument.LoadFile(_fileName, RichTextBoxStreamType.PlainText);
        }

        public string FileName
        {
            get { return _fileName; }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.CloseAllDocuments(DockPanel);
        }
    }
}
