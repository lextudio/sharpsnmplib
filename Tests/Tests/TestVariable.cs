/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using NUnit.Framework;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
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
            List<Variable> vList = new List<Variable>();
            vList.Add(v);

            Sequence varbindSection = Variable.Transform(vList);
            Assert.AreEqual(1, varbindSection.Count);
            Sequence varbind = (Sequence)varbindSection[0];
            Assert.AreEqual(2, varbind.Count);
        }
    }
}
#pragma warning restore 1591