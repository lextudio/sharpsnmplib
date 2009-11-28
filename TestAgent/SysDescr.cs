using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysDescr : IScalarObject
    {
        private static readonly ObjectIdentifier _id = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");

        public ISnmpData Get()
        {
            return new OctetString(string.Format(CultureInfo.InvariantCulture, "{0};{1}", Environment.MachineName, Environment.OSVersion));
        }

        public void Set(ISnmpData data)
        {
            throw new ReadOnlyException();
        }

        public ObjectIdentifier Id
        {
            get
            {
                return _id;
            }
        }
    }
}
