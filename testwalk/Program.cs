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
using System.IO;
using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace TestWalk
{
    class Program
    {
        public static void Main(string[] args)
        {
            //*
            using (StreamWriter writer = new StreamWriter("within.txt"))
            {
                writer.WriteLine("within subtree mode");
                IList<Variable> list = new List<Variable>();
                writer.WriteLine("V1 walk");                
                try
                {
                    Messenger.Walk(VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("public"), new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 7, 5 }), list, 1000, WalkMode.WithinSubtree);
                } 
                catch (SharpTimeoutException ex)
                {
                    Console.WriteLine(ex.Details);
                }

                foreach (Variable v in list)
                {
                    writer.WriteLine(v);
                }
                
                list = new List<Variable>();
                writer.WriteLine("V2 walk");
                try
                {
                    Messenger.BulkWalk(VersionCode.V2, new IPEndPoint(IPAddress.Parse("120.89.90.0"),//IPAddress.Loopback,
                                                                      161), new OctetString("public"), new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 7, 5 }), list, 1000, 10, WalkMode.WithinSubtree);
                } 
                catch (SharpTimeoutException ex)
                {
                    Console.WriteLine(ex.Details);
                }

                foreach (Variable v in list)
                {
                    writer.WriteLine(v);
                }
                writer.Close();
            }
            //*/
            
            using (StreamWriter writer = new StreamWriter("default.txt")) 
            {
                writer.WriteLine("default mode");
                IList<Variable> list = new List<Variable>();
                writer.WriteLine("V1 walk");
                ObjectIdentifier o = new ObjectIdentifier("1.3.6.1.2.1.25.2.3.1.6.3");

                try
                {
                    Messenger.Walk(VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("public"), o, list, 1000, WalkMode.Default);
                }
                catch (SharpTimeoutException ex)
                {
                    writer.WriteLine(ex.Details);
                }

                foreach (Variable v in list)
                {
                    writer.WriteLine(v);
                }
                
                list = new List<Variable>();
                writer.WriteLine("V2 walk");
                try
                {
                    Messenger.BulkWalk(VersionCode.V2, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("public"), o, list, 2000, 10, WalkMode.Default);
                }
                catch (SharpTimeoutException ex) 
                {
                    writer.WriteLine(ex.Details);
                }

                foreach (Variable v in list)
                {
                    writer.WriteLine(v);
                }

                writer.Close();
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}