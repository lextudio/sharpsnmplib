using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Mib;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Browser
{
    internal partial class FormTable : Form
    {
        private readonly IDefinition _definition;
        private bool _columnCountSet;
        private Thread _refreshThread;

        delegate void RefreshTableCallback(IList<Variable> list);

        public FormTable(IDefinition def)
        {
            _definition = def;
            InitializeComponent();
            cbColumnDisplay.SelectedIndex = 1;
        }

        public void SetRows(int rowCount)
        {
            dataGridTable.RowCount = rowCount;
        }

        public void PopulateGrid(IList<Variable> list)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (dataGridTable.InvokeRequired)
            {
                RefreshTableCallback r =  PopulateGrid;
                Invoke(r, new object[] { list });
            }
            else 
            {
                int rowCount = dataGridTable.RowCount;

                dataGridTable.ColumnCount = (list.Count / dataGridTable.RowCount);
                dataGridTable.RowCount = 0;
                _columnCountSet = true;

                //
                // Create the column headers
                //
                CreateColumns();

                //
                // Fill in the tree itself
                //
                for (int x = 0; x < rowCount; x++)
                {
                    string[] row = new string[dataGridTable.ColumnCount];

                    for (int y = 0; y < dataGridTable.ColumnCount; y++)
                    {
                        row[y] = list[(y * rowCount) + x].Data.ToString();
                    }

                    dataGridTable.Rows.Add(row);

                    Refresh();
                }
            }
        }

        private void checkBoxRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxRefresh.Checked)
            {
                _refreshThread = new Thread(RefreshTable);
                _refreshThread.Start();
            }
            else
            {
                _refreshThread.Abort();
            }
        }

        internal void CreateColumns()
        {
            int x = 0;
            
            //
            // We want to move into the table object here
            //
            IEnumerator<IDefinition> i = _definition.Children.GetEnumerator();
            i.Reset();
            i.MoveNext();
            IDefinition d = i.Current;


            foreach (IDefinition def in d.Children)
            {
                dataGridTable.Columns[x++].Name = cbColumnDisplay.SelectedIndex == 0 ? def.TextualForm : def.Name;
            }
        }

        private void cbColumnDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_columnCountSet)
            {
                CreateColumns();
                Refresh();
            }
        }

        public void RefreshTable()
        {
            // TODO: how to get rid of infinite loop?
            // will use WatchDog class to optimize this.
            while (true)
            {
                IProfileRegistry registry = Program.Container.Resolve<IProfileRegistry>();
                if (registry == null)
                {
                    break;
                }

                AgentProfile prof = registry.DefaultProfile;
                IList<Variable> list = new List<Variable>();
                Thread.Sleep(Convert.ToInt32(textBoxRefresh.Text, CultureInfo.CurrentCulture) * 1000);
                int rows = Messenger.Walk(prof.VersionCode, prof.Agent, new OctetString(prof.GetCommunity), new ObjectIdentifier(_definition.GetNumericalForm()), list, 1000, WalkMode.WithinSubtree);

                SetRows(rows);
                PopulateGrid(list);
            }
        }

        private void textBoxRefresh_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue > 0 && e.KeyValue < 31) || (e.KeyValue > 47 && e.KeyValue < 58) || (e.KeyValue > 95 && e.KeyValue < 106))
            {
                //Allow all numbers to be printed
            }
            else
            {
                e.SuppressKeyPress = true;
            }
        }

        private void FormTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_refreshThread != null)
            {
                _refreshThread.Abort();
            }
        }
    }
}
