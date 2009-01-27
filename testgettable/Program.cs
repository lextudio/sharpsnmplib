using System;
using Lextm.SharpSnmpLib;
using System.Net;
using System.IO;
using Lextm.SharpSnmpLib.Mib;

namespace TestGetTable
{
    class Program
    {
        static StreamWriter writer = new StreamWriter(File.OpenWrite("result.txt"));

        static void Main(string[] args)
        {
            try
            {
                Variable[,] table = Manager.GetTable(VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("public"),
                    new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 2, 2 }), 5000, ObjectRegistry.Default);
                for (int row = 0; row < table.GetUpperBound(0); row++)
                {
                    for (int col = 0; col < table.GetUpperBound(1); col++)
                    {
                        writer.Write(table[row, col].Data + ", ");
                    }
                    writer.WriteLine();
                }
                writer.Close(); 
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
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
