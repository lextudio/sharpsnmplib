using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Browser
{
    public partial class FormTable : Form
    {
        private IDefinition definition;

        [CLSCompliant(false)]
        public FormTable(IDefinition def)
        {
            definition = def;
            InitializeComponent();
        }

        public void setRows(int rowCount)
        {
            dataGridTable.RowCount = rowCount;
        }

        public void populateGrid(IList<Variable> list)
        {
            int rowCount = dataGridTable.RowCount;

            dataGridTable.ColumnCount = (list.Count / dataGridTable.RowCount);
            dataGridTable.RowCount = 0;

            //
            // Populate the Column headers
            //
            for (int x = 0; x < dataGridTable.ColumnCount;x++ )
            {
                dataGridTable.Columns[x].Name = "Test";
            }

            //
            // Fill in the tree itself
            //
            //dataGridTable.DataSource = list;
            for (int x = 0; x < rowCount; x++)
            {
                string[] row = new string[dataGridTable.ColumnCount];

                for (int y = 0; y < dataGridTable.ColumnCount; y++)
                {
                    row[y] = list[(y * rowCount) + x].Data.ToString();
                }

                dataGridTable.Rows.Add(row);
            }
        }

        private void checkBoxRefresh_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
