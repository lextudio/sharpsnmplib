// Agent found event args.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

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
        /// <remarks>If the agent is SNMP v3, this is <c>null</c>.</remarks>
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