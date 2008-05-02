/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 18:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Net;

using NUnit.Framework;
namespace SharpSnmpLib.Tests
{
	/// <summary>
	/// Description of TestGetMessage.
	/// </summary>
	[TestFixture]
	public class TestGetRequestMessage
	{
		[Test]
		public void Test()
		{
			byte[] expected = Resource.get;
            ISnmpMessage message = MessageFactory.ParseMessage(expected);
            Assert.AreEqual(SnmpType.GetRequestPDU, message.TypeCode);
            GetRequestPdu pdu = (GetRequestPdu)message.Pdu;
            Assert.AreEqual(1, pdu.Variables.Count);
            Variable v = pdu.Variables[0];
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }, v.Id.ToOid());
            Assert.AreEqual(typeof(Null), v.Data.GetType());
		}
	}
}
