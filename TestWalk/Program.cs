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
using Lextm.SharpSnmpLib;
using System.Net;
using System.IO;

namespace TestWalk
{
	class Program
	{
        static StreamWriter writer = new StreamWriter(File.OpenWrite("result.txt"));

		public static void Main(string[] args)
		{
			Variable seed;
            Variable next = new Variable(new uint[] { 1, 3, 6, 1, 2, 1, 2, 2 });//1.3.6.1.2.1.2.2
			do
			{
				seed = next;
			}
			while (HasNext(seed, out next));
            writer.Close();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		static bool HasNext(Variable seed, out Variable next)
		{
			bool result;
			try {
				GetNextRequestMessage message = new GetNextRequestMessage(VersionCode.V1,
				                                                          IPAddress.Parse("127.0.0.1"),
				                                                          "public",
				                                                          new List<Variable>(1) {
				                                                          	seed});
				
				next = message.Send(1000)[0];
				result = true;
			} catch (SharpErrorException ex) {
				next = null;
				result = false;
                writer.WriteLine(ex.Details);
			}
			if (result) {
				writer.WriteLine(next.ToString());
			}	
			return result;
		}
	}
}