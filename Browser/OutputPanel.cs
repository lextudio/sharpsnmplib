/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 15:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Browser
{
	/// <summary>
	/// Description of OutputPanel.
	/// </summary>
	partial class OutputPanel : DockContent
	{
		public OutputPanel()
		{
			InitializeComponent();
		}

        internal void ReportMessage(string message)
        {
            txtMessages.AppendText(string.Format("[{0}] {1}", DateTime.Now, message));
            txtMessages.AppendText(Environment.NewLine);
            txtMessages.ScrollToCaret();
        }
	}
}
