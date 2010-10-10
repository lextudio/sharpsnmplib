using System;
using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Event arguments for agent found event.
    /// </summary>
    public sealed class AgentFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the agent.
        /// </summary>
        /// <value>The agent.</value>
        public IPEndPoint Agent { get; private set; }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <value>The variable.</value>
        /// <remarks>If the agent is SNMP v3, this is <code>null</code>.</remarks>
        public Variable Variable { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentFoundEventArgs"/> class.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="variable">The variable.</param>
        public AgentFoundEventArgs(IPEndPoint agent, Variable variable)
        {
            Agent = agent;
            Variable = variable;
        }
    }
}