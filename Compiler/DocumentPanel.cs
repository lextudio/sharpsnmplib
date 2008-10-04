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
    public partial class DocumentPanel : DockContent
    {
        private string _fileName;

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
    }
}
