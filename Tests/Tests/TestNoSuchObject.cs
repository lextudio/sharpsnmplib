/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/2/15
 * Time: 20:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
	[TestFixture]
	public class TestNoSuchObject
	{
		[Test]
		public void TestToBytes()
		{
			NoSuchObject obj = new NoSuchObject();
			Assert.AreEqual(new byte[] { 0x80, 0x00 }, obj.ToBytes());
		}
	}
}
