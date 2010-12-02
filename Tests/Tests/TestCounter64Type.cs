
using NUnit.Framework;
using Lextm.SharpSnmpLib.Mib;
using System.IO;
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    class TestCounter64Type
    {
        [Test]
        public void TestEnumerable()
        {
            const string test = "SomeEnum ::= Counter64";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            string name = lexer.NextSymbol.ToString();
            lexer.NextSymbol.Expect(Symbol.Assign);
            lexer.NextSymbol.Expect(Symbol.Counter64);

            Counter64Type i = new Counter64Type("module", "name", lexer);
        }
    }
}
