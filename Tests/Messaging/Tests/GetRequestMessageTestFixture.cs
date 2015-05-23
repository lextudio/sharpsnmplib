/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 18:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;
using System.Net.Sockets;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;

#pragma warning disable 1591

namespace Lextm.SharpSnmpLib.Messaging.Tests
{
    /// <summary>
    /// Description of TestGetMessage.
    /// </summary>
    [TestFixture]
    public class GetRequestMessageTestFixture
    {
        [Test]
        [Category("Default")]
        public void Test()
        {
            byte[] expected = Properties.Resources.get;
            ISnmpMessage message = MessageFactory.ParseMessages(expected, new UserRegistry())[0];
            Assert.AreEqual(SnmpType.GetRequestPdu, message.TypeCode());
            GetRequestPdu pdu = (GetRequestPdu)message.Pdu();
            Assert.AreEqual(1, pdu.Variables.Count);
            Variable v = pdu.Variables[0];
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }, v.Id.ToNumerical());
            Assert.AreEqual(typeof(Null), v.Data.GetType());
            Assert.GreaterOrEqual(expected.Length, message.ToBytes().Length);
        }

        [Test]
        [Category("Default")]
        public void TestConstructor()
        {
            List<Variable> list = new List<Variable>(1)
                                      {
                                          new Variable(new ObjectIdentifier(new uint[] {1, 3, 6, 1, 2, 1, 1, 6, 0}),
                                                       new Null())
                                      };
            GetRequestMessage message = new GetRequestMessage(0, VersionCode.V2, new OctetString("public"), list);
            Assert.GreaterOrEqual(Properties.Resources.get.Length, message.ToBytes().Length);
        }

        [Test]
        [Category("Default")]
        public void TestConstructorV3Auth1()
        {
            const string bytes = "30 73" +
                                 "02 01  03 " +
                                 "30 0F " +
                                 "02  02 35 41 " +
                                 "02  03 00 FF E3" +
                                 "04 01 05" +
                                 "02  01 03" +
                                 "04 2E  " +
                                 "30 2C" +
                                 "04 0D  80 00 1F 88 80 E9 63 00  00 D6 1F F4  49 " +
                                 "02 01 0D  " +
                                 "02 01 57 " +
                                 "04 05 6C 65 78  6C 69 " +
                                 "04 0C  1C 6D 67 BF  B2 38 ED 63 DF 0A 05 24  " +
                                 "04 00 " +
                                 "30 2D  " +
                                 "04 0D 80 00  1F 88 80 E9 63 00 00 D6  1F F4 49 " +
                                 "04  00 " +
                                 "A0 1A 02  02 01 AF 02 01 00 02 01  00 30 0E 30  0C 06 08 2B  06 01 02 01 01 03 00 05  00";
            ReportMessage report = new ReportMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(13633),
                    new Integer32(0xFFE3),
                    0),
                new SecurityParameters(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    new Integer32(0x0d),
                    new Integer32(0x57),
                    new OctetString("lexli"),
                    new OctetString(new byte[12]),
                    OctetString.Empty),
                new Scope(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    OctetString.Empty,
                    new ReportPdu(
                        0x01AF,
                        ErrorCode.NoError,
                        0,
                        new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")) })),
                DefaultPrivacyProvider.DefaultPair,
                null);
            
            IPrivacyProvider privacy = new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("testpass")));
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                13633,
                0x01AF,
                new OctetString("lexli"),
                new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")) },
                privacy,
                Messenger.MaxMessageSize,
                report);
            
            Assert.AreEqual(Levels.Authentication | Levels.Reportable, request.Header.SecurityLevel);
            Assert.AreEqual(ByteTool.Convert(bytes), request.ToBytes());
        }

        [Test]
        [Category("Default")]
        public void TestConstructorV2AuthMd5PrivDes()
        {
            const string bytes = "30 81 80 02  01 03 30 0F  02 02 6C 99  02 03 00 FF" +
                                 "E3 04 01 07  02 01 03 04  38 30 36 04  0D 80 00 1F" +
                                 "88 80 E9 63  00 00 D6 1F  F4 49 02 01  14 02 01 35" +
                                 "04 07 6C 65  78 6D 61 72  6B 04 0C 80  50 D9 A1 E7" +
                                 "81 B6 19 80  4F 06 C0 04  08 00 00 00  01 44 2C A3" +
                                 "B5 04 30 4B  4F 10 3B 73  E1 E4 BD 91  32 1B CB 41" +
                                 "1B A1 C1 D1  1D 2D B7 84  16 CA 41 BF  B3 62 83 C4" +
                                 "29 C5 A4 BC  32 DA 2E C7  65 A5 3D 71  06 3C 5B 56" +
                                 "FB 04 A4";
            MD5AuthenticationProvider auth = new MD5AuthenticationProvider(new OctetString("testpass"));
            IPrivacyProvider privacy = new DESPrivacyProvider(new OctetString("passtest"), auth);
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(0x6C99),
                    new Integer32(0xFFE3),
                    Levels.Authentication | Levels.Privacy | Levels.Reportable),
                new SecurityParameters(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    new Integer32(0x14),
                    new Integer32(0x35),
                    new OctetString("lexmark"),
                    new OctetString(ByteTool.Convert("80  50 D9 A1 E7 81 B6 19 80  4F 06 C0")),
                    new OctetString(ByteTool.Convert("00 00 00  01 44 2C A3 B5"))),
                new Scope(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    OctetString.Empty,
                    new GetRequestPdu(
                        0x3A25,
                        new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")) })),
                privacy,
                null);
            Assert.AreEqual(Levels.Authentication | Levels.Privacy | Levels.Reportable, request.Header.SecurityLevel);
            Assert.AreEqual(ByteTool.Convert(bytes), request.ToBytes());
        }

        [Test]
        [Category("Default")]
        public void TestConstructorV3AuthMd5()
        {
            const string bytes = "30 73" +
                                 "02 01  03 " +
                                 "30 0F " +
                                 "02  02 35 41 " +
                                 "02  03 00 FF E3" +
                                 "04 01 05" +
                                 "02  01 03" +
                                 "04 2E  " +
                                 "30 2C" +
                                 "04 0D  80 00 1F 88 80 E9 63 00  00 D6 1F F4  49 " +
                                 "02 01 0D  " +
                                 "02 01 57 " +
                                 "04 05 6C 65 78  6C 69 " +
                                 "04 0C  1C 6D 67 BF  B2 38 ED 63 DF 0A 05 24  " +
                                 "04 00 " +
                                 "30 2D  " +
                                 "04 0D 80 00  1F 88 80 E9 63 00 00 D6  1F F4 49 " +
                                 "04  00 " +
                                 "A0 1A 02  02 01 AF 02 01 00 02 01  00 30 0E 30  0C 06 08 2B  06 01 02 01 01 03 00 05  00";
            IPrivacyProvider pair = new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("testpass")));
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(13633),
                    new Integer32(0xFFE3),
                    Levels.Authentication | Levels.Reportable),
                new SecurityParameters(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    new Integer32(0x0d),
                    new Integer32(0x57),
                    new OctetString("lexli"),
                    new OctetString(ByteTool.Convert("1C 6D 67 BF  B2 38 ED 63 DF 0A 05 24")),
                    OctetString.Empty),
                new Scope(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    OctetString.Empty,
                    new GetRequestPdu(
                        0x01AF,
                        new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"), new Null()) })),
                pair,
                null);
            Assert.AreEqual(Levels.Authentication | Levels.Reportable, request.Header.SecurityLevel);
            Assert.AreEqual(ByteTool.Convert(bytes), request.ToBytes());
        }

        [Test]
        [Category("Default")]
        public void TestConstructorV3AuthSha()
        {
            const string bytes = "30 77 02 01  03 30 0F 02  02 47 21 02  03 00 FF E3" +
                                 "04 01 05 02  01 03 04 32  30 30 04 0D  80 00 1F 88" +
                                 "80 E9 63 00  00 D6 1F F4  49 02 01 15  02 02 01 5B" +
                                 "04 08 6C 65  78 74 75 64  69 6F 04 0C  7B 62 65 AE" +
                                 "D3 8F E3 7D  58 45 5C 6C  04 00 30 2D  04 0D 80 00" +
                                 "1F 88 80 E9  63 00 00 D6  1F F4 49 04  00 A0 1A 02" +
                                 "02 56 FF 02  01 00 02 01  00 30 0E 30  0C 06 08 2B" +
                                 "06 01 02 01  01 03 00 05  00";
            IPrivacyProvider pair = new DefaultPrivacyProvider(new SHA1AuthenticationProvider(new OctetString("password")));
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(0x4721),
                    new Integer32(0xFFE3),
                    Levels.Authentication | Levels.Reportable),
                new SecurityParameters(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    new Integer32(0x15),
                    new Integer32(0x015B),
                    new OctetString("lextudio"),
                    new OctetString(ByteTool.Convert("7B 62 65 AE D3 8F E3 7D  58 45 5C 6C")),
                    OctetString.Empty),
                new Scope(
                    new OctetString(ByteTool.Convert("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    OctetString.Empty,
                    new GetRequestPdu(
                        0x56FF,
                        new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"), new Null()) })),
                pair,
                null);
            Assert.AreEqual(Levels.Authentication | Levels.Reportable, request.Header.SecurityLevel);
            Assert.AreEqual(ByteTool.Convert(bytes), request.ToBytes());
        }
  
        [Test]
        [Category("Default")]
        public void TestDiscoveryV3()
        {
            const string bytes = "30 3A 02 01 03 30 0F 02 02 6A 09 02 03 00 FF E3" +
                                 " 04 01 04 02 01 03 04 10 30 0E 04 00 02 01 00 02" +
                                 " 01 00 04 00 04 00 04 00 30 12 04 00 04 00 A0 0C" +
                                 " 02 02 2C 6B 02 01 00 02 01 00 30 00";
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(0x6A09),
                    new Integer32(0xFFE3),
                    Levels.Reportable),
                new SecurityParameters(
                    OctetString.Empty,
                    Integer32.Zero,
                    Integer32.Zero,
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    OctetString.Empty,
                    OctetString.Empty,
                    new GetRequestPdu(0x2C6B, new List<Variable>())),
                DefaultPrivacyProvider.DefaultPair,
                null
               );
            string test = ByteTool.Convert(request.ToBytes());
            Assert.AreEqual(bytes, test);
        }

        [Test]
        [Category("Default")]
        public void TestToBytes()
        {
            const string s = "30 27 02 01  01 04 06 70  75 62 6C 69  63 A0 1A 02" +
                             "02 4B ED 02  01 00 02 01  00 30 0E 30  0C 06 08 2B" +
                             "06 01 02 01  01 01 00 05  00                      ";
            byte[] expected = ByteTool.Convert(s);
            GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"), new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
            Assert.AreEqual(expected, message.ToBytes());
        }
        
        [Test]
        [Category("Default")]
        public async Task TestResponseAsync()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            engine.Listener.AddBinding(new IPEndPoint(IPAddress.Loopback, 16100));
            engine.Start();

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"), new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
            
            var users1 = new UserRegistry();
            var response = await message.GetResponseAsync(new IPEndPoint(IPAddress.Loopback, 16100), users1, socket);

            engine.Stop();
            Assert.AreEqual(SnmpType.ResponsePdu, response.TypeCode());
        }
        
        [Test]
        [Category("Default")]
        public void TestResponse()
        {
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            engine.Listener.AddBinding(new IPEndPoint(IPAddress.Loopback, 16101));
            engine.Start();

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"), new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
            
            const int time = 1500;
            var response = message.GetResponse(time, new IPEndPoint(IPAddress.Loopback, 16101), socket);
            Assert.AreEqual(0x4bed, response.RequestId());

            engine.Stop();
        }

        [Test]
        [Category("Default")]
        public async void TestResponses()
        {
            var start = 16102;
            var end = start + 320;
            var engine = CreateEngine();
            engine.Listener.ClearBindings();
            for (var index = start; index < end; index++)
            {
                engine.Listener.AddBinding(new IPEndPoint(IPAddress.Loopback, index));
            }

            var time = DateTime.Now;
            engine.Start();
            Console.WriteLine(DateTime.Now - time);
            
            const int timeout = 100000;
            for (int index = start; index < end; index++)
            //Parallel.For(start, end, async index =>
            {
                GetRequestMessage message = new GetRequestMessage(index, VersionCode.V2, new OctetString("public"), new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Console.WriteLine("manager [{0}]{1}", Thread.CurrentThread.ManagedThreadId, DateTime.UtcNow);
                //var response = message.GetResponse(timeout, new IPEndPoint(IPAddress.Loopback, index), socket);
                var response =
                    await
                        message.GetResponseAsync(new IPEndPoint(IPAddress.Loopback, index), new UserRegistry(), socket);
                Console.WriteLine("manager [{0}]{1}", Thread.CurrentThread.ManagedThreadId, DateTime.UtcNow);
                watch.Stop();
                Console.WriteLine("manager {0}: {1}: port {2}", index, watch.Elapsed, ((IPEndPoint)socket.LocalEndPoint).Port);
                Assert.AreEqual(index, response.RequestId());
            }
            // );

            engine.Stop();
        }

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

            var users = new UserRegistry();
            users.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            users.Add(new OctetString("authen"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            users.Add(new OctetString("privacy"), new DESPrivacyProvider(new OctetString("privacyphrase"),
                                                                         new MD5AuthenticationProvider(new OctetString("authentication"))));

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
            return new SnmpEngine(pipelineFactory, new Listener { Users = users }, new EngineGroup());
        }

        [Test]
        [Category("Default")]
        public void TestTimeOut()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            GetRequestMessage message = new GetRequestMessage(0x4bed, VersionCode.V2, new OctetString("public"), new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) });

            const int time = 1500;
            var timer = new System.Diagnostics.Stopwatch();            
            timer.Start();
            //IMPORTANT: test against an agent that doesn't exist.
            Assert.Throws<TimeoutException>(() => message.GetResponse(time, new IPEndPoint(IPAddress.Parse("8.8.8.8"), 161), socket));
            timer.Stop();            
            
            long elapsedMilliseconds = timer.ElapsedMilliseconds;
            Console.WriteLine(@"elapsed: " + elapsedMilliseconds);
            Console.WriteLine(@"timeout: " + time);
            Assert.LessOrEqual(time, elapsedMilliseconds);

            // FIXME: these values are valid on my machine openSUSE 11.2. (lex)
            // This test case usually fails on Windows, as strangely WinSock API call adds an extra 500-ms.
            if (SnmpMessageExtension.IsRunningOnMono)
            {
                Assert.LessOrEqual(elapsedMilliseconds, time + 100);
            }
        }
    }
}
#pragma warning restore 1591