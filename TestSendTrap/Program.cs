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
using Lextm.SharpSnmpLib;
using System.Net;

namespace TestSendTrap
{
	class Program
	{
		public static void Main(string[] args)
		{
			TrapV1Message message = new TrapV1Message(VersionCode.V1, 
			                                          IPAddress.Parse("127.0.0.1"),
			                                          "public",
			                                          new ObjectIdentifier(new uint[] {1,3,6}),
			                                          GenericCode.ColdStart,
			                                          0,
			                                          0, 
			                                          new List<Variable>());
			message.Send(IPAddress.Parse("127.0.0.1"), 162);
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}