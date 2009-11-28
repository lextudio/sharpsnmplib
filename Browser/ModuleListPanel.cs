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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Lextm.Common;
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
        private const string Filter = "*.module";
        private IObjectRegistry _objects;
        private FileSystemWatcher _watcher;
        private WatchDog _dog;

        public ModuleListPanel()
        {
            InitializeComponent();
        }
        
        void ModuleListPanel_Load(object sender, EventArgs e)
        {
            Objects.OnChanged += RefreshPanel;
            RefreshPanel(Objects, EventArgs.Empty);

            _dog = new WatchDog(10000d);
            _dog.Bark += DogBark;
            _dog.Enabled = actEnableMonitor.Checked;

            _watcher = new FileSystemWatcher(((ReloadableObjectRegistry)Objects).Path, Filter);
            _watcher.IncludeSubdirectories = false;
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.EnableRaisingEvents = actEnableMonitor.Checked;
        }

        private void DogBark(object sender, EventArgs e)
        {
            ((ReloadableObjectRegistry)Objects).Reload();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _dog.Feed();
        }

        [Dependency]
        public IObjectRegistry Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        private void RefreshPanel(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate { RefreshPanel(sender, e); });
                return;
            }

            ReloadableObjectRegistry reg = (ReloadableObjectRegistry)sender;
            SuspendLayout();
            listView1.Items.Clear();
            List<string> loaded = new List<string>(reg.Tree.LoadedModules);
            loaded.Sort();
            foreach (string module in loaded)
            {
                ListViewItem item = listView1.Items.Add(module);
                item.Group = listView1.Groups["lvgLoaded"];
            }
            
            string[] files = Directory.GetFiles(reg.Path, Filter);
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
            tslblCount.Text = string.Format(CultureInfo.InvariantCulture, "loaded: {0}; unloaded: {1}", listView1.Groups["lvgLoaded"].Items.Count, listView1.Groups["lvgPending"].Items.Count);
        }

        private void ActAddExecute(object sender, EventArgs e)
        {
            ReloadableObjectRegistry reg = (ReloadableObjectRegistry)Objects;
            string index = Path.Combine(reg.Path, "index");
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                string name = item.Text.ToUpperInvariant();
                List<string> list = new List<string>(File.ReadAllLines(index));
                if (!list.Contains(name))
                {
                    list.Add(name);
                    File.WriteAllLines(index, list.ToArray());
                }
            }

            reg.Reload();
        }

        private void ActRemoveExecute(object sender, EventArgs e)
        {
            ReloadableObjectRegistry reg = (ReloadableObjectRegistry)Objects;
            string index = Path.Combine(reg.Path, "index");
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                string name = item.Text.ToUpperInvariant();
                List<string> list = new List<string>(File.ReadAllLines(index));
                if (list.Contains(name))
                {
                    list.Remove(name);
                    File.WriteAllLines(index, list.ToArray());
                }
            }

            reg.Reload();
        }

        private void ActRemoveUpdate(object sender, EventArgs e)
        {
            actRemove.Enabled = listView1.SelectedItems.Count >0 && ItemsInGroup(listView1.SelectedItems, "lvgLoaded");
        }

        private static bool ItemsInGroup(ListView.SelectedListViewItemCollection collection, string group)
        {
            foreach (ListViewItem item in collection)
            {
                if (item.Group.Name != group)
                {
                    return false;
                }
            }

            return true;
        }

        private void ActAddUpdate(object sender, EventArgs e)
        {
            actAdd.Enabled = listView1.SelectedItems.Count > 0 && ItemsInGroup(listView1.SelectedItems, "lvgPending");
        }

        private void ListView1MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextModuleMenu.Show(listView1, e.Location);
            }
        }

        private void ActEnableMonitorExecute(object sender, EventArgs e)
        {
            _dog.Enabled = actEnableMonitor.Checked;
            _watcher.EnableRaisingEvents = actEnableMonitor.Checked;
        }
    }
}
