using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
    internal partial class FormProfile : Form
    {
        private AgentProfile _agent;

        public FormProfile(AgentProfile agent)
        {
            InitializeComponent();
            _agent = agent;
            if (_agent != null)
            {

            }
        }

        internal string IP
        {
            get { return null; }
        }

        internal VersionCode VersionCode
        {
            get { return VersionCode.V1; }
        }

        internal string GetCommunity
        {
            get { return null; }
        }

        internal string SetCommunity
        {
            get { return null; }
        }
    }
}
