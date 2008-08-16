/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/9
 * Time: 12:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;

using Lextm.SharpSnmpLib;

namespace TestDiscovery
{
    class Program
    {
        public static void Main(string[] args)
        {
            Manager m = new Manager();
            m.Timeout = 60000;
            IDictionary<IPEndPoint, Variable> agents = m.Discover(VersionCode.V2, new IPEndPoint(IPAddress.Broadcast, 161), "public");
            foreach (KeyValuePair<IPEndPoint, Variable> pair in agents)
            {
                Console.WriteLine(pair.Key + " announces " + pair.Value.Data);
            }
            
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}