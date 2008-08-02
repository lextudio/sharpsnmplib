using System;
using System.Net;
using Lextm.SharpSnmpLib;

class TestGet
{
	// get system.sysLocation on localhost
	static void Main(string[] args)
	{
        Manager manager = new Manager();
        try
        {
        	Variable test = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }));
            Variable variable = manager.Get(IPAddress.Parse("127.0.0.1"), 161, "public", test);
            Console.WriteLine(variable.Data);
            
            Variable v2 = manager.Get(VersionCode.V2, IPAddress.Parse("127.0.0.1"), 161, "public", test);
            Console.WriteLine(v2.Data);
        }
        catch (SharpSnmpException ex)
        {
            if (ex is SharpOperationException)
            {
                Console.WriteLine((ex as SharpOperationException).Details);
            }
            else
            {
                Console.WriteLine(ex);
            }
        }
        Console.WriteLine("Press any key to exit...");
        Console.Read();
	}
} 

