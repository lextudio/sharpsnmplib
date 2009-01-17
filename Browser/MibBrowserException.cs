using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lextm.SharpSnmpLib.Browser
{
    [Serializable]
    public class BrowserException : Exception
    {
        public BrowserException() { }
        
        public BrowserException(string message) : base(message) { }
        
        public BrowserException(string message, Exception inner) : base(message, inner) { }
    
        protected BrowserException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}