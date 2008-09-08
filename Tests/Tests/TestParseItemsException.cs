/*
 * Created by SharpDevelop.
 * User: steves
 * Date: 2008/9/4
 * Time: 15:25
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
    public class TestParseItemsException
    {
        [Test]
        // [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestConstructor()
        {
            Sequence a = Variable.Transform(
                new List<Variable>() {
                   new Variable(
                        new ObjectIdentifier(new uint[] {1,3,6,1,2,1,2,2,1,22,1}),
                        new ObjectIdentifier(new uint[] {0,0}))
                });
        }
    }
}
#pragma warning restore 1591