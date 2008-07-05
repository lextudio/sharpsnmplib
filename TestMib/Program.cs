using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Tests;
using Lextm.SharpSnmpLib.Mib;

namespace TestMib
{
    class Program
    {
        static void Main(string[] args)
        {
            //new TestLexer().TestParse();
            //new TestMibDocument().TestINET_ADDRESS_MIB();
            //new TestMibDocument().TestNet_Snmp_Examples_MIB();
            //new TestMibDocument().TestSNMPv2_SMI();            
            //new TestMibDocument().Testv2TM();
            TestObjectRegistry test = new TestObjectRegistry();
            test.TestZeroDotZero();//TestSNMPv2MIBNumerical();
        }
    }
}

