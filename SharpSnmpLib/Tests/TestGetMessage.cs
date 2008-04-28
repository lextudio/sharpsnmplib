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
	public class TestGetMessage
	{
		[Test]
		public void TestToBytes()
		{
			byte[] expected = Resource.get;
			GetMessage message = new GetMessage(IPAddress.Parse("127.0.0.1"), 
			                                    "public",
			                                    new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 })));
			byte[] bytes = message.ToBytes();
//			int length = (expected.Length < bytes.Length)? expected.Length: bytes.Length;
//			for (int i = 0; i < length; i++)
//			{
//				Assert.AreEqual(expected[i], bytes[i], "index is " + i);
//			}
			Assert.AreEqual(expected, bytes);
		}
	}
}
