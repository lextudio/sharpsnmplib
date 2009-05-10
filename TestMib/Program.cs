using System;
using Lextm.SharpSnmpLib.Tests;

namespace TestMib
{
    class Program
    {
        static void Main(string[] args)
        {
            //ObjectRegistry.TestLoadFile(@"D:\lextm\documents\SharpDevelop Projects\SharpSnmpLib\SharpSnmpLib\Resources\SNMPv2-TM.txt");
            //ObjectRegistry.Instance.CompileFolder(@"D:\lextm\documents\Downloads\MIBS", "*.mib");
            //ObjectRegistry.TestLoadFile(@"D:\lextm\documents\Downloads\MIBS\SNMPv2-PDU.mib");

            new TestObjectRegistry().TestSYMMIB_MIB_MIB();
            
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}


