using SharpSnmpLib.Tests;
using System;
using System.Net;
using SharpSnmpLib;
using Snmp;
using X690;

class TestGet
{
	// get system.sysLocation on localhost
	static void Main(string[] args)
	{
        Manager manager = new Manager();
        try
        {
            Variable variable = manager.Get(IPAddress.Parse("127.0.0.1"), "public",
                new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 })));
            Console.WriteLine(variable.Data);
        }
        catch (SharpSnmpException ex)
        {
            Console.WriteLine(ex);
        }
        Console.WriteLine("Press any key to exit...");
        Console.Read();
	}
} 