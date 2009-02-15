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
		private Listener _listener;
		
		public NotificationPanel()
		{
			InitializeComponent();
		}
		
		[Dependency]
		public Listener Listener
		{
			get { return _listener; }
			set { _listener = value; }
		}
		
		private void NotificationPanel_Load(object sender, EventArgs e)
		{
            Listener.ExceptionRaised += Listener_ExceptionRaised;
			Listener.TrapV1Received += new EventHandler<MessageReceivedEventArgs<TrapV1Message>>(Listener_TrapV1Received);;
			Listener.TrapV2Received += new EventHandler<MessageReceivedEventArgs<TrapV2Message>>(Listener_TrapV2Received);
			Listener.InformRequestReceived += new EventHandler<MessageReceivedEventArgs<InformRequestMessage>>(Listener_InformRequestReceived);
		}

		private void Listener_InformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
		{
			
		}

		private void Listener_TrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
		{
			
		}

		private void Listener_TrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
		{
			
		}

	    private void Listener_ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
	    {
	        
	    }
	    
	}
}
