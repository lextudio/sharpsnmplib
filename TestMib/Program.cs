using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Mib;

namespace TestMib
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectRegistry.TestLoadFolder(@"D:\lextm\documents\Downloads\MIBS", "*.mib");
            //ObjectRegistry.TestLoadFile(@"D:\lextm\documents\Downloads\MIBS\ARROWPOINT-IPV4-OSPF-MIB.mib");
            Console.Read();
        }
    }
}


