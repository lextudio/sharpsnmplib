/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/1/28
 * Time: 16:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of NotificationPanel.
	/// </summary>
	internal partial class NotificationPanel : DockContent
	{
		private Manager _manager;
		
		public NotificationPanel()
		{
			InitializeComponent();
		}
		
		[Dependency]
		public Manager Manager
		{
			get { return _manager; }
			set { _manager = value; }
		}
		
		void NotificationPanel_Load(object sender, EventArgs e)
		{
			Manager.TrapV1Received += new EventHandler<TrapV1ReceivedEventArgs>(Manager_TrapV1Received);
			Manager.TrapV2Received += new EventHandler<TrapV2ReceivedEventArgs>(Manager_TrapV2Received);
			Manager.InformRequestReceived += new EventHandler<InformRequestReceivedEventArgs>(Manager_InformRequestReceived);
		}

		void Manager_InformRequestReceived(object sender, InformRequestReceivedEventArgs e)
		{
			
		}

		void Manager_TrapV2Received(object sender, TrapV2ReceivedEventArgs e)
		{
			
		}

		void Manager_TrapV1Received(object sender, TrapV1ReceivedEventArgs e)
		{
			
		}
	}
}
