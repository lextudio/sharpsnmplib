/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace SnmpTrapD
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            Console.WriteLine("#SNMP is available at http://sharpsnmplib.codeplex.com");
            Listener watcher = new Listener();
            DefaultListenerAdapter adapter = new DefaultListenerAdapter();
            watcher.Adapters.Add(adapter);
            adapter.TrapV1Received += WatcherTrapV1Received;
            adapter.TrapV2Received += WatcherTrapV2Received;
            adapter.InformRequestReceived += WatcherInformRequestReceived;
            watcher.AddBinding(new IPEndPoint(IPAddress.Any, 162));
            watcher.Start();
            Console.WriteLine("Press any key to stop . . . ");
            Console.Read();
        }

        private static void WatcherInformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
        {
            Console.WriteLine(e);
        }

        private static void WatcherTrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
        {
            Console.WriteLine(e);
        }

        private static void WatcherTrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
        {
            Console.WriteLine(e);
        }
    }
}