using System.IO;
using Lextm.SharpSnmpLib.Mib;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    class TestIntegerType
    {
        [Test]
        public void TestEnumerable()
        {
            const string test = "SomeEnum ::= INTEGER {first(1), second(2)}";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Integer);

            IntegerType i = new IntegerType("module", "name", lexer);
            Assert.IsTrue(i.IsEnumeration);
            Assert.AreEqual(1, i["first"]);
            Assert.AreEqual("second", i[2]);
        }

        [Test]
        public void TestRanges()
        {
            const string test = "SomeEnum ::= INTEGER (8 | 5 .. 20 | 31 .. 60 )";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Integer);

            IntegerType i = new IntegerType("module", "name", lexer);
            Assert.IsFalse(i.IsEnumeration);
            Assert.IsTrue(i.Contains(8));
            Assert.IsTrue(i.Contains(5));
            Assert.IsTrue(i.Contains(15));
            Assert.IsTrue(i.Contains(20));
            Assert.IsTrue(i.Contains(35));
            Assert.IsFalse(i.Contains(4));
            Assert.IsFalse(i.Contains(-9));
            Assert.IsFalse(i.Contains(25));
            Assert.IsFalse(i.Contains(61));
        }
    }
}
