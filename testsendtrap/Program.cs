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

namespace TestSendTrap
{
	class Program
	{
		public static void Main(string[] args)
		{
		    
            TrapV1Message message = new TrapV1Message(VersionCode.V1,
                                                      IPAddress.Parse("127.0.0.1"),
                                                      new OctetString("public"),
                                                      new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                                                      GenericCode.ColdStart,
                                                      0,
                                                      0,
                                                      new List<Variable>());
            message.Send(IPAddress.Loopback, 162);
            /*
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(@"d:\send1.dat"));
            writer.Write(message.ToBytes());
            writer.Close();
            //*/
            
            
            TrapV2Message m2 = new TrapV2Message(VersionCode.V2,
                                                 new OctetString("public"),
                                                      new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                                                      0,
                                                      new List<Variable>());
            m2.Send(IPAddress.Loopback, 162);
            /*
            writer = new BinaryWriter(File.OpenWrite(@"d:\send2.dat"));
            writer.Write(m2.ToBytes());
            writer.Close();
            //*/
           
            InformRequestMessage m3 = new InformRequestMessage(VersionCode.V2, new OctetString("public"), new ObjectIdentifier(new uint[] { 1, 3, 6 }),
                                                      0,
                                                      new List<Variable>());
            try
            {
                m3.Send(IPAddress.Loopback, 2000, 162);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            } 
            /*
            writer = new BinaryWriter(File.OpenWrite(@"d:\send3.dat"));
            writer.Write(m3.ToBytes());
            writer.Close();
            //*/
            
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}