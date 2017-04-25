using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Lextm.SharpSnmpLib.Integration
{
    public class TrapDaemonTestFixture
    {
        static NumberGenerator port = new NumberGenerator(40000, 65000);

        [Fact]
        public async Task TestTrapV2HandlerWithV2Message()
        {
            // TODO: this is a hack. review it later.
            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
#if !NETSTANDARD
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));
#endif
            var count = 0;

            var trapv1 = new TrapV1MessageHandler();
            var trapv1Mapping = new HandlerMapping("v1", "TRAPV1", trapv1);

            var trapv2 = new TrapV2MessageHandler();
            trapv2.MessageReceived += (sender, args) => { count++; };
            var trapv2Mapping = new HandlerMapping("v2,v3", "TRAPV2", trapv2);

            var inform = new InformRequestMessageHandler();
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
                var daemonEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
                engine.Listener.AddBinding(daemonEndPoint);
                engine.Start();

                await Messenger.SendTrapV2Async(1, VersionCode.V2, daemonEndPoint, new OctetString("public"), new ObjectIdentifier("1.3.6.1"), 500, new List<Variable>());
                await Task.Delay(5000);

                Assert.Equal(1, count);

                engine.Stop();
            }
        }

        [Fact]
        public async Task TestTrapV2HandlerWithV3Message()
        {
            // TODO: this is a hack. review it later.
            var engineId = new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449"));
            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")))
            {
                EngineId = engineId
            });
#if !NETSTANDARD
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));
#endif
            var count = 0;

            var trapv1 = new TrapV1MessageHandler();
            var trapv1Mapping = new HandlerMapping("v1", "TRAPV1", trapv1);

            var trapv2 = new TrapV2MessageHandler();
            trapv2.MessageReceived += (sender, args) => 
            { count++; };
            var trapv2Mapping = new HandlerMapping("v2,v3", "TRAPV2", trapv2);

            var inform = new InformRequestMessageHandler();
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
                var daemonEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
                engine.Listener.AddBinding(daemonEndPoint);
                engine.Start();

                var privacy = new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")));
                var trap = new TrapV2Message(
                    VersionCode.V3,
                    1004947569,
                    234419641,
                    new OctetString("authen"),
                    new ObjectIdentifier("1.3.6"),
                    0,
                    new List<Variable>(),
                    privacy,
                    0x10000,
                    engineId,
                    0,
                    0);
                await trap.SendAsync(daemonEndPoint);
                await Task.Delay(5000);

                Assert.Equal(1, count);

                engine.Stop();
            }
        }

        [Fact]
        public async Task TestTrapV2HandlerWithV3MessageAndWrongEngineId()
        {
            // TODO: this is a hack. review it later.
            var engineId = new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449"));
            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")))
            {
                EngineId = engineId
            });
#if !NETSTANDARD
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));
#endif
            var count = 0;

            var trapv1 = new TrapV1MessageHandler();
            var trapv1Mapping = new HandlerMapping("v1", "TRAPV1", trapv1);

            var trapv2 = new TrapV2MessageHandler();
            trapv2.MessageReceived += (sender, args) => { count++; };
            var trapv2Mapping = new HandlerMapping("v2,v3", "TRAPV2", trapv2);

            var inform = new InformRequestMessageHandler();
            var informMapping = new HandlerMapping("v2,v3", "INFORM", inform);

            var store = new ObjectStore();
            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1, v2, v3 });
            var handlerFactory = new MessageHandlerFactory(new[] { trapv1Mapping, trapv2Mapping, informMapping });

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            var group = new EngineGroup();
            using (var engine = new SnmpEngine(pipelineFactory, new Listener { Users = users }, group))
            {
                var daemonEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
                engine.Listener.AddBinding(daemonEndPoint);
                engine.Start();

                var privacy = new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")));
                var trap = new TrapV2Message(
                    VersionCode.V3,
                    1004947569,
                    234419641,
                    new OctetString("authen"),
                    new ObjectIdentifier("1.3.6"),
                    0,
                    new List<Variable>(),
                    privacy,
                    0x10000,
                    new OctetString(ByteTool.Convert("80001F8880E9630000D61FF450")),
                    0,
                    0);
                await trap.SendAsync(daemonEndPoint);
                await Task.Delay(5000);

                Assert.Equal(0, count);
                Assert.Equal(new Counter32(1), group.UnknownEngineId.Data);

                engine.Stop();
            }
        }
    }
}
