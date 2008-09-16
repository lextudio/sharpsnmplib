using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
    partial class FormSet : Form
    {
        public FormSet()
        {
            InitializeComponent();
        }

        public string OldVal
        {
            set
            {
                txtCurrent.Text = value;
            }
        }

        public string NewVal
        {
            get
            {
                return txtNew.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
