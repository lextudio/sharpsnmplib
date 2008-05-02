/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 15:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace SharpSnmpLib.Tests
{
	[TestFixture]
	public class TestGetRequestPdu
	{
		[Test]
		public void Test()
		{
			byte[] expected = new byte[] {
				48, 129, 
				47, 2, 129, 1, 0, 4, 129, 6, 112, 117, 98, 108, 105, 99, 160, 129, 31, 2, 129, 1, 10, 2, 129, 1, 0, 2, 129, 1, 0, 48, 129, 16, 48, 129, 13, 6, 129, 8, 43, 6, 1, 2, 1, 1, 6, 0, 
				5, 0};
			Variable v = new Variable(new ObjectIdentifier(new uint[] {1, 3, 6, 1, 2, 1, 1, 6, 0}));
			PduCounter.Clear();
			GetRequestPdu pdu = new GetRequestPdu(0, 0, new List<Variable>() {v});
			Assert.AreEqual(expected, pdu.ToMessageBody(VersionCode.V1, "public").ToBytes());
		}
	}
}
