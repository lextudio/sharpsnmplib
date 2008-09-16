using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Browser
{
    [Serializable]
    public class MibBrowserException : Exception
    {
        public MibBrowserException(string message) : base(message) { }
    }
}