using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Xunit;
using System.IO;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Unit.Messaging
{
    public class TrapV1MessageTestFixture
    {
        [Fact]
        public void TestSendTrap()
        {
            TrapV1Message message = new TrapV1Message(VersionCode.V1, 
                                                      IPAddress.Parse("127.0.0.1"),
                                                      new OctetString("public"),
                                                      new ObjectIdentifier(new uint[] {1,3,6}),
                                                      GenericCode.AuthenticationFailure,
                                                      0, 
                                                      0,
                                                      new List<Variable>());
            byte[] bytes = message.ToBytes();
            ISnmpMessage parsed = MessageFactory.ParseMessages(bytes, new UserRegistry())[0];
            Assert.Equal(SnmpType.TrapV1Pdu, parsed.TypeCode());
            TrapV1Message m = (TrapV1Message)parsed;
            Assert.Equal(GenericCode.AuthenticationFailure, m.Generic);
            Assert.Equal(0, m.Specific);
            Assert.Equal("public", m.Community.ToString());
            Assert.Equal(IPAddress.Parse("127.0.0.1"), m.AgentAddress);
            Assert.Equal(new uint[] {1,3,6}, m.Enterprise.ToNumerical());
            Assert.Equal(0U, m.TimeStamp);
            Assert.Equal(0, m.Variables().Count);
        }
#if !NETSTANDARD
        [Fact]
        public void TestParseNoVarbind()
        {
            byte[] buffer = File.ReadAllBytes(Path.Combine("Resources", "novarbind.dat"));
            ISnmpMessage m = MessageFactory.ParseMessages(buffer, new UserRegistry())[0];
            TrapV1Message message = (TrapV1Message)m;
            Assert.Equal(GenericCode.EnterpriseSpecific, message.Generic);
            Assert.Equal(12, message.Specific);
            Assert.Equal("public", message.Community.ToString());
            Assert.Equal(IPAddress.Parse("127.0.0.1"), message.AgentAddress);
            Assert.Equal(new uint[] { 1,3 }, message.Enterprise.ToNumerical());
            Assert.Equal(16352U, message.TimeStamp);
            Assert.Equal(0, message.Variables().Count);            
        }
        
        [Fact]
        public void TestParseOneVarbind()
        {
            byte[] buffer = File.ReadAllBytes(Path.Combine("Resources", "onevarbind.dat"));
            TrapV1Message message = (TrapV1Message)MessageFactory.ParseMessages(buffer, new UserRegistry())[0];
            Assert.Equal(1, message.Variables().Count);
            Assert.Equal(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1000, 2 }, message.Enterprise.ToNumerical());
            Assert.Equal("TrapTest", message.Variables()[0].Data.ToString());
            Assert.Equal(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, message.Variables()[0].Id.ToNumerical());
        }
        
        [Fact]
        public void TestParseTwoVarbinds()
        {
            byte[] buffer = File.ReadAllBytes(Path.Combine("Resources", "twovarbinds.dat"));
            TrapV1Message message = (TrapV1Message)MessageFactory.ParseMessages(buffer, new UserRegistry())[0];
            Assert.Equal(2, message.Variables().Count);
            Assert.Equal("TrapTest", message.Variables()[0].Data.ToString());
            Assert.Equal(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, message.Variables()[0].Id.ToNumerical());
            Assert.Equal("TrapTest", message.Variables()[1].Data.ToString());
            Assert.Equal(new uint[] {1,3,6,1,4,1,2162,1001,22,0}, message.Variables()[1].Id.ToNumerical());
        }
#endif
    }
}
#pragma warning restore 1591

