/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace test
{
	class Program
	{
		public static void Main(string[] args)
		{
            Console.WriteLine("Start watcher");

            Listener watcher = new Listener();
            watcher.TrapV1Received += new EventHandler<MessageReceivedEventArgs<TrapV1Message>>(watcher_TrapV1Received);
            watcher.TrapV2Received += new EventHandler<MessageReceivedEventArgs<TrapV2Message>>(watcher_TrapV2Received);
            watcher.InformRequestReceived += new EventHandler<MessageReceivedEventArgs<InformRequestMessage>>(watcher_InformRequestReceived);
            watcher.Start();

            Console.WriteLine("Press any key to stop . . . ");
            Console.ReadKey(true);
            watcher.Stop();
            Console.WriteLine("Press any key to exit . . . ");
            Console.Read();
		}

		static void watcher_InformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
		{
			Console.WriteLine(e);
		}

		static void watcher_TrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
		{
			Console.WriteLine(e);
		}

		static void watcher_TrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
		{
			Console.WriteLine(e);
		}

	}
}