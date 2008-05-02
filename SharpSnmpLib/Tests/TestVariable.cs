/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 12:15
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
	public class TestVariable
	{
		[Test]
		public void TestToBytes()
		{
			Variable v = new Variable(
					new ObjectIdentifier(new uint[] {1,3,6,1,4,1,2162,1001,21,0}),
					new OctetString("TrapTest"));
			SnmpArray varbindSection = Variable.ConvertTo(new List<Variable>() {v});
			Assert.AreEqual(1, varbindSection.Items.Count);
			SnmpArray varbind = (SnmpArray)varbindSection.Items[0];
			Assert.AreEqual(2, varbind.Items.Count);
		}
	}
}
