using System;
using System.Collections.Generic;
using System.Text;

namespace Browser
{
    class MibBrowserException : Exception
    {
        public MibBrowserException(string message) : base(message) { }
    }
}
