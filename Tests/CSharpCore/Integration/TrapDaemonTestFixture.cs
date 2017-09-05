using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;

namespace Lextm.SharpSnmpLib.Integration
{
    public class TrapDaemonTestFixture
    {
        static NumberGenerator port = new NumberGenerator(40000, 65000);

        [Fact]
        public async Task TestTrapV2HandlerWithV2Message()
        {
            var manualEvent = new ManualResetEventSlim();
            // TODO: this is a hack. review it later.
            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"),
                new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
#if !NETSTANDARD
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));
#endif
            var count = 0;

            var trapv1 = new TrapV1MessageHandler();
            var trapv1Mapping = new HandlerMapping("v1", "TRAPV1", trapv1);

            var trapv2 = new TrapV2MessageHandler();
            trapv2.MessageReceived += (sender, args) =>
            {
                count++;
                manualEvent.Set();
            };
            var trapv2Mapping = new HandlerMapping("v2,v3", "TRAPV2", trapv2);

            var inform = new InformRequestMessageHandler();
            var informMapping = new HandlerMapping("v2,v3", "INFORM", inform);

            var store = new ObjectStore();
            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] {v1, v2, v3});
            var handlerFactory = new MessageHandlerFactory(new[] {trapv1Mapping, trapv2Mapping, informMapping});

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            var engine = new SnmpEngine(pipelineFactory, new Listener {Users = users}, new EngineGroup());
            var daemonEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(daemonEndPoint);
            engine.Listener.ExceptionRaised += (sender, e) => { Assert.True(false, "unhandled exception"); };
            engine.Listener.MessageReceived += (sender, e) => { Console.WriteLine(e.Message); };
            engine.Start();

            try
            {
                await Messenger.SendTrapV2Async(1, VersionCode.V2, daemonEndPoint, new OctetString("public"),
                    new ObjectIdentifier("1.3.6.1"), 500, new List<Variable>());
                manualEvent.Wait();

                Assert.Equal(1, count);
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        [Fact]
        public async Task TestTrapV2HandlerWithV3Message()
        {
            var manualEvent = new ManualResetEventSlim();
            // TODO: this is a hack. review it later.
            var engineId = new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449"));
            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"),
                new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")))
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
            {
                count++;
                manualEvent.Set();
            };
            var trapv2Mapping = new HandlerMapping("v2,v3", "TRAPV2", trapv2);

            var inform = new InformRequestMessageHandler();
            var informMapping = new HandlerMapping("v2,v3", "INFORM", inform);

            var store = new ObjectStore();
            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] {v1, v2, v3});
            var handlerFactory = new MessageHandlerFactory(new[] {trapv1Mapping, trapv2Mapping, informMapping});

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            var engine = new SnmpEngine(pipelineFactory, new Listener {Users = users}, new EngineGroup());
            var daemonEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(daemonEndPoint);
            engine.Start();

            try
            {
                var privacy =
                    new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")));
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
                manualEvent.Wait();

                Assert.Equal(1, count);
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        [Fact]
        public async Task TestTrapV2HandlerWithV3MessageAndWrongEngineId()
        {
            var manualEvent = new ManualResetEventSlim();
            // TODO: this is a hack. review it later.
            var engineId = new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449"));
            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"),
                new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")))
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
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] {v1, v2, v3});
            var handlerFactory = new MessageHandlerFactory(new[] {trapv1Mapping, trapv2Mapping, informMapping});

            var logger = new TestLogger();
            logger.Handler = (obj, args) => { manualEvent.Set(); };

            var pipelineFactory = new SnmpApplicationFactory(logger, store, membership, handlerFactory);
            var group = new EngineGroup();
            var engine = new SnmpEngine(pipelineFactory, new Listener {Users = users}, group);
            var daemonEndPoint = new IPEndPoint(IPAddress.Loopback, port.NextId);
            engine.Listener.AddBinding(daemonEndPoint);
            engine.Start();

            try
            {
                var privacy =
                    new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")));
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
                manualEvent.Wait();

                Assert.Equal(0, count);
                Assert.Equal(new Counter32(1), group.UnknownEngineId.Data);
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        class TestLogger : ILogger
        {
            public EventHandler Handler;

            public void Log(ISnmpContext context)
            {
                Handler?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
