using System;
using System.IO;
using ICSharpCode.TextEditor.Document;
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
            txtDocument.SetHighlighting("SMI"); // Activate the highlighting, use the name from the SyntaxDefinition node.
            txtDocument.LoadFile(_fileName);
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
            
            IDockContent[] documents = panel.DocumentsToArray();
            foreach (IDockContent content in documents)
            {
                content.DockHandler.Close();
            }
        }
    }
}
