// Timeout exception type.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Timeout exception type of #SNMP.
    /// </summary>
    [DataContract]
    public sealed class TimeoutException : OperationException
    {
        /// <summary>
        /// The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Creates a <see cref="TimeoutException"/> instance.
        /// </summary>
        public TimeoutException() 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="TimeoutException"/> instance with a specific <see cref="string"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public TimeoutException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="TimeoutException"/> instance with a specific <see cref="string"/> and an <see cref="Exception"/> instance.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public TimeoutException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="TimeoutException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "TimeoutException: timeout: {0}", Timeout.ToString(CultureInfo.InvariantCulture));
        }
        
        /// <summary>
        /// Creates a <see cref="TimeoutException"/>.
        /// </summary>
        /// <param name="agent">Agent address</param>
        /// <param name="timeout">Timeout</param>
        /// <returns></returns>
        public static TimeoutException Create(IPAddress agent, int timeout)
        {
            if (agent == null)
            {
                throw new ArgumentNullException(nameof(agent));
            }
            
            var ex = new TimeoutException($"Request timed out after {timeout}-ms.") { Agent = agent, Timeout = timeout };
            return ex;
        }
    }
}
