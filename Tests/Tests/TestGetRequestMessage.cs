/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 18:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using NUnit.Framework;

#pragma warning disable 1591

namespace Lextm.SharpSnmpLib.Tests
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
			ISnmpMessage message = MessageFactory.ParseMessages(expected)[0];
            Assert.AreEqual(SnmpType.GetRequestPdu, message.Pdu.TypeCode);
            GetRequestPdu pdu = (GetRequestPdu)message.Pdu;
            Assert.AreEqual(1, pdu.Variables.Count);
            Variable v = pdu.Variables[0];
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }, v.Id.ToNumerical());
            Assert.AreEqual(typeof(Null), v.Data.GetType());
            Assert.GreaterOrEqual(expected.Length, message.ToBytes().Length);
		}

        [Test]
        public void TestConstructor()
        {
            List<Variable> list = new List<Variable>(1);
            list.Add(new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }), new Null()));
            GetRequestMessage message = new GetRequestMessage(VersionCode.V2, new OctetString("public"), list);
            Assert.GreaterOrEqual(Resource.get.Length, message.ToBytes().Length);
        }
	}
}
#pragma warning restore 1591