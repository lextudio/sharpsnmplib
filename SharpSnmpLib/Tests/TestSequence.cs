/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:57
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
	public class TestSequence
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestConstructor()
		{
			Sequence array = new Sequence(new List<int>() {1, 2, 3});
		}
		[Test]
		public void TestToBytes()
		{
			Sequence a = Variable.ConvertTo(
				new List<Variable>() {
					new Variable(
						new ObjectIdentifier(new uint[] {1,3,6,1,4,1,2162,1001,21,0}),
						new OctetString("TrapTest"))
				});
			byte[] bytes = a.ToBytes();
			ISnmpData data = SnmpDataFactory.CreateSnmpData(bytes);
			Assert.AreEqual(SnmpType.Sequence, data.TypeCode);
			Sequence array = (Sequence)data;
			Assert.AreEqual(1, array.Items.Count);
			ISnmpData item = array.Items[0];
			Assert.AreEqual(SnmpType.Sequence, item.TypeCode);
			Sequence v = (Sequence)item;
			Assert.AreEqual(2, v.Items.Count);
			Assert.AreEqual(SnmpType.ObjectIdentifier, v.Items[0].TypeCode);
			ObjectIdentifier o = (ObjectIdentifier)v.Items[0];
			Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, o.ToNumerical());
			Assert.AreEqual(SnmpType.OctetString, v.Items[1].TypeCode);
			Assert.AreEqual("TrapTest", v.Items[1].ToString());
		}
	}
}
#pragma warning restore 1591

