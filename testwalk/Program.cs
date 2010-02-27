/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Mono.Options;

namespace SnmpWalk
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            string community = "public";
            bool dump = true;
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
            WalkMode mode = WalkMode.WithinSubtree;

            OptionSet p = new OptionSet()
                .Add("c:", "-c for community name, (default is public)", delegate(string v) { if (v != null) community = v; })
                .Add("l:", "-l for security level, (default is noAuthNoPriv)", delegate(string v)
                                                                                   {
                                                                                       if (v == "noAuthNoPriv")
                                                                                       {
                                                                                           level = Levels.None | Levels.Reportable;
                                                                                       }
                                                                                       else if (v == "authNoPriv")
                                                                                       {
                                                                                           level = Levels.Authentication | Levels.Reportable;
                                                                                       }
                                                                                       else if (v == "authPriv")
                                                                                       {
                                                                                           level = Levels.Authentication | Levels.Privacy | Levels.Reportable;
                                                                                       }
                                                                                   })
                .Add("a:", "-a for authentication method", delegate(string v) { authentication = v; })
                .Add("A:", "-A for authentication passphrase", delegate(string v) { authPhrase = v; })
                .Add("x:", "-x for privacy method", delegate(string v) { privacy = v; })
                .Add("X:", "-X for privacy passphrase", delegate(string v) { privPhrase = v; })
                .Add("u:", "-u for security name", delegate(string v) { user = v; })
                .Add("h|?|help", "-h, -?, -help for help.", delegate(string v) { showHelp = v != null; })
                .Add("V", "-V to display version number of this application.", delegate(string v) { showVersion = v != null; })
                .Add("d", "-d to display message dump", delegate(string v) { dump = true; })
                .Add("t:", "-t for timeout value (unit is second).", delegate(string v) { timeout = int.Parse(v) * 1000; })
                .Add("r:", "-r for retry count (default is 0)", delegate(string v) { retry = int.Parse(v); })
                .Add("v|version:", "-v for SNMP version (v1, v2 are currently supported)", delegate(string v)
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
                                                                                               })
                .Add("m|mode:", "-m for WALK mode (subtree, all are supported)", delegate(string v)
                                                                                     {
                                                                                         if (v == "subtree")
                                                                                         {
                                                                                             mode = WalkMode.WithinSubtree;
                                                                                         }
                                                                                         else if (v == "all")
                                                                                         {
                                                                                             mode = WalkMode.Default;
                                                                                         }
                                                                                         else
                                                                                         {
                                                                                             throw new ArgumentException("unknown argument: " + v);
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

            if (extra.Count < 1 || extra.Count > 2)
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
                ObjectIdentifier test = extra.Count == 1 ? new ObjectIdentifier("1.3.6.1.2.1") : new ObjectIdentifier(extra[1]);

                IPEndPoint receiver = new IPEndPoint(ip, 161);
                if (version == VersionCode.V1)
                {
                    IList<Variable> result = new List<Variable>();
                    Messenger.Walk(version, receiver, new OctetString(community), test, result, timeout, mode);
                    foreach (Variable variable in result)
                    {
                        Console.WriteLine(variable);
                    }

                    return;
                }
                
                if (version == VersionCode.V2)
                {
                    IList<Variable> result = new List<Variable>();
                    Messenger.BulkWalk(version, receiver, new OctetString(community), test, result, timeout, retry, mode);
                    foreach (Variable variable in result)
                    {
                        Console.WriteLine(variable);
                    }

                    return;
                }
                
                if (version == VersionCode.V3)
                {
                    Console.WriteLine("Not yet implemented for v3");
                    return;
                }
                
                /*
                IAuthenticationProvider auth;
                if ((level & Levels.Authentication) == Levels.Authentication)
                {
                    auth = GetAuthenticationProviderByName(authentication, authPhrase);
                }
                else
                {
                    auth = DefaultAuthenticationProvider.Instance;
                }

                IPrivacyProvider priv;
                if ((level & Levels.Privacy) == Levels.Privacy)
                {
                    priv = new DESPrivacyProvider(new OctetString(privPhrase), auth);
                }
                else
                {
                    priv = DefaultPrivacyProvider.Instance;
                }

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
                // */
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
        }

        private static IAuthenticationProvider GetAuthenticationProviderByName(string authentication, string phrase)
        {
            if (authentication.ToUpper() == "MD5")
            {
                return new MD5AuthenticationProvider(new OctetString(phrase));
            }
            
            if (authentication.ToUpper() == "SHA")
            {
                return new SHA1AuthenticationProvider(new OctetString(phrase));
            }

            throw new ArgumentException("unknown name", "authentication");
        }
        
        private static void ShowHelp()
        {
            Console.WriteLine("#SNMP is available at http://sharpsnmplib.codeplex.com");
        }
    }
}