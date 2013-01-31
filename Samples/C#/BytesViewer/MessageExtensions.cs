using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BytesViewer
{
    internal static class MessageExtensions
    {
        public static void Fill(this IList<ISnmpMessage> messages, TreeView tree)
        {
            foreach (var message in messages)
            {
                message.Fill(tree);
            }
        }

        public static void Fill(this ISnmpMessage message, TreeView tree)
        {
            var messageNode = tree.Nodes.Add(string.Format("Version {0}", message.Version));
            var header = messageNode.Nodes.Add("header");
            header.Nodes.Add(string.Format("message id {0}", message.Header.MessageId));
            header.Nodes.Add(string.Format("max size {0}", message.Header.MaxSize));
            header.Nodes.Add(string.Format("security level {0}", message.Header.SecurityLevel.GetString()));
            
            var parameter = messageNode.Nodes.Add("parameters");
            parameter.Nodes.Add(string.Format("user {0}", message.Parameters.UserName));
            parameter.Nodes.Add(string.Format("engine id {0}", message.Parameters.EngineId.ToHexString()));
            parameter.Nodes.Add(string.Format("engine boots {0}", message.Parameters.EngineBoots));
            parameter.Nodes.Add(string.Format("engine time {0}", message.Parameters.EngineTime));
            parameter.Nodes.Add(string.Format("checksum {0}", message.Parameters.AuthenticationParameters.ToHexString()));
            parameter.Nodes.Add(string.Format("checksum broken {0}", message.Parameters.IsInvalid));
            parameter.Nodes.Add(string.Format("encryption token {0}", message.Parameters.PrivacyParameters.ToHexString()));
            
            var scope = messageNode.Nodes.Add("scope");
            if (message.TypeCode() == SnmpType.Unknown)
            {
                scope.Nodes.Add("decryption error");
            }
            else if (OctetString.IsNullOrEmpty(message.Parameters.PrivacyParameters))
            {
                scope.Nodes.Add(string.Format("context name {0}", message.Scope.ContextName));
                scope.Nodes.Add(string.Format("context engine id {0}", message.Scope.ContextEngineId.ToHexString()));
                var pdu = scope.Nodes.Add(string.Format("pdu type {0}", message.Scope.Pdu.TypeCode));
                pdu.Nodes.Add(string.Format("request id {0}", message.Scope.Pdu.RequestId));
                pdu.Nodes.Add(string.Format("error status {0}", message.Scope.Pdu.ErrorStatus.ToErrorCode()));
                pdu.Nodes.Add(string.Format("error index {0}", message.Scope.Pdu.ErrorIndex));
                var variables = pdu.Nodes.Add(string.Format("variable count {0}", message.Scope.Pdu.Variables.Count));
                foreach (var variable in message.Scope.Pdu.Variables)
                {
                    variables.Nodes.Add(variable.ToString());
                }
            }
            else
            {
                scope.Nodes.Add("encrypted data");
            }

            tree.ExpandAll();
        }

        public static string GetString(this Levels levels)
        {
            var result = new StringBuilder();
            if ((levels & Levels.Authentication) == Levels.Authentication)
            {
                result.Append("authenticated|");
            }

            if ((levels & Levels.Privacy) == Levels.Privacy)
            {
                result.Append("encrypted|");
            }

            if ((levels & Levels.Reportable) == Levels.Reportable)
            {
                result.Append("reportable|");
            }

            if (result.Length > 0)
            {
                result.Length--;
            }
            else
            {
                return "none";
            }

            return result.ToString();
        }
    }
}
