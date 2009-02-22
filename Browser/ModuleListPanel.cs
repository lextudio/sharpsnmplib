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
	internal partial class ModuleListPanel : DockContent
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
			List<string> loaded = new List<string>(reg.Tree.LoadedModules);
			loaded.Sort();
			foreach (string module in loaded)
			{
				ListViewItem item = listView1.Items.Add(module);
				item.Group = listView1.Groups["lvgLoaded"];
			}
			
			string[] files = Directory.GetFiles(reg.Path, "*.module");
			foreach (string file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);
				if (loaded.Contains(name))
				{
					continue;
				}
				
				ListViewItem item = listView1.Items.Add(name);
				item.BackColor = Color.LightGray;
				item.Group = listView1.Groups["lvgPending"];
			}
			
			ResumeLayout();
			listView1.Groups["lvgLoaded"].Header = string.Format(CultureInfo.CurrentCulture, "Loaded ({0})", listView1.Groups["lvgLoaded"].Items.Count);
			listView1.Groups["lvgPending"].Header = string.Format(CultureInfo.CurrentCulture, "Unloaded ({0})", listView1.Groups["lvgPending"].Items.Count);
			tslblCount.Text = "loaded count: " + listView1.Groups["lvgLoaded"].Items.Count + "; unloaded count: " + listView1.Groups["lvgPending"].Items.Count;
    	}

		private void actAdd_Execute(object sender, EventArgs e)
		{
			string name = listView1.SelectedItems[0].Text.ToUpperInvariant();
			string index = Path.Combine(Objects.Path, "index");
			List<string> list = new List<string>(File.ReadAllLines(index));
			if (!list.Contains(name))
			{
				list.Add(name);
				File.WriteAllLines(index, list.ToArray());
			}
			
			Objects.Reload();
		}

		private void actRemove_Execute(object sender, EventArgs e)
		{
			string name = listView1.SelectedItems[0].Text.ToUpperInvariant();
			string index = Path.Combine(Objects.Path, "index");
			List<string> list = new List<string>(File.ReadAllLines(index));
			if (list.Contains(name))
			{
				list.Remove(name);
				File.WriteAllLines(index, list.ToArray());
			}
			
			Objects.Reload();
		}

		private void actRemove_Update(object sender, EventArgs e)
		{
			actRemove.Enabled = listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].Group.Name == "lvgLoaded";
		}
		
	    private void actAdd_Update(object sender, EventArgs e)
		{
			actAdd.Enabled = listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].Group.Name == "lvgPending";
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
