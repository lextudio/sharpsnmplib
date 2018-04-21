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
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
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

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            if (DESPrivacyProvider.IsSupported)
            {
                users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                            new MD5AuthenticationProvider(new OctetString("authentication"))));
            }

            if (AESPrivacyProviderBase.IsSupported)
            {
                users.Add(new OctetString("aes"), new AESPrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
                users.Add(new OctetString("aes192"), new AES192PrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
                users.Add(new OctetString("aes256"), new AES256PrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
            }

            var trapv1 = new TrapV1MessageHandler();
            trapv1.MessageReceived += WatcherTrapV1Received;
            var trapv1Mapping = new HandlerMapping("v1", "TRAPV1", trapv1);

            var trapv2 = new TrapV2MessageHandler();
            trapv2.MessageReceived += WatcherTrapV2Received;
            var trapv2Mapping = new HandlerMapping("v2,v3", "TRAPV2", trapv2);

            var inform = new InformRequestMessageHandler();
            inform.MessageReceived += WatcherInformRequestReceived;
            var informMapping = new HandlerMapping("v2,v3", "INFORM", inform);

            var store = new ObjectStore();
            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1, v2, v3 });
            var handlerFactory = new MessageHandlerFactory(new[] { trapv1Mapping, trapv2Mapping, informMapping });

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            using (var engine = new SnmpEngine(pipelineFactory, new Listener { Users = users }, new EngineGroup()))
            {
                engine.Listener.AddBinding(new IPEndPoint(IPAddress.Any, 162));
                engine.Start();
                Console.WriteLine("#SNMP is available at https://sharpsnmp.com");
                Console.WriteLine("Press any key to stop . . . ");
                Console.Read();
                engine.Stop();
            }
        }

        private static void WatcherInformRequestReceived(object sender, InformRequestMessageReceivedEventArgs e)
        {
            Console.WriteLine("INFORM version {0}: {1}", e.InformRequestMessage.Version, e.InformRequestMessage);
        }

        private static void WatcherTrapV2Received(object sender, TrapV2MessageReceivedEventArgs e)
        {
            Console.WriteLine("TRAP version {0}: {1}", e.TrapV2Message.Version, e.TrapV2Message);
        }

        private static void WatcherTrapV1Received(object sender, TrapV1MessageReceivedEventArgs e)
        {
            Console.WriteLine("TRAP version {0}; {1}", e.TrapV1Message.Version, e.TrapV1Message);
        }
    }
}
