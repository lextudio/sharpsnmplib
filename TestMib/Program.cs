using System;
using Lextm.SharpSnmpLib.Tests;

namespace TestMib
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestAst().TestLexerOK();
            
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}


