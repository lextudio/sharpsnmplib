using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Lextm.SharpSnmpLib.Security;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var community = new OctetString("public");
            var request = new GetRequestMessage(2, VersionCode.V1, community, new List<Variable>
            {
                new Variable(new ObjectIdentifier("0.0"))
            });
            var registry = new UserRegistry();
            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            var dest = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 161);
            Console.WriteLine("snapshot 1");
            Console.ReadKey();
            Console.WriteLine("started");
            Task.Run(async () =>
            {
                for (int index = 0; index < 1000000; index++)
                {
                    var response = await request.GetResponseAsync(
                        dest,
                        registry,
                        socket);
                    Console.WriteLine(response.Scope.Pdu.ErrorStatus);
                }
            });

            Console.WriteLine("snapshot 2");
            Console.ReadKey();
        }
    }
}
