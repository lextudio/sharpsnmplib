using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Net;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class Logger : IDisposable
    {
        private StreamWriter writer;

        public Logger()
        {
            writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "snmp.log"), true);
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "#Software: #SNMP Suite {0}", Assembly.GetEntryAssembly().GetName().Version));
            writer.WriteLine("#Version: 1.0");
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "#Date: {0}", DateTime.UtcNow));
            writer.WriteLine("#Fields: date time s-ip cs-method cs-uri-stem s-port cs-username c-ip sc-status time-taken");
            writer.AutoFlush = true;
        }

        public void Log(int port, SnmpType type, ISnmpMessage message, IPEndPoint client, long time)
        {
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6} {7} {8}", 
                DateTime.UtcNow, "-", type, GetStem(message.Pdu.Variables), port, message.Parameters.UserName, client.Address, message.Pdu.ErrorStatus.ToErrorCode(), time));
        }

        private static string GetStem(IList<Variable> list)
        {
            StringBuilder result = new StringBuilder();
            foreach (Variable v in list)
            {
                result.AppendFormat("{0};", v.Id);
            }

            if (result.Length > 0)
            {
                result.Length--;
            }

            return result.ToString();
        }

        public void Dispose()
        {
            writer.Dispose();
        }
    }
}
