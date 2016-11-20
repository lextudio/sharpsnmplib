// SNMP exception.
// Copyright (C) 2008 Lex Li.
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
using System.Runtime.Serialization; 

namespace Lextm.SharpSnmpLib
{    
    /// <summary>
    /// Base exception type of #SNMP.
    /// </summary>
    [DataContract]
    public class SnmpException : Exception
    {
        /// <summary>
        /// Creates a <see cref="SnmpException"/>.
        /// </summary>
        public SnmpException() 
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="SnmpException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SnmpException(string message) : base(message) 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SnmpException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SnmpException(string message, Exception inner) 
            : base(message, inner) 
        {
        }

#if !NETFX_CORE
        ///// <summary>
        ///// Creates a <see cref="SnmpException"/> instance.
        ///// </summary>
        ///// <param name="info">Info</param>
        ///// <param name="context">Context</param>
        //protected SnmpException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
#endif

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SnmpException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Details;
        }        
     
        /// <summary>
        /// Details on operation.
        /// </summary>
        protected virtual string Details
        {
            get
            {
                return base.ToString();
            }
        }
    }
}
