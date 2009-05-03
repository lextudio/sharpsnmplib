using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Mono.Options;
using Lextm.SharpSnmpLib.Security;

namespace TestGet
{
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
            SecurityLevel level = SecurityLevel.None | SecurityLevel.Reportable;
            string user = string.Empty;

            OptionSet p = new OptionSet()
                .Add("c:", "-c for community name, (default is public)", delegate (string v) { if (v != null) community = v; })
                .Add("l:", "-l for security level, (default is noAuthNoPriv)", delegate (string v) {
                    if (v == "noAuthNoPriv")
                    {
                        level = SecurityLevel.None | SecurityLevel.Reportable;
                    }
                    else if (v == "authNoPriv")
                    {
                        level = SecurityLevel.Authentication | SecurityLevel.Reportable;
                    }
                    else if (v == "authPriv")
                    {
                        level = SecurityLevel.Authentication | SecurityLevel.Privacy | SecurityLevel.Reportable;
                    }
                })
                .Add("u:", "-u for security name", delegate(string v) { user = v; })
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
                foreach (IPAddress address in Dns.GetHostAddresses(extra[0]))
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = address;
                        break;
                    }
                }
            
                if (ip == null)
                {
                    Console.WriteLine("invalid host or wrong IP address found: " + extra[0]);
                    return;
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

                IPEndPoint receiver = new IPEndPoint(ip, 161);
                if (version != VersionCode.V3)
                {
                    foreach (
                        Variable variable in
                            Messenger.Get(version, receiver, new OctetString(community), vList, timeout))
                    {
                        Console.WriteLine(variable);
                    }

                    return;
                }

                GetRequestMessage request = new GetRequestMessage(0, version, new OctetString(user), vList);
                if ((level | SecurityLevel.Authentication) == SecurityLevel.Authentication)
                {
                }
                else
                {
                    request.Authentication = DefaultAuthenticationProvider.Instance;
                }

                if ((level | SecurityLevel.Privacy) == SecurityLevel.Privacy)
                {
                }
                else
                {
                    request.Privacy = DefaultPrivacyProvider.Instance;
                }

                GetResponseMessage response = request.GetResponseV3(timeout, receiver, Messenger.GetSocket(receiver.AddressFamily)); 
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
}