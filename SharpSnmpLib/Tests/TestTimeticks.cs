/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
	[TestFixture]
	public class TestTimeticks
	{
		[Test]
		public void TestConstructor()
		{
			TimeTicks time = new TimeTicks(15);
			Assert.AreEqual(15, time.ToInt32());
		}
		[Test]
		public void TestToBytes()
		{
			TimeTicks time = new TimeTicks(16352);
			ISnmpData data = SnmpDataFactory.CreateSnmpData(time.ToBytes());
			Assert.AreEqual(data.TypeCode, SnmpType.TimeTicks);
			Assert.AreEqual(time.ToString(), data.ToString());
		}
	}
}
#pragma warning restore 1591