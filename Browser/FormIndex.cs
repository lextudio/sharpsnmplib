using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Browser
{
    partial class FormIndex : Form
    {
        public FormIndex()
        {
            InitializeComponent();
            nudIndex.Maximum = decimal.MaxValue;
        }

        public int Index
        {
            get
            {
                return (int)nudIndex.Value;
            }
        }
    }
}
