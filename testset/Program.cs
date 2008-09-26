/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 20:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using Lextm.SharpSnmpLib;
using System.Collections.Generic;

namespace TestSet
{
	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				Variable test = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }), 
				                                             new OctetString("Beijing"));
			    Manager.Set(VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("private"), new List<Variable>() {test}, 5000);
			    Manager.Set(VersionCode.V2, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("private"), new List<Variable>() {test}, 5000);
			}
			catch (SharpOperationException ex)
			{
				Console.WriteLine(ex);
			}

			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}