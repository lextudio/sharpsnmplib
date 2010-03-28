/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 3/14/2010
 * Time: 1:18 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Logger class, who logs message processed to the rolling log file.
    /// </summary>    
    internal class RollingLogger : ILogger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Empty = "-";
        
        public RollingLogger()
        {
            if (log.IsInfoEnabled)
            {
                log.Info(string.Format(CultureInfo.InvariantCulture, "#Software: #SNMP Agent {0}", System.Reflection.Assembly.GetEntryAssembly().GetName().Version));
                log.Info("#Version: 1.0");
                log.Info(string.Format(CultureInfo.InvariantCulture, "#Date: {0}", DateTime.UtcNow));
                log.Info("#Fields: date time s-ip cs-method cs-uri-stem s-port cs-username c-ip sc-status cs-version time-taken");
            }
        }

        public void Log(SnmpContext context)
        {
            TimeSpan timeTaken = DateTime.Now.Subtract(context.CreatedTime);
            if (log.IsInfoEnabled)
            {
                log.Info(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
                    DateTime.UtcNow,
                    Empty,
                    context.Request.Pdu == null ? Empty : context.Request.Pdu.TypeCode.ToString(),
                    GetStem(context.Request.Pdu),
                    context.Listener.Port,
                    context.Request.Parameters.UserName,
                    context.Sender.Address,
                    (context.Response == null) ? Empty : context.Response.Pdu.ErrorStatus.ToErrorCode().ToString(),
                    context.Request.Version,
                    timeTaken));
            }
        }

        private static string GetStem(ISnmpPdu pdu)
        {
            if (pdu == null)
            {
                return Empty;
            }

            StringBuilder result = new StringBuilder();
            foreach (Variable v in pdu.Variables)
            {
                result.AppendFormat("{0};", v.Id);
            }

            if (result.Length > 0)
            {
                result.Length--;
            }

            return result.ToString();
        }
    }
}
