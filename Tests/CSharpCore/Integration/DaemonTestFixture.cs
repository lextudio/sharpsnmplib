using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Lextm.SharpSnmpLib.Integration
{
    public class DaemonTestFixture
    {
        private static readonly NumberGenerator Port = new NumberGenerator(40000, 45000);
        private const int MaxTimeout = 5 * 60 * 1000; // 5 minutes


        private SnmpEngine CreateEngine(bool timeout = false)
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
            if (timeout)
            {
                store.Add(new TimeoutObject());
            }

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            if (DESPrivacyProvider.IsSupported)
            {
                users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                             new MD5AuthenticationProvider(new OctetString("authentication"))));
            }

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
            listener.ExceptionRaised += (sender, e) => { Assert.True(false, "unexpected exception"); };
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
                    new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });

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
                    new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });

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
        public void TestResponseVersion3()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                IAuthenticationProvider auth = new MD5AuthenticationProvider(new OctetString("authentication"));
                IPrivacyProvider priv = new DefaultPrivacyProvider(auth);

                var ending = new AutoResetEvent(false);
                var timeout = 3000;
                Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
                ReportMessage report = discovery.GetResponse(timeout, serverEndPoint);

                var expected = Messenger.NextRequestId;
                GetRequestMessage request = new GetRequestMessage(VersionCode.V3, Messenger.NextMessageId, expected, new OctetString("authen"), OctetString.Empty, new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) }, priv, Messenger.MaxMessageSize, report);

                var source = Observable.Defer(() =>
                {
                    ISnmpMessage reply = request.GetResponse(timeout, serverEndPoint);
                    return Observable.Return(reply);
                })
                .RetryWithBackoffStrategy(
                    retryCount: 4,
                    retryOnError: e => e is Messaging.TimeoutException
                );

                source.Subscribe(reply =>
                {
                    ISnmpPdu snmpPdu = reply.Pdu();
                    Assert.Equal(SnmpType.ResponsePdu, snmpPdu.TypeCode);
                    Assert.Equal(expected, reply.RequestId());
                    Assert.Equal(ErrorCode.NoError, snmpPdu.ErrorStatus.ToErrorCode());
                    ending.Set();
                });
                Assert.True(ending.WaitOne(MaxTimeout));
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
        public void TestResponseVersion3_2()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                IAuthenticationProvider auth = new MD5AuthenticationProvider(new OctetString("authenticationauthentication"));
                IPrivacyProvider priv = new DefaultPrivacyProvider(auth);

                var timeout = 3000;
                Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
                ReportMessage report = discovery.GetResponse(timeout, serverEndPoint);

                var expected = Messenger.NextRequestId;
                GetRequestMessage request = new GetRequestMessage(VersionCode.V3, Messenger.NextMessageId, expected, new OctetString("authen"), OctetString.Empty, new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) }, priv, Messenger.MaxMessageSize, report);
                ISnmpMessage reply = request.GetResponse(timeout, serverEndPoint);
                ISnmpPdu snmpPdu = reply.Pdu();
                Assert.Equal(SnmpType.ResponsePdu, snmpPdu.TypeCode);
                Assert.Equal(expected, reply.RequestId());
                Assert.Equal(ErrorCode.NoError, snmpPdu.ErrorStatus.ToErrorCode());
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

            var timeout = 1000;
            var wait = 60 * timeout;
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
                    new OctetString("public"), timeout);
                Assert.True(signal.WaitOne(wait));

                signal.Reset();
                discoverer.Discover(VersionCode.V2, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                    new OctetString("public"), timeout);
                Assert.True(signal.WaitOne(wait));

                signal.Reset();
                discoverer.Discover(VersionCode.V3, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port), null,
                    timeout);
                Assert.True(signal.WaitOne(wait));
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
        public void TestDiscovererAsyncV1()
        {
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                return;
            }

            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Any, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            var timeout = 1000;
            var wait = 60 * timeout;
            try
            {
                var signal = new AutoResetEvent(false);
                var ending = new AutoResetEvent(false);
                var discoverer = new Discoverer();
                discoverer.AgentFound += (sender, args)
                    =>
                {
                    Assert.True(args.Agent.Address.ToString() != "0.0.0.0");
                    signal.Set();
                };

                var source = Observable.Defer(async () =>
                {
                    await discoverer.DiscoverAsync(VersionCode.V1, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                        new OctetString("public"), timeout);
                    var result = signal.WaitOne(wait);
                    if (!result)
                    {
                        throw new Messaging.TimeoutException();
                    }

                    return Observable.Return(result);
                })
                .RetryWithBackoffStrategy(
                    retryCount: 4,
                    retryOnError: e => e is Messaging.TimeoutException
                );

                source.Subscribe(result =>
                {
                    Assert.True(result);
                    ending.Set();
                });
                Assert.True(ending.WaitOne(MaxTimeout));
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
        public void TestDiscovererAsyncV2()
        {
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                return;
            }

            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Any, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            var timeout = 1000;
            var wait = 60 * timeout;
            try
            {
                var signal = new AutoResetEvent(false);
                var ending = new AutoResetEvent(false);
                var discoverer = new Discoverer();
                discoverer.AgentFound += (sender, args)
                    =>
                {
                    Assert.True(args.Agent.Address.ToString() != "0.0.0.0");
                    signal.Set();
                };

                var source = Observable.Defer(async () =>
                {
                    await discoverer.DiscoverAsync(VersionCode.V2, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                        new OctetString("public"), timeout);
                    var result = signal.WaitOne(wait);
                    if (!result)
                    {
                        throw new Messaging.TimeoutException();
                    }

                    return Observable.Return(result);
                })
                .RetryWithBackoffStrategy(
                    retryCount: 4,
                    retryOnError: e => e is Messaging.TimeoutException
                );

                source.Subscribe(result =>
                {
                    Assert.True(result);
                    ending.Set();
                });
                Assert.True(ending.WaitOne(MaxTimeout));
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
        public void TestDiscovererAsyncV3()
        {
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                return;
            }

            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Any, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            var timeout = 1000;
            var wait = 60 * timeout;
            try
            {
                var signal = new AutoResetEvent(false);
                var ending = new AutoResetEvent(false);
                var discoverer = new Discoverer();
                discoverer.AgentFound += (sender, args)
                    =>
                {
                    Assert.True(args.Agent.Address.ToString() != "0.0.0.0");
                    signal.Set();
                };

                var source = Observable.Defer(async () =>
                {
                    await discoverer.DiscoverAsync(VersionCode.V3, new IPEndPoint(IPAddress.Broadcast, serverEndPoint.Port),
                        null, timeout);
                    var result = signal.WaitOne(wait);
                    if (!result)
                    {
                        throw new Messaging.TimeoutException();
                    }

                    return Observable.Return(result);
                })
                .RetryWithBackoffStrategy(
                    retryCount: 4,
                    retryOnError: e => e is Messaging.TimeoutException
                );

                source.Subscribe(result =>
                {
                    Assert.True(result);
                    ending.Set();
                });
                Assert.True(ending.WaitOne(MaxTimeout));
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

#if NET452
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
                        new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
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
                        new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
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
            var ending = new AutoResetEvent(false);
            var source = Observable.Defer(() =>
                {
                    var start = 0;
                    var end = start + count;
                    var engine = CreateEngine();
                    engine.Listener.ClearBindings();
                    var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
                    engine.Listener.AddBinding(serverEndPoint);
#if NET452
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
                                    new List<Variable> { new Variable( new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
                                // Comment below to reveal wrong sequence number issue.
                                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                                    ProtocolType.Udp);

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

                    return Observable.Return(0);
                })
                .RetryWithBackoffStrategy(
                    retryCount: 4,
                    retryOnError: e => e is Messaging.TimeoutException
                );

            source.Subscribe(result => { ending.Set(); });
            Assert.True(ending.WaitOne(MaxTimeout));
        }

        [Theory]
        [InlineData(32)]
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
                                new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) }, timeout);
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
            var engine = CreateEngine(true);
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"),
                    new List<Variable> { new Variable(new ObjectIdentifier("1.5.2")) });

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
        public void TestSetWrongLength()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                var syscontact_toolong = new Variable((new SysContact()).Variable.Id, new OctetString(new string('x', 256)));
                SetRequestMessage message = new SetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"),
                    new List<Variable> { syscontact_toolong });

                var resp = message.GetResponse(1500, serverEndPoint, socket);
                Assert.Equal(ErrorCode.WrongLength, resp.Pdu().ErrorStatus.ToErrorCode());
                Assert.Equal(1, resp.Pdu().ErrorIndex.ToInt32());
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

                var time = 3000;
                if (SnmpMessageExtension.IsRunningOnMac)
                {
                    var exception =
                        Assert.Throws<SocketException>(() => message.GetResponse(time, serverEndPoint, socket));
                    Assert.Equal(SocketError.MessageSize, exception.SocketErrorCode);
                }
                else
                {
                    // IMPORTANT: test against an agent that doesn't exist.
                    var result = message.GetResponse(time, serverEndPoint, socket);
                    Assert.True(result.Scope.Pdu.ErrorStatus.ToErrorCode() == ErrorCode.NoError);
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
        public void TestWalk()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                var list = new List<Variable>();
                var time = 3000;
                // IMPORTANT: test against an agent that doesn't exist.
                var result = Messenger.Walk(
                    VersionCode.V1,
                    serverEndPoint,
                    new OctetString("public"),
                    new ObjectIdentifier("1.3.6.1.2.1.1"),
                    list,
                    time,
                    WalkMode.WithinSubtree);
                Assert.Equal(16, list.Count);
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
        public async Task TestWalkAsync()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                var list = new List<Variable>();
                // IMPORTANT: test against an agent that doesn't exist.
                var result = await Messenger.WalkAsync(
                    VersionCode.V1,
                    serverEndPoint,
                    new OctetString("public"),
                    new ObjectIdentifier("1.3.6.1.2.1.1"),
                    list,
                    WalkMode.WithinSubtree);
                Assert.Equal(16, list.Count);
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
        public void TestBulkWalk()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                var ending = new AutoResetEvent(false);
                var list = new List<Variable>();
                var time = 3000;

                var source = Observable.Defer(() =>
                {
                    var result = Messenger.BulkWalk(
                        VersionCode.V2,
                        serverEndPoint,
                        new OctetString("public"),
                        OctetString.Empty,
                        new ObjectIdentifier("1.3.6.1.2.1.1"),
                        list,
                        time,
                        10,
                        WalkMode.WithinSubtree,
                        null,
                        null);
                    return Observable.Return(result);
                })
                .RetryWithBackoffStrategy(
                    retryCount: 4,
                    retryOnError: e => e is Messaging.TimeoutException
                );

                source.Subscribe(result =>
                {
                    Assert.Equal(16, list.Count);
                    ending.Set();
                });
                Assert.True(ending.WaitOne(MaxTimeout));
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
        public async Task TestBulkWalkAsync()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, Port.NextId);
            engine.Listener.AddBinding(serverEndPoint);
            engine.Start();

            try
            {
                var list = new List<Variable>();
                // IMPORTANT: test against an agent that doesn't exist.
                var result = await Messenger.BulkWalkAsync(
                    VersionCode.V2,
                    serverEndPoint,
                    new OctetString("public"),
                    OctetString.Empty,
                    new ObjectIdentifier("1.3.6.1.2.1.1"),
                    list,
                    10,
                    WalkMode.WithinSubtree,
                    null,
                    null);
                Assert.Equal(16, list.Count);
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

    static class ReactiveExtensions
    {
        // Adopted from https://gist.github.com/niik/6696449
        public static readonly Func<int, TimeSpan> ExpontentialBackoff = n => TimeSpan.FromSeconds(Math.Pow(n, 2));

        public static IObservable<T> RetryWithBackoffStrategy<T>(
            this IObservable<T> source,
            int retryCount = 3,
            Func<int, TimeSpan> strategy = null,
            Func<Exception, bool> retryOnError = null)
        {
            strategy = strategy ?? ExpontentialBackoff;

            if (retryOnError == null)
                retryOnError = e => true;

            int attempt = 0;

            return Observable.Defer(() =>
            {
                return ((++attempt == 1) ? source : source.DelaySubscription(strategy(attempt - 1)))
                    .Select(item => new Tuple<bool, T, Exception>(true, item, null))
                    .Catch<Tuple<bool, T, Exception>, Exception>(e => retryOnError(e)
                        ? Observable.Throw<Tuple<bool, T, Exception>>(e)
                        : Observable.Return(new Tuple<bool, T, Exception>(false, default(T), e)));
            })
            .Retry(retryCount)
            .SelectMany(t => t.Item1
                ? Observable.Return(t.Item2)
                : Observable.Throw<T>(t.Item3));
        }
    }
}
