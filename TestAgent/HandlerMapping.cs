using System;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class HandlerMapping
    {
        private readonly string _version;
        private readonly string _command;
        private readonly IMessageHandler _handler;

        public HandlerMapping(string version, string command, IMessageHandler handler)
        {
            _version = version;
            _command = command;
            _handler = handler;
        }

        public HandlerMapping(string version, string command, string type, string assembly)
        {
            _version = version;
            _command = command;
            _handler = CreateMessageHandler(assembly, type);
        }

        private static IMessageHandler CreateMessageHandler(string assemblyName, string type)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string name = assembly.GetName().Name;
                if (string.Compare(name, assemblyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return (IMessageHandler) Activator.CreateInstance(assembly.GetType(type));
                }
            }

            return (IMessageHandler)Activator.CreateInstance(AppDomain.CurrentDomain.Load(assemblyName).GetType(type));
        }

        public IMessageHandler Handler
        {
            get { return _handler; }
        }

        public bool CanHandle(ISnmpMessage message)
        {
            if (!VersionMatched(message))
            {
                return false;
            }

            if (!CommandMatched(message))
            {
                return false;
            }

            return true;
        }

        private bool CommandMatched(ISnmpMessage message)
        {
            if (StringEquals(_command, "*"))
            {
                return true;
            }

            return StringEquals(_command + "RequestPdu", message.Pdu.TypeCode.ToString());
        }

        private bool VersionMatched(ISnmpMessage message)
        {
            if (StringEquals(_version, "*"))
            {
                return true;
            }

            return StringEquals(message.Version.ToString(), _version);
        }

        private static bool StringEquals(string left, string right)
        {
            return string.Compare(left, right, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}