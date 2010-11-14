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
using Lextm.SharpSnmpLib.Pipeline;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SnmpTrapD
{
    internal static class Program
    {
        internal static IUnityContainer Container { get; private set; }

        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            Container = new UnityContainer().LoadConfiguration("snmptrapd");

            var trapv1 = Container.Resolve<TrapV1MessageHandler>("TrapV1Handler");
            trapv1.MessageReceived += WatcherTrapV1Received;
            var trapv2 = Container.Resolve<TrapV2MessageHandler>("TrapV2Handler");
            trapv2.MessageReceived += WatcherTrapV2Received;
            var inform = Container.Resolve<InformMessageHandler>("InformHandler");
            inform.MessageReceived += WatcherInformRequestReceived;
            using (var engine = Container.Resolve<SnmpEngine>())
            {
                engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, 162));
                engine.Start();
                Console.WriteLine("#SNMP is available at http://sharpsnmplib.codeplex.com");
                Console.WriteLine("Press any key to stop . . . ");
                Console.Read();
                engine.Stop();
            }
        }

        private static void WatcherInformRequestReceived(object sender, MessageReceivedEventArgs<InformRequestMessage> e)
        {
            Console.WriteLine(e.Message);
        }

        private static void WatcherTrapV2Received(object sender, MessageReceivedEventArgs<TrapV2Message> e)
        {
            Console.WriteLine(e.Message);
        }

        private static void WatcherTrapV1Received(object sender, MessageReceivedEventArgs<TrapV1Message> e)
        {
            Console.WriteLine(e.Message);
        }

    }
}