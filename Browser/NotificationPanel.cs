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
		private TrapListener _listener;
		
		public NotificationPanel()
		{
			InitializeComponent();
		}
		
		[Dependency]
		public TrapListener Listener
		{
			get { return _listener; }
			set { _listener = value; }
		}
		
		private void NotificationPanel_Load(object sender, EventArgs e)
		{
            Listener.ExceptionRaised += Listener_ExceptionRaised;
			Listener.TrapV1Received += Manager_TrapV1Received;
			Listener.TrapV2Received += Manager_TrapV2Received;
			Listener.InformRequestReceived += Manager_InformRequestReceived;
		}

	    private void Listener_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
	    {
	        
	    }

	    private void Manager_InformRequestReceived(object sender, InformRequestReceivedEventArgs e)
		{
			
		}

        private void Manager_TrapV2Received(object sender, TrapV2ReceivedEventArgs e)
		{
			
		}

        private void Manager_TrapV1Received(object sender, TrapV1ReceivedEventArgs e)
		{
			
		}
	}
}
