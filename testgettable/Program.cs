using System;
using System.IO;
using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;

namespace TestGetTable
{
    class Program
    {
        static readonly StreamWriter writer = new StreamWriter("result.txt");

        static void Main(string[] args)
        {
            try
            {
                Variable[,] table = Messenger.GetTable(VersionCode.V1, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("public"),
                    new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 2, 2 }), 5000, 10, DefaultObjectRegistry.Instance);
                writer.WriteLine("V1 table");
                for (int row = 0; row < table.GetUpperBound(0); row++)
                {
                    for (int col = 0; col < table.GetUpperBound(1); col++)
                    {
                        writer.Write(table[row, col].Data + ", ");
                    }
                    writer.WriteLine();
                }

                table = Messenger.GetTable(VersionCode.V2, new IPEndPoint(IPAddress.Loopback, 161), new OctetString("public"),
                    new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 2, 2 }), 5000, 10, DefaultObjectRegistry.Instance);
                writer.WriteLine("V2 table");
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
