using System;
using System.Reflection;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Handler mapping class, who is used to map incoming messages to their handlers.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class HandlerMapping
    {
        private readonly string _version;
        private readonly string _command;
        private readonly IMessageHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerMapping"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="command">The command.</param>
        /// <param name="handler">The handler.</param>
        public HandlerMapping(string version, string command, IMessageHandler handler)
        {
            _version = version;
            _command = command;
            _handler = handler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerMapping"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="command">The command.</param>
        /// <param name="type">The type.</param>
        /// <param name="assembly">The assembly.</param>
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

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <value>The handler.</value>
        public IMessageHandler Handler
        {
            get { return _handler; }
        }

        /// <summary>
        /// Determines whether this instance can handle the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///     <c>true</c> if this instance can handle the specified message; otherwise, <c>false</c>.
        /// </returns>
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