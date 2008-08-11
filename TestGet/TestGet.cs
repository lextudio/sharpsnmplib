using System;
using System.Net;
using Lextm.SharpSnmpLib;
using System.Text;
using System.Collections.Generic;

class TestGet
{
	// get system.sysLocation on localhost
	static void Main(string[] args)
	{
	    string ip;
	    if (args.Length == 0)
	    {
	        ip = "127.0.0.1";
	    }
	    else 
	    {
	        ip = args[0];
	    }
        Manager manager = new Manager();
        Variable test = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 }));
        try
        {            
            Variable variable = manager.Get(VersionCode.V1, IPAddress.Parse(ip), 161, "public", new List<Variable>() {test})[0];
            Console.WriteLine(variable.Data);
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
        
        try
        {  
            Variable v2 = manager.Get(VersionCode.V2, IPAddress.Parse(ip), 161, "public", new List<Variable>() {test})[0];
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

