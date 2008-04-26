/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SharpSnmpLib;

namespace test
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Start watcher");
			
			TrapListener watcher = new TrapListener();
			watcher.Start(162);
			
			Console.WriteLine("Press any key to stop . . . ");
			Console.ReadKey(true);
			watcher.Stop();
			Console.WriteLine("Press any key to exit . . . ");
			Console.Read();
		}
	}
}