/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using Lextm.SharpSnmpLib.Messaging;
using NUnit.Framework;
using System.Text;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestTrapV1Pdu
    {
        [Test]
        public void TestToTrapMessage()
        {
            Variable v = new Variable(new ObjectIdentifier(new uint[] {1,3,6,1,4,1,2162,1001,21,0}), 
                                      new OctetString("TrapTest"));
            List<Variable> vList = new List<Variable>();
            vList.Add(v);

            TrapV1Pdu pdu = new TrapV1Pdu(new ObjectIdentifier(new uint[] {1, 3, 6, 1, 4, 1, 2162, 1000, 2}),
                                          new IP("127.0.0.1"),
                                          new Integer32((int)GenericCode.EnterpriseSpecific),
                                          new Integer32(12),
                                          new TimeTicks(16352),
                                          vList);
            byte[] bytes = Helper.PackMessage(VersionCode.V1, new OctetString("public"), pdu).ToBytes();
            TrapV1Message message = (TrapV1Message)MessageFactory.ParseMessages(bytes, new Lextm.SharpSnmpLib.Security.UserRegistry())[0];
            Assert.AreEqual("127.0.0.1", message.AgentAddress.ToString());
            Assert.AreEqual(GenericCode.EnterpriseSpecific, message.Generic);
            Assert.AreEqual(12, message.Specific);
            Assert.AreEqual(16352, message.TimeStamp);
            Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 2162, 1000, 2}, message.Enterprise.ToNumerical());
            Assert.AreEqual(1, message.Variables.Count);
            Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, message.Variables[0].Id.ToNumerical());
            Assert.AreEqual("TrapTest", message.Variables[0].Data.ToString());
        }
        
        [Test]
        public void TestToTrapMessageChinese()
        {
            Variable v = new Variable(new ObjectIdentifier(new uint[] {1,3,6,1,4,1,2162,1001,21,0}), 
                                      new OctetString("中国", Encoding.Unicode));
            List<Variable> vList = new List<Variable>();
            vList.Add(v);

            TrapV1Pdu pdu = new TrapV1Pdu(new ObjectIdentifier(new uint[] {1, 3, 6, 1, 4, 1, 2162, 1000, 2}),
                                          new IP("127.0.0.1"),
                                          new Integer32((int)GenericCode.EnterpriseSpecific),
                                          new Integer32(12),
                                          new TimeTicks(16352),
                                          vList);
            byte[] bytes = Helper.PackMessage(VersionCode.V1, new OctetString("public"), pdu).ToBytes();
            TrapV1Message message = (TrapV1Message)MessageFactory.ParseMessages(bytes, new Lextm.SharpSnmpLib.Security.UserRegistry())[0];
            Assert.AreEqual("127.0.0.1", message.AgentAddress.ToString());
            Assert.AreEqual(GenericCode.EnterpriseSpecific, message.Generic);
            Assert.AreEqual(12, message.Specific);
            Assert.AreEqual(16352, message.TimeStamp);
            Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 2162, 1000, 2}, message.Enterprise.ToNumerical());
            Assert.AreEqual(1, message.Variables.Count);
            Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, message.Variables[0].Id.ToNumerical());
            Assert.AreEqual("中国", (message.Variables[0].Data as OctetString).ToString(Encoding.Unicode));
        }
    }
}
#pragma warning restore 1591

