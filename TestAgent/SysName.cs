using System;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysName: IScalarObject
    {
        private static readonly ObjectIdentifier _id = new ObjectIdentifier("1.3.6.1.2.1.1.5.0");
        private OctetString _name = new OctetString(Environment.MachineName);

        public ISnmpData Data
        {
            get { return _name; }
            set
            {
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("data");
                }

                _name = (OctetString)value;
            }
        }

        public ObjectIdentifier Id
        {
            get { return _id; }
        }
    }
}
