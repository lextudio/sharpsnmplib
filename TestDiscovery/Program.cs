/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/9
 * Time: 12:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace TestDiscovery
{
    class Program
    {
        public static void Main(string[] args)
        {
            Discoverer discoverer = new Discoverer();
            discoverer.AgentFound += discoverer_AgentFound;
            discoverer.Discover(VersionCode.V2, new IPEndPoint(IPAddress.Broadcast, 161), new OctetString("public"), 6000);
            
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        static void discoverer_AgentFound(object sender, AgentFoundEventArgs e)
        {
            Console.WriteLine(e.Agent + " announces " + e.Variable.Data);
        }
    }
}