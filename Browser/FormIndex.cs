using System;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
    partial class FormIndex : Form
    {
        public FormIndex()
        {
            InitializeComponent();
        }

        public uint Index
        {
            get
            {
                return (uint)nudIndex.Value;
            }
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
