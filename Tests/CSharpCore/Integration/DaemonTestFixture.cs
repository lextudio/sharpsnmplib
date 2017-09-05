using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Lextm.SharpSnmpLib.Integration
{
    public class DaemonTestFixture
    {
        private static readonly NumberGenerator Port = new NumberGenerator(40000, 45000);

        private SnmpEngine CreateEngine()
        {
            // TODO: this is a hack. review it later.
            var store = new ObjectStore();
            store.Add(new SysDescr());
            store.Add(new SysObjectId());
            store.Add(new SysUpTime());
            store.Add(new SysContact());
            store.Add(new SysName());
            store.Add(new SysLocation());
            store.Add(new SysServices());
            store.Add(new SysORLastChange());
            store.Add(new SysORTable());
            store.Add(new IfNumber());
            store.Add(new IfTable());
            store.Add(new TimeoutObject());

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
#if !NETSTANDARD
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));
#endif
            var getv1 = new GetV1MessageHandler();
            var getv1Mapping = new HandlerMapping("v1", "GET", getv1);

            var getv23 = new GetMessageHandler();
            var getv23Mapping = new HandlerMapping("v2,v3", "GET", getv23);

            var setv1 = new SetV1MessageHandler();
            var setv1Mapping = new HandlerMapping("v1", "SET", setv1);

            var setv23 = new SetMessageHandler();
            var setv23Mapping = new HandlerMapping("v2,v3", "SET", setv23);

            var getnextv1 = new GetNextV1MessageHandler();
            var getnextv1Mapping = new HandlerMapping("v1", "GETNEXT", getnextv1);

            var getnextv23 = new GetNextMessageHandler();
            var getnextv23Mapping = new HandlerMapping("v2,v3", "GETNEXT", getnextv23);

            var getbulk = new GetBulkMessageHandler();
            var getbulkMapping = new HandlerMapping("v2,v3", "GETBULK", getbulk);

            var v1 = new Version1MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v2 = new Version2MembershipProvider(new OctetString("public"), new OctetString("public"));
            var v3 = new Version3MembershipProvider();
            var membership = new ComposedMembershipProvider(new IMembershipProvider[] { v1, v2, v3 });
            var handlerFactory = new MessageHandlerFactory(new[]
            {
                getv1Mapping,
                getv23Mapping,
                setv1Mapping,
                setv23Mapping,
                getnextv1Mapping,
                getnextv23Mapping,
                getbulkMapping
            });

            var pipelineFactory = new SnmpApplicationFactory(store, membership, handlerFactory);
            var listener = new Listener { Users = users };
            listener.ExceptionRaised += (sender, e) => { Assert.True(false, "unexpected exception");};
            return new SnmpEngine(pipelineFactory, listener, new EngineGroup());
        }

        private class TimeoutObject : ScalarObject
        {
            public TimeoutObject()
                : base(new ObjectIdentifier("1.5.2"))
            {

            }

            public override ISnmpData Data
            {
                get
                {
                    Thread.Sleep(1500 * 2);
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }
        }

        [Fact]
        public async Task TestResponseAsync()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"),
                    new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))});

                var users1 = new UserRegistry();
                var response = await message.GetResponseAsync(serverEndPoint, users1, socket);
                Assert.Equal(SnmpType.ResponsePdu, response.TypeCode());
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
        public void TestResponse()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"),
                    new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))});

                const int time = 1500;
                var response = message.GetResponse(time, serverEndPoint, socket);
                Assert.Equal(0x4bed, response.RequestId());
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
        public void TestDiscoverer()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Any, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                var signal = new AutoResetEvent(false);
                var discoverer = new Discoverer();
                discoverer.AgentFound += (sender, args)
                    =>
                {
                    Assert.True(args.Agent.Address.ToString() != "0.0.0.0");
                    signal.Set();
                };
                discoverer.Discover(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                    new OctetString("public"), 3000);
                signal.WaitOne();

                signal.Reset();
                discoverer.Discover(VersionCode.V2, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                    new OctetString("public"), 3000);
                signal.WaitOne();

                signal.Reset();
                discoverer.Discover(VersionCode.V3, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port), null,
                    3000);
                signal.WaitOne();
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
        public async void TestDiscovererAsync()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Any, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                var signal = new AutoResetEvent(false);
                var discoverer = new Discoverer();
                discoverer.AgentFound += (sender, args)
                    =>
                {
                    Assert.True(args.Agent.Address.ToString() != "0.0.0.0");
                    signal.Set();
                };
                await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                    new OctetString("public"), 3000);
                signal.WaitOne();

                signal.Reset();
                await discoverer.DiscoverAsync(VersionCode.V2, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                    new OctetString("public"), 3000);
                signal.WaitOne();

                signal.Reset();
                await discoverer.DiscoverAsync(VersionCode.V3, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                    null, 3000);
                signal.WaitOne();
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        [Theory]
#if  NETSTANDARD
        [InlineData(64)]
#else
        [InlineData(256)]
#endif
        public async Task TestResponsesFromMultipleSources(int count)
        {
            var start = 16102;
            var end = start + count;
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            for (var index = start; index < end; index++)
            {
                engine.Listener.AddBinding(new IPEndPoint(IPAddress.Loopback, index));
            }

#if !NETSTANDARD 
            // IMPORTANT: need to set min thread count so as to boost performance.
            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            var threads = engine.Listener.Bindings.Count;
            ThreadPool.SetMinThreads(threads + 1, minIOC);
#endif
            engine.Start();

            try
            {
                for (int index = start; index < end; index++)
                {
                    GetRequestMessage message = new GetRequestMessage(index, VersionCode.V2, new OctetString("public"),
                        new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))});
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    var response =
                        await
                            message.GetResponseAsync(new IPEndPoint(IPAddress.Loopback, index), new UserRegistry(),
                                socket);
                    watch.Stop();
                    Assert.Equal(index, response.RequestId());
                }
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        [Theory]
        [InlineData(32)]
        public async Task TestResponsesFromSingleSource(int count)
        {
            var start = 0;
            var end = start + count;
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            //// IMPORTANT: need to set min thread count so as to boost performance.
            //int minWorker, minIOC;
            //// Get the current settings.
            //ThreadPool.GetMinThreads(out minWorker, out minIOC);
            //var threads = engine.Listener.Bindings.Count;
            //ThreadPool.SetMinThreads(threads + 1, minIOC);

            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                for (int index = start; index < end; index++)
                {
                    GetRequestMessage message = new GetRequestMessage(0, VersionCode.V2, new OctetString("public"),
                        new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))});
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    var response =
                        await
                            message.GetResponseAsync(serverEndPoint, new UserRegistry(), socket);
                    watch.Stop();
                    Assert.Equal(0, response.RequestId());
                }
            }
            catch (Exception)
            {
                Console.WriteLine(serverEndPoint.Port);
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        [Theory]
        [InlineData(32)]
        public void TestResponsesFromSingleSourceWithMultipleThreads(int count)
        {
            var start = 0;
            var end = start + count;
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
#if !NETSTANDARD 
            // IMPORTANT: need to set min thread count so as to boost performance.
            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            var threads = engine.Listener.Bindings.Count;
            ThreadPool.SetMinThreads(threads + 1, minIOC);
#endif
            engine.Start();

            try
            {
                const int timeout = 10000;

                // Uncomment below to reveal wrong sequence number issue.
                // Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                Parallel.For(start, end, index =>
                    {
                        GetRequestMessage message = new GetRequestMessage(index, VersionCode.V2,
                            new OctetString("public"),
                            new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))});
                        // Comment below to reveal wrong sequence number issue.
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        var response = message.GetResponse(timeout, serverEndPoint, socket);
                        watch.Stop();
                        Assert.Equal(index, response.RequestId());
                    }
                );
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }

        [Theory]
        [InlineData(128)]
        public void TestResponsesFromSingleSourceWithMultipleThreadsFromManager(int count)
        {
            var start = 0;
            var end = start + count;
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                const int timeout = 60000;

                //for (int index = start; index < end; index++)
                Parallel.For(start, end, index =>
                    {
                        try
                        {
                            var result = Messenger.Get(VersionCode.V2, serverEndPoint, new OctetString("public"),
                                new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))}, timeout);
                            Assert.Equal(1, result.Count);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(serverEndPoint.Port);
                        }
                    }
                );
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
        public void TestTimeOut()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"),
                    new List<Variable> {new Variable(new ObjectIdentifier("1.5.2"))});

                const int time = 1500;
                var timer = new Stopwatch();
                timer.Start();
                //IMPORTANT: test against an agent that doesn't exist.
                Assert.Throws<Messaging.TimeoutException>(() => message.GetResponse(time, serverEndPoint, socket));
                timer.Stop();

                long elapsedMilliseconds = timer.ElapsedMilliseconds;
                Assert.True(time <= elapsedMilliseconds);

                // FIXME: these values are valid on my machine openSUSE 11.2. (lex)
                // This test case usually fails on Windows, as strangely WinSock API call adds an extra 500-ms.
                if (SnmpMessageExtension.IsRunningOnMono)
                {
                    Assert.True(elapsedMilliseconds <= time + 100);
                }
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
        public void TestLargeMessage()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                var list = new List<Variable>();
                for (int i = 0; i < 1000; i++)
                {
                    list.Add(new Variable(new ObjectIdentifier("1.3.6.1.1.1.0")));
                }

                GetRequestMessage message = new GetRequestMessage(
                    0x4bed,
                    VersionCode.V2,
                    new OctetString("public"),
                    list);

                Assert.True(message.ToBytes().Length > 10000);

                var time = 1500;
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    // IMPORTANT: test against an agent that doesn't exist.
                    var result = message.GetResponse(time, serverEndPoint, socket);
                    Assert.True(result.Scope.Pdu.ErrorStatus.ToErrorCode() == ErrorCode.NoError);
                }
                else
                {
                    var exception = Assert.Throws<SocketException>(() => message.GetResponse(time, serverEndPoint, socket));
                    Assert.Equal(SocketError.MessageSize, exception.SocketErrorCode);
                }
            }
            finally
            {
                if (SnmpMessageExtension.IsRunningOnWindows)
                {
                    engine.Stop();
                }
            }
        }
    }
}
