using System;
using System.Windows.Forms;
using RemObjects.Mono.Helpers;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
    internal partial class DocumentPanel : DockContent
    {
        private readonly string _fileName;

        public DocumentPanel(string fileName)
        {
            InitializeComponent();
            if (PlatformSupport.Platform == PlatformType.Windows)
            {
                Icon = Properties.Resources.document_properties;
            }

            _fileName = fileName;
            TabText = _fileName;
            txtDocument.LoadFile(_fileName, RichTextBoxStreamType.PlainText);
        }

        public string FileName
        {
            get { return _fileName; }
        }

        private void CloseAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            CloseAllDocuments(DockPanel);
        }
        
        private static void CloseAllDocuments(DockPanel panel)
        {
            if (panel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                throw new InvalidOperationException("cannot work in System MDI mode");
            }
            
            var documents = panel.DocumentsToArray();
            foreach (var content in documents)
            {
                content.DockHandler.Close();
            }
        }
    }
}
