/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 15:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of OutputPanel.
	/// </summary>
	internal partial class OutputPanel : DockContent, IOutputPanel
	{
		private IProfileRegistry _profiles;

		public OutputPanel()
		{
			InitializeComponent();
		}

		public void WriteLine(string message)
		{
			if (InvokeRequired)
			{
				Invoke((MethodInvoker)delegate { WriteLine(message); });
				return;
			}

		    IPEndPoint agent = Profiles.DefaultProfile != null ? Profiles.DefaultProfile.Agent : null;
            txtMessages.AppendText(string.Format(CultureInfo.CurrentCulture, "[{2}] [{0}] {1}", DateTime.Now, message, agent));
			txtMessages.AppendText(Environment.NewLine);
			txtMessages.ScrollToCaret();
		}

		[Dependency]
		public IProfileRegistry Profiles
		{
			get { return _profiles; }
			set { _profiles = value; }
		}

		private void ActClearExecute(object sender, EventArgs e)
		{
			txtMessages.Clear();
		}

		private void TxtMessagesMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				contextOuputMenu.Show(txtMessages, e.Location);
			}
		}
	}
}
