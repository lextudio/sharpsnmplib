// Operation exception type.
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

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Operation exception of #SNMP.
    /// </summary>
    [DataContract]
    public class OperationException : SnmpException
    {
        /// <summary>
        /// Agent address.
        /// </summary>
        protected IPAddress Agent { get; set; }

        /// <summary>
        /// Creates a <see cref="OperationException"/> instance.
        /// </summary>
        public OperationException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="OperationException"/> instance with a specific <see cref="string"/>.
        /// </summary>
        /// <param name="message"></param>
        public OperationException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="OperationException"/> instance with a specific <see cref="string"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OperationException(string message, Exception inner) : base(message, inner) 
        { 
        }

        /// <summary>
        /// Details on operation.
        /// </summary>
        protected override string Details
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}. Agent: {1}", Message, Agent);
            }
        }
     
        /// <summary>
        /// Creates a <see cref="OperationException"/> with a specific <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="agent">Agent address</param>
        public static OperationException Create(string message, IPAddress agent)
        {
            var ex = new OperationException(message) { Agent = agent };
            return ex;
        }
    }
}
