/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Lextm.SharpSnmpLib;
using System.Net;

namespace TestGetNext
{
	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				GetNextRequestMessage message = new GetNextRequestMessage(VersionCode.V1, 
				                                                          IPAddress.Parse("127.0.0.1"),
				                                                          "public",
				                                                          new List<Variable>(1) {
				                                                          	new Variable(
				                                                          		new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 })});
				
				Variable variable = message.Send(1000, 161)[0];
				Console.WriteLine(variable.ToString());
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
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}