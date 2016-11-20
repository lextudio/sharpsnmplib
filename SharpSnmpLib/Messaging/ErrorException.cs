// Error exception type.
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
    /// Error exception of #SNMP. Raised when an error message is received.
    /// </summary>
    [DataContract]
    public sealed class ErrorException : OperationException
    {
        /// <summary>
        /// Message body.
        /// </summary>
        public ISnmpMessage Body { get; private set; }

        /// <summary>
        /// Creates a <see cref="ErrorException"/> instance.
        /// </summary>
        public ErrorException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="ErrorException"/> instance with a specific <see cref="string"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public ErrorException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="ErrorException"/> instance with a specific <see cref="string"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public ErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Details on error.
        /// </summary>
        protected override string Details
        {
            get
            {
                var pdu = Body.Pdu();
                var index = pdu.ErrorIndex.ToInt32();
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}. {1}. Index: {2}. Errored Object ID: {3}",
                    Message,
                    pdu.ErrorStatus.ToErrorCode(),
                    index.ToString(CultureInfo.InvariantCulture),
                    index == 0 ? null : pdu.Variables[index - 1].Id);
            }
        }
         
        /// <summary>
        /// Creates a <see cref="ErrorException"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="agent">Agent address.</param>
        /// <param name="body">Error message body.</param>
            /// <returns></returns>
        public static ErrorException Create(string message, IPAddress agent, ISnmpMessage body)
        {
            var ex = new ErrorException(message) { Agent = agent, Body = body };
            return ex;
        }
    }
}
