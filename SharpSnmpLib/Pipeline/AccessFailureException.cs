// Access failure exception class.
// Copyright (C) 2009-2010 Lex Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/28/2009
 * Time: 7:41 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Access failure exception. 
    /// Raised when,
    /// 1. GET operation is performed on a write-only object.
    /// 2. SET operation is performed on a read-only object.
    /// </summary>
    [Serializable]
    public sealed class AccessFailureException : Exception
    {
        /// <summary>
        /// Creates a <see cref="AccessFailureException"/>.
        /// </summary>
        public AccessFailureException() 
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="AccessFailureException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public AccessFailureException(string message) : base(message) 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="AccessFailureException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public AccessFailureException(string message, Exception inner) 
            : base(message, inner) 
        {
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Creates a <see cref="AccessFailureException"/> instance.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        private AccessFailureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        } 
#endif
       
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="AccessFailureException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AccessFailureException: " + Message;
        }
    }
}
