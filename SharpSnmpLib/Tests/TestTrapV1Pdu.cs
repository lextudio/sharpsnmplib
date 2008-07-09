/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
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
			TrapV1Pdu pdu = new TrapV1Pdu(new ObjectIdentifier(new uint[] {1, 3, 6, 1, 4, 1, 2162, 1000, 2}),
			                              new IP("127.0.0.1"),
			                              GenericCode.EnterpriseSpecific,
			                              new Integer32(12),
			                              new TimeTicks(16352),
			                              new List<Variable>() {v});
			byte[] bytes = pdu.ToMessageBody(VersionCode.V1, "public").ToBytes();
			TrapV1Message message = (TrapV1Message)MessageFactory.ParseMessage(bytes);
			Assert.AreEqual("127.0.0.1", message.AgentAddress.ToString());
			Assert.AreEqual(GenericCode.EnterpriseSpecific, message.Generic);
			Assert.AreEqual(12, message.Specific);
			Assert.AreEqual(16352, message.TimeStamp);
			Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 2162, 1000, 2}, message.Enterprise.ToNumerical());
			Assert.AreEqual(1, message.Variables.Count);
			Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, message.Variables[0].Id.ToNumerical());
			Assert.AreEqual("TrapTest", message.Variables[0].Data.ToString());
		}
	}
}
#pragma warning restore 1591

