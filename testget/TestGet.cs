using System;
using System.Net;
using Lextm.SharpSnmpLib;
using System.Text;
using System.Collections.Generic;
using NDesk.Options;

class TestGet
{
    // get system.sysLocation on localhost
    static void Main(string[] args)
    {
        string community = "public";
        bool show_help   = false;
        VersionCode version = VersionCode.V1;
        int timeout = 1000;

        OptionSet p = new OptionSet ()
            .Add ("c:", delegate (string v) { if (v != null) community = v; })
            .Add ("h|?|help",  delegate (string v) { show_help = v != null; })
            .Add ("t:", delegate (string v) { timeout = int.Parse(v) * 1000; })
            .Add ("V|version:",   delegate (string v) { version = (VersionCode)Enum.Parse(typeof(VersionCode), v, true); });
        
        List<string> extra = p.Parse (args);
        
        if (show_help)
        {
            Console.WriteLine("The syntax is similar to Net-SNMP. http://www.net-snmp.org/docs/man/snmpget.html");
            return;
        }

        if (extra.Count < 2)
        {
            Console.WriteLine("The syntax is similar to Net-SNMP. http://www.net-snmp.org/docs/man/snmpget.html");
            return;
        }
        
        string ip = extra[0];
        List<Variable> vList = new List<Variable>();
        for (int i = 1; i < extra.Count; i++)
        {
            Variable test = new Variable(new ObjectIdentifier(extra[i]));
            vList.Add(test);
        }
        
        try
        {            
            foreach (Variable variable in Manager.Get(version, new IPEndPoint(IPAddress.Parse(ip), 161), new OctetString(community), vList, timeout))
            {
                Console.WriteLine(variable);
            }
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
    }
}

