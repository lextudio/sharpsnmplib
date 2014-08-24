// SNMP error code enum.
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

namespace Lextm.SharpSnmpLib
{
    using System;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Error code for SNMP operations. (0-5 are first defined in SNMP v1, and others are added in v2)
    /// </summary>
    [DataContract]
    public enum ErrorCode
    {
        /// <summary>
        /// There was no problem performing the request.
        /// </summary>
        NoError = 0,
        
        /// <summary>
        /// The response to your request was too big to fit into one response.
        /// </summary>
        TooBig = 1,
        
        /// <summary>
        /// An agent was asked to get or set an OID that it can't find; i.e., the OID doesn't exist.
        /// </summary>
        NoSuchName = 2,
        
        /// <summary>
        /// A read-write or write-only object was set to an inconsistent value.
        /// </summary>
        BadValue = 3,
        
        /// <summary>
        /// This error is generally not used. The noSuchName error is equivalent to this one.
        /// </summary>
        ReadOnly = 4,
        
        /// <summary>
        /// This is a catch-all error. If an error occurs for which none of the previous messages is appropriate, a genError is issued.
        /// </summary>
        GenError = 5,
        
        /// <summary>
        /// A set to an inaccessible variable was attempted. This typically occurs when the variable has an ACCESS type of not-accessible.
        /// </summary>
        NoAccess = 6,
        
        /// <summary>
        /// An object was set to a type that is different from its definition. This error will occur if you try to set an object that is of type INTEGER to a string, for example.
        /// </summary>
        WrongType = 7,
        
        /// <summary>
        /// An object's value was set to something other than what it calls for. For instance, a string can be defined to have a maximum character size. This error occurs if you try to set a string object to a value that exceeds its maximum length.
        /// </summary>
        WrongLength = 8,
        
        /// <summary>
        /// A set operation was attempted using the wrong encoding for the object being set.
        /// </summary>
        WrongEncoding = 9,
        
        /// <summary>
        /// A variable was set to a value it doesn't understand. This can occur when a read-write is defined as an enumeration, and you try to set it to a value that is not one of the enumerated types.
        /// </summary>
        WrongValue = 10,
        
        /// <summary>
        /// You tried to set a nonexistent variable or create a variable that doesn't exist in the MIB.
        /// </summary>
        NoCreation = 11,
        
        /// <summary>
        /// A MIB variable is in an inconsistent state, and is not accepting any set requests.
        /// </summary>
        InconsistentValue = 12,
        
        /// <summary>
        /// No system resources are available to perform a set.
        /// </summary>
        ResourceUnavailable = 13,
        
        /// <summary>
        /// This is a catch-all error for set failures.
        /// </summary>
        CommitFailed = 14,
        
        /// <summary>
        /// A set failed and the agent was unable to roll back all the previous sets up until the point of failure.
        /// </summary>
        UndoFailed = 15,
        
        /// <summary>
        /// An SNMP command could not be authenticated; in other words, someone has supplied an incorrect community string.
        /// </summary>
        AuthorizationError = 16,
        
        /// <summary>
        /// A variable will not accept a set, even though it is supposed to.
        /// </summary>
        NotWritable = 17,
        
        /// <summary>
        /// You attempted to set a variable, but that attempt failed because the variable was in some kind of inconsistent state.
        /// </summary>
        InconsistentName = 18
    }
}
