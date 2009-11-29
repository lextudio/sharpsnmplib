using System;
using System.Globalization;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysDescr : IScalarObject
    {
        private static readonly ObjectIdentifier _id = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");
        private readonly OctetString _description = new OctetString(string.Format(CultureInfo.InvariantCulture, "{0};{1}", Environment.MachineName, Environment.OSVersion));

        public ISnmpData Data
        {
            get { return _description; }
            set { throw new AccessFailureException(); }
        }

        public ObjectIdentifier Id
        {
            get { return _id; }
        }
    }
}
