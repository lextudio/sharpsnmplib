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

namespace test
{
	class Program
	{
		public static void Main(string[] args)
		{
            Console.WriteLine("Start watcher");

            TrapListener watcher = new TrapListener();
            watcher.TrapV1Received += new EventHandler<TrapV1ReceivedEventArgs>(watcher_TrapReceived);
            watcher.TrapV2Received += new EventHandler<TrapV2ReceivedEventArgs>(watcher_TrapV2Received);
            watcher.InformRequestReceived += new EventHandler<InformRequestReceivedEventArgs>(watcher_InformRequestReceived);
            watcher.Start();

            Console.WriteLine("Press any key to stop . . . ");
            Console.ReadKey(true);
            watcher.Stop();
            Console.WriteLine("Press any key to exit . . . ");
            Console.Read();
		}

        static void watcher_TrapV2Received(object sender, TrapV2ReceivedEventArgs e)
        {
            Console.WriteLine(e);
        }

        static void watcher_TrapReceived(object sender, TrapV1ReceivedEventArgs e)
        {
            Console.WriteLine(e);
        }
        
        static void watcher_InformRequestReceived(object sender, InformRequestReceivedEventArgs e)
        {
            Console.WriteLine(e);
        }
	}
}