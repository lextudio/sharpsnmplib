/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/30
 * Time: 10:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Lextm.SharpSnmpLib.Mib;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestConstructHelper
    {
        [Test]
        public void TestMethod()
        {
            Lexer lexer = new Lexer();
            lexer.Parse(new StringReader("{ iso org(3) dod(6) 1 }"));
            string parent;
            uint value;
            ConstructHelper.ParseOidValue(lexer, out parent, out value);
            Assert.AreEqual("iso.org(3).dod(6)", parent);
            Assert.AreEqual(1, value);
        }
        
        [Test]
        public void TestMethod1()
        {
            Lexer lexer = new Lexer();
            lexer.Parse(new StringReader("{ iso(1) std(0) iso8802(8802) ieee802dot1(1)" + Environment.NewLine +
"     ieee802dot1mibs(1) 1 }"));
            string parent;
            uint value;
            ConstructHelper.ParseOidValue(lexer, out parent, out value);
            Assert.AreEqual("iso(1).std(0).iso8802(8802).ieee802dot1(1).ieee802dot1mibs(1)", parent);
            Assert.AreEqual(1, value);
        }
        
        
    }
}
