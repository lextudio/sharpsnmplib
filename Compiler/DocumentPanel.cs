using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Lextm.SharpSnmpLib.Compiler
{
    internal partial class DocumentPanel : DockContent
    {
        private string _fileName;
        private IMediator _mediator;

        public DocumentPanel(IMediator mediator, string fileName)
        {
            InitializeComponent();
            _mediator = mediator;
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
            _mediator.CloseAllDocuments();
        }
    }
}
