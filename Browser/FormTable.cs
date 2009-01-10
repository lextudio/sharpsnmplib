using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Browser
{
    public partial class FormTable : Form
    {
        private IDefinition definition;
        private bool columnCountSet;
        private Thread refreshThread;

        delegate void RefreshTableCallback(IList<Variable> list);

        [CLSCompliant(false)]
        public FormTable(IDefinition def)
        {
            definition = def;
            InitializeComponent();
            cbColumnDisplay.SelectedIndex = 1;
        }

        public void setRows(int rowCount)
        {
            dataGridTable.RowCount = rowCount;
        }

        public void populateGrid(IList<Variable> list)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridTable.InvokeRequired)
            {
                RefreshTableCallback r = new RefreshTableCallback(populateGrid);
                this.Invoke(r, new object[] { list });
            }
            else 
            {
                int rowCount = dataGridTable.RowCount;

                dataGridTable.ColumnCount = (list.Count / dataGridTable.RowCount);
                dataGridTable.RowCount = 0;
                columnCountSet = true;

                //
                // Create the column headers
                //
                createColumns();

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

                    this.Refresh();
                }
            }
        }

        private void checkBoxRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxRefresh.Checked)
            {
                refreshThread = new Thread(new ThreadStart(refreshTable));
                refreshThread.Start();
            }
            else
            {
                refreshThread.Abort();
            }
        }

        internal void createColumns()
        {
            int x = 0;
            
            //
            // We want to move into the table object here
            //
            IEnumerator<IDefinition> i = definition.Children.GetEnumerator();
            i.Reset();
            i.MoveNext();
            IDefinition d = i.Current;


            foreach (IDefinition def in d.Children)
            {
                if(cbColumnDisplay.SelectedIndex == 0)
                {
                    dataGridTable.Columns[x++].Name = def.TextualForm;
                }
                else
                {
                    dataGridTable.Columns[x++].Name = def.Name;
                }
            }
        }

        private void cbColumnDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (columnCountSet)
            {
                createColumns();
                this.Refresh();
            }
        }

        public void refreshTable()
        {
            while (true)
            {
                IList<Variable> list = new List<Variable>();
                AgentProfile prof = ProfileRegistry.Instance.DefaultProfile;
                int rows = 0;

                Thread.Sleep(Convert.ToInt32(textBoxRefresh.Text, CultureInfo.CurrentCulture) * 1000);
                rows = Manager.Walk(prof.VersionCode, prof.Agent, new OctetString(prof.GetCommunity), new ObjectIdentifier(definition.GetNumericalForm()), list, 1000, WalkMode.WithinSubtree);

                this.setRows(rows);
                this.populateGrid(list);
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
            if (refreshThread != null)
            {
                refreshThread.Abort();
            }
        }
    }
}
