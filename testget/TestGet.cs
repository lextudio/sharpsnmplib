using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Lextm.SharpSnmpLib;
using Mono.Options;

class TestGet
{
    static void Main(string[] args)
    {
        string community = "public";
        bool show_help   = false;
        bool show_version = false;
        VersionCode version = VersionCode.V1;
        int timeout = 1000; 
        int retry = 0;

        OptionSet p = new OptionSet()
            .Add("c:", "-c for community name, (default is public)", delegate (string v) { if (v != null) community = v; })
            .Add("h|?|help", "-h, -?, -help for help.", delegate (string v) { show_help = v != null; })
            .Add("v", "-v to display version number of this application.", delegate (string v) { show_version = v != null; })
            .Add("t:", "-t for timeout value (unit is second).", delegate (string v) { timeout = int.Parse(v) * 1000; })
            .Add("r:", "-r for retry count (default is 0)", delegate (string v) { retry = int.Parse(v); })
            .Add("V|version:", "-V or -version for SNMP version (v1, v2 are currently supported)", delegate (string v) { version = (VersionCode)Enum.Parse(typeof(VersionCode), v, true); });
        
        List<string> extra = p.Parse (args);
        
        if (show_help)
        {
            Console.WriteLine("The syntax is similar to Net-SNMP. http://www.net-snmp.org/docs/man/snmpget.html");
            return;
        }
        
        if (show_version)
        {
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            return;
        }

        if (extra.Count < 2)
        {
            Console.WriteLine("The syntax is similar to Net-SNMP. http://www.net-snmp.org/docs/man/snmpget.html");
            return;
        }
        
        IPAddress ip;
        bool parsed = IPAddress.TryParse(extra[0], out ip);
        if (!parsed)
        {
            ip = IPAddress.Loopback;
            foreach (IPAddress address in Dns.GetHostAddresses(extra[0]))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = address;
                    break;
                }
            }
        }
        
        try
        {
            List<Variable> vList = new List<Variable>();
            for (int i = 1; i < extra.Count; i++)
            {
                Variable test = new Variable(new ObjectIdentifier(extra[i]));
                vList.Add(test);
            }
            
            foreach (Variable variable in Manager.Get(version, new IPEndPoint(ip, 161), new OctetString(community), vList, timeout))
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

