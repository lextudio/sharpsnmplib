using System;
using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Event arguments for agent found event.
    /// </summary>
    public class AgentFoundEventArgs : EventArgs
    {
        private readonly IPEndPoint _agent;

        /// <summary>
        /// Gets the agent.
        /// </summary>
        /// <value>The agent.</value>
        public IPEndPoint Agent
        {
            get { return _agent; }
        }

        private readonly Variable _variable;

        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <value>The variable.</value>
        public Variable Variable
        {
            get { return _variable; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentFoundEventArgs"/> class.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="variable">The variable.</param>
        public AgentFoundEventArgs(IPEndPoint agent, Variable variable)
        {
            _agent = agent;
            _variable = variable;
        }
    }
}