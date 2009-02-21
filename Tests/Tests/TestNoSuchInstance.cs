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
#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Tests
{
	[TestFixture]
	public class TestNoSuchInstance
	{
		[Test]
		public void TestToBytes()
		{
			NoSuchInstance obj = new NoSuchInstance();
			Assert.AreEqual(new byte[] { 0x81, 0x00 }, obj.ToBytes());
		}
	}
}
#pragma warning restore 1591, 0618