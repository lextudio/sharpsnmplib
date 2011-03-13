using NUnit.Framework;
using Lextm.SharpSnmpLib.Mib;
using System.IO;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    class TestUnsignedType
    {
        [Test]
        public void TestRanges()
        {
            const string test = "SomeEnum ::= Unsigned32 (8 | 10 ..20 | 31 .. 60 )";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Unsigned32);

            UnsignedType i = new UnsignedType("module", "name", lexer);
            Assert.IsTrue(i.Contains(8));
            Assert.IsTrue(i.Contains(10));
            Assert.IsTrue(i.Contains(15));
            Assert.IsTrue(i.Contains(20));
            Assert.IsTrue(i.Contains(35));
            Assert.IsFalse(i.Contains(4));
            Assert.IsFalse(i.Contains(-9));
            Assert.IsFalse(i.Contains(25));
            Assert.IsFalse(i.Contains(61));
        }

        [Test]
        public void TestOverlappingRanges1()
        {
            const string test = "SomeEnum ::= Gauge32 (8 | 5 .. 20 |31 .. 60 )";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Gauge32);

            Assert.Throws<MibException>(() => new UnsignedType("module", "name", lexer));
        }

        [Test]
        public void TestOverlappingRanges2()
        {
            const string test = "SomeEnum ::= Gauge32 (8 | 8 .. 20 | 31 .. 60 )";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Gauge32);

            Assert.Throws<MibException>(() => new UnsignedType("module", "name", lexer));
        }

        [Test]
        public void TestOverlappingRanges3()
        {
            const string test = "SomeEnum ::= Unsigned32 (8 | 8 | 31 .. 60 )";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Unsigned32);

            Assert.Throws<MibException>(() => new UnsignedType("module", "name", lexer));
        }

        [Test]
        public void TestOverlappingRanges4()
        {
            const string test = "SomeEnum ::= Unsigned32 (8 | 5..20 | 31 .. 60 )";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Unsigned32);

            Assert.Throws<MibException>(() => new UnsignedType("module", "name", lexer));
        }
    }
}
