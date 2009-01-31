/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 18:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
	/// <summary>
	/// Description of DocumentListPanel.
	/// </summary>
	internal partial class DocumentListPanel : DockContent
	{
	    private CompilerCore _compiler;

	    public DocumentListPanel()
		{
			InitializeComponent();
		}
		
        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            string fileName = lvFiles.SelectedItems[0].Tag.ToString();
            OpenDocument(fileName);
        }
        
        public void OpenDocument(string fileName)
		{
            IDockContent opened = FindDocument(fileName);
			if (opened != null)
			{
			    opened.DockHandler.Show();
				TraceSource source = new TraceSource("Compiler");
				source.TraceInformation("The document: " + fileName + " has already opened!");
				source.Flush();
				source.Close();
				return;
			}
			
			DocumentPanel doc = new DocumentPanel(fileName);
			doc.Show(DockPanel, DockState.Document);
		}
		
		private IDockContent FindDocument(string fileName)
		{
			if (DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
			{
				throw new InvalidOperationException("cannot be System MDI");
			}

			foreach (IDockContent content in DockPanel.Documents)
			{
				if (content.DockHandler.TabText == fileName)
				{
					return content;
				}
			}

			return null;
		}

        private void DocumentListPanel_Load(object sender, EventArgs e)
        {
            Compiler.FileAdded += FileOpen;
        }

	    private void FileOpen(object sender, FileAddedEventArgs e)
	    {
	        foreach (string file in e.Files)
	        {
	            ListViewItem item = lvFiles.Items.Add(Path.GetFileName(file));
	            item.Tag = file;
	        }

            if (e.Files.Count > 0)
            {
                OpenDocument(e.Files[0]);
            }
	    }

	    [Dependency]
	    public CompilerCore Compiler
	    {
            get { return _compiler; }
            set { _compiler = value; }
	    }
    }
}
