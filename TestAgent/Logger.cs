using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Logger class, who logs message processed to the log file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Logger : IDisposable
    {
        private readonly StreamWriter _writer;
        private const string Empty = "-";

        public Logger()
        {
            _writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "snmp.log"), true);
            _writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "#Software: #SNMP Suite {0}", Assembly.GetEntryAssembly().GetName().Version));
            _writer.WriteLine("#Version: 1.0");
            _writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "#Date: {0}", DateTime.UtcNow));
            _writer.WriteLine("#Fields: date time s-ip cs-method cs-uri-stem s-port cs-username c-ip sc-status version time-taken");
            _writer.AutoFlush = true;
        }

        public void Log(SnmpContext context)
        {
            TimeSpan timeTaken = DateTime.Now.Subtract(context.CreatedTime);
            _writer.WriteLine(string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
                DateTime.UtcNow,
                Empty,
                context.Request.Pdu.TypeCode,
                GetStem(context.Request.Pdu.Variables),
                context.Listener.Port,
                context.Request.Parameters.UserName,
                context.Sender.Address,
                (context.Response == null) ? Empty : context.Response.Pdu.ErrorStatus.ToErrorCode().ToString(),
                context.Request.Version,
                timeTaken));
        }

        private static string GetStem(IEnumerable<Variable> list)
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
            _writer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
