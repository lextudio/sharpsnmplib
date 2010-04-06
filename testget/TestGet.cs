// typical usage
// snmpget -c=public -v=1 localhost 1.3.6.1.2.1.1.1.0
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Mono.Options;
using Lextm.SharpSnmpLib.Messaging;

namespace SnmpGet
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            string community = "public";
            bool showHelp   = false;
            bool showVersion = false;
            VersionCode version = VersionCode.V1;
            int timeout = 1000;
            int retry = 0;
            Levels level = Levels.None | Levels.Reportable;
            string user = string.Empty;
            string authentication = string.Empty;
            string authPhrase = string.Empty;
            string privacy = string.Empty;
            string privPhrase = string.Empty;
            bool dump = false;

            OptionSet p = new OptionSet()
                .Add("c:", "-c for community name, (default is public)", delegate (string v) { if (v != null) community = v; })
                .Add("l:", "-l for security level, (default is noAuthNoPriv)", delegate (string v)
                                                                                   {
                                                                                       if (v.ToUpperInvariant() == "NOAUTHNOPRIV")
                                                                                       {
                                                                                           level = Levels.None | Levels.Reportable;
                                                                                       }
                                                                                       else if (v.ToUpperInvariant() == "AUTHNOPRIV")
                                                                                       {
                                                                                           level = Levels.Authentication | Levels.Reportable;
                                                                                       }
                                                                                       else if (v.ToUpperInvariant() == "AUTHPRIV")
                                                                                       {
                                                                                           level = Levels.Authentication | Levels.Privacy | Levels.Reportable;
                                                                                       }
                                                                                       else
                                                                                       {
                                                                                           throw new ArgumentException("no such security mode: " + v);
                                                                                       }
                                                                                   })
                .Add("a:", "-a for authentication method (MD5 or SHA)", delegate (string v) { authentication = v; })
                .Add("A:", "-A for authentication passphrase", delegate(string v) { authPhrase = v; })
                .Add("x:", "-x for privacy method", delegate (string v) { privacy = v; })
                .Add("X:", "-X for privacy passphrase", delegate (string v) { privPhrase = v; })
                .Add("u:", "-u for security name", delegate(string v) { user = v; })
                .Add("h|?|help", "-h, -?, -help for help.", delegate (string v) { showHelp = v != null; })
                .Add("V", "-V to display version number of this application.", delegate (string v) { showVersion = v != null; })
                .Add("d", "-d to display message dump", delegate(string v) { dump = true; })
                .Add("t:", "-t for timeout value (unit is second).", delegate (string v) { timeout = int.Parse(v) * 1000; })
                .Add("r:", "-r for retry count (default is 0)", delegate (string v) { retry = int.Parse(v); })
                .Add("v|version:", "-v for SNMP version (v1, v2 and v3 are currently supported)", delegate (string v)
                                                                                               {
                                                                                                   switch (int.Parse(v))
                                                                                                   {
                                                                                                       case 1:
                                                                                                           version = VersionCode.V1;
                                                                                                           break;
                                                                                                       case 2:
                                                                                                           version = VersionCode.V2;
                                                                                                           break;
                                                                                                       case 3:
                                                                                                           version = VersionCode.V3;
                                                                                                           break;
                                                                                                       default:
                                                                                                           throw new ArgumentException("no such version: " + v);
                                                                                                   }
                                                                                               });
			
            List<string> extra = p.Parse (args);
			
            if (showHelp)
            {
                ShowHelp();   
                return;
            }
			
            if (showVersion)
            {
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                return;
            }

            if (extra.Count < 2)
            {
                ShowHelp();
                return;
            }
			
            IPAddress ip;
            bool parsed = IPAddress.TryParse(extra[0], out ip);
            if (!parsed)
            {
                foreach (IPAddress address in Dns.GetHostAddresses(extra[0]))
                {
                    if (address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    ip = address;
                    break;
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
				
                if (string.IsNullOrEmpty(user))
                {
                    Console.WriteLine("User name need to be specified for v3.");
                    return;
                }

                IAuthenticationProvider auth = (level & Levels.Authentication) == Levels.Authentication
                                                   ? GetAuthenticationProviderByName(authentication, authPhrase)
                                                   : DefaultAuthenticationProvider.Instance;

                IPrivacyProvider priv = (level & Levels.Privacy) == Levels.Privacy
                                            ? new DESPrivacyProvider(new OctetString(privPhrase), auth)
                                            : DefaultPrivacyProvider.Instance;

                Discovery discovery = new Discovery(1, 101);
                ReportMessage report = discovery.GetResponse(timeout, receiver);
				
                ProviderPair record = new ProviderPair(auth, priv);
                GetRequestMessage request = new GetRequestMessage(VersionCode.V3, 100, 0, new OctetString(user), vList, record, report);

                ISnmpMessage response = request.GetResponse(timeout, receiver);
                if (dump)
                {
                    Console.WriteLine(ByteTool.Convert(request.ToBytes()));
                }

                if (response.Pdu.ErrorStatus.ToInt32() != 0) // != ErrorCode.NoError
                {
                    throw ErrorException.Create(
                        "error in response",
                        receiver.Address,
                        response);
                }

                foreach (Variable v in response.Pdu.Variables)
                {
                    Console.WriteLine(v);
                }
            }
            catch (SnmpException ex)
            {
                if (ex is OperationException)
                {
                    Console.WriteLine((ex as OperationException).Details);
                }
                else
                {
                    Console.WriteLine(ex);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("#SNMP is available at http://sharpsnmplib.codeplex.com");
        }

        private static IAuthenticationProvider GetAuthenticationProviderByName(string authentication, string phrase)
        {
            if (authentication.ToUpperInvariant() == "MD5")
            {
                return new MD5AuthenticationProvider(new OctetString(phrase));
            }
			
            if (authentication.ToUpperInvariant() == "SHA")
            {
                return new SHA1AuthenticationProvider(new OctetString(phrase));
            }

            throw new ArgumentException("unknown name", "authentication");
        }
    }
}