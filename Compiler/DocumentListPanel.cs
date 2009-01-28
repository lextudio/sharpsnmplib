/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 18:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
	/// <summary>
	/// Description of DocumentListPanel.
	/// </summary>
	internal partial class DocumentListPanel : DockContent
	{
		private IList<string> names = new List<string>();
		
		public DocumentListPanel()
		{
			InitializeComponent();
		}
		
		public IEnumerable<string> Files
		{
			get { return names; }
		}

        public void Add(string file)
        {
        	if (names.Contains(file))
        	{
        		return;
        	}
        	
            ListViewItem item = lvFiles.Items.Add(Path.GetFileName(file));
            item.Tag = file;
            names.Add(file);
        }

        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            string fileName = lvFiles.SelectedItems[0].Tag.ToString();
            MainForm main = (MainForm) DockPanel.TopLevelControl;
            main.OpenDocument(fileName);
        }
    }
}
