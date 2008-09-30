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
            //ObjectRegistry.TestLoadFile(@"D:\lextm\documents\SharpDevelop Projects\SharpSnmpLib\SharpSnmpLib\Resources\SNMPv2-TM.txt");
            ObjectRegistry.TestLoadFolder(@"D:\lextm\documents\Downloads\MIBS", "*.mib");
            //ObjectRegistry.TestLoadFile(@"D:\lextm\documents\Downloads\MIBS\SNMPv2-PDU.mib");
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}


