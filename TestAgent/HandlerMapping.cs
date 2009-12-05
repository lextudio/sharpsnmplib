using System;
using System.Reflection;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class HandlerMapping
    {
        private readonly string _version;
        private readonly string _command;
        private readonly IMessageHandler _handler;

        public HandlerMapping(string version, string command, string type, string assembly, ObjectStore store)
        {
            _version = version;
            _command = command;
            _handler = CreateMessageHandler(assembly, type, store);
        }

        private static IMessageHandler CreateMessageHandler(string assemblyName, string type, ObjectStore store)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string name = assembly.GetName().Name;
                if (string.Compare(name, assemblyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return (IMessageHandler) Activator.CreateInstance(assembly.GetType(type), new object[] {store});
                }
            }

            return (IMessageHandler)Activator.CreateInstance(AppDomain.CurrentDomain.Load(assemblyName).GetType(type), new object[] { store });
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