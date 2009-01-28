/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/20
 * Time: 20:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Mib;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of ModuleListPanel.
	/// </summary>
	public partial class ModuleListPanel : DockContent
	{
	    private IObjectRegistry _objects;

	    public ModuleListPanel()
		{
			InitializeComponent();
			
		}
		
		void ModuleListPanel_Load(object sender, EventArgs e)
		{
            Objects.OnChanged += RefreshPanel;
			RefreshPanel(Objects, EventArgs.Empty);
		}

        [Dependency]
	    public IObjectRegistry Objects
	    {
	        get { return _objects; }
	        set { _objects = value; }
	    }

	    private void RefreshPanel(object sender, EventArgs e)
		{
			ObjectRegistry reg = (ObjectRegistry)sender;
			SuspendLayout();
			listView1.Items.Clear();
			foreach (string module in reg.Tree.LoadedModules)
			{
				ListViewItem item = listView1.Items.Add(module);
				item.Group = listView1.Groups["lvgLoaded"];
			}
			
			foreach (string pending in reg.Tree.PendingModules)
			{
				ListViewItem item = listView1.Items.Add(pending);
				item.BackColor = Color.LightGray;
				item.Group = listView1.Groups["lvgPending"];
			}
			
			ResumeLayout();
			listView1.Groups["lvgLoaded"].Header = string.Format(CultureInfo.CurrentCulture, "Loaded ({0})", reg.Tree.LoadedModules.Count);
			listView1.Groups["lvgPending"].Header = string.Format(CultureInfo.CurrentCulture, "Pending ({0})", reg.Tree.PendingModules.Count);
			tslblCount.Text = "loaded count: " + reg.Tree.LoadedModules.Count + "; pending count: " + reg.Tree.PendingModules.Count;
		}

		private void actAdd_Execute(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			
			ICollection<string> files = openFileDialog1.FileNames;
			if (files.Count == 0)
			{
				return;
			}
			
			if (!Directory.Exists(Objects.Path))
			{
				Directory.CreateDirectory(Objects.Path);
			}
			
			foreach (string file in files)
			{
				string name = Path.GetFileName(file);
				string destFileName = Path.Combine(Objects.Path, name);
				if (File.Exists(destFileName))
				{
					TraceSource source = new TraceSource("Browser");
					source.TraceInformation("File already exists: " + name);
					source.Flush();
					source.Close();
				}
				else
				{
					File.Copy(file, destFileName);
				}
			}
			
			Objects.Refresh();
		}

		private void actRemove_Execute(object sender, EventArgs e)
		{
			TraceSource source = new TraceSource("Browser");
			source.TraceInformation("Deletion is not yet implemented: " + listView1.SelectedItems[0].Text);
			source.Flush();
			source.Close();
		}

		private void actRemove_Update(object sender, EventArgs e)
		{
			actRemove.Enabled = listView1.SelectedItems.Count == 1;
		}

		private void listView1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				contextModuleMenu.Show(listView1, e.Location);
			}
		}
	}
}
