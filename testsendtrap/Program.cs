/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/9
 * Time: 21:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Lextm.SharpSnmpLib;
using System.Threading;

namespace TestSendTrap
{
	class Program
	{
        public static void Main(string[] args)
        {
            IPAddress address;
            if (args.Length == 1)
            {
                address = IPAddress.Parse(args[0]);
            }
            else
            {
                address = IPAddress.Loopback;
            }

            Agent.SendTrapV1(new IPEndPoint(address, 162), IPAddress.Loopback,
                                                      new OctetString("public"),
                                                      new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                                                      GenericCode.ColdStart,
                                                      0,
                                                      0,
                                                      new List<Variable>());

            Thread.Sleep(50);


            Agent.SendTrapV2(VersionCode.V2, new IPEndPoint(address, 162), 
                                                 new OctetString("public"),
                                                      new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                                                      0,
                                                      new List<Variable>());
            Thread.Sleep(50);

            try
            {
                Agent.SendInform(VersionCode.V2, new IPEndPoint(address, 162), new OctetString("public"), new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                                          0,
                                          new List<Variable>(), 2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
	}
}