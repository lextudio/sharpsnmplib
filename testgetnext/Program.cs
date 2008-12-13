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
                List<Variable> vList = new List<Variable>();
                vList.Add(new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0")));
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 161);
                GetNextRequestMessage message = new GetNextRequestMessage(VersionCode.V1,
                                                                          new OctetString("public"),
                                                                          vList);
                GetResponseMessage response = message.GetResponse(1000, endpoint);
                if (response.ErrorStatus != ErrorCode.NoError)
                {
                    throw SharpErrorException.Create(
                        "error in response",
                        endpoint.Address,
                        response.ErrorStatus,
                        response.ErrorIndex,
                        response.Variables[response.ErrorIndex - 1].Id);
                }

                Variable variable = response.Variables[0];
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