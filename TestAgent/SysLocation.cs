using System;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysLocation : IScalarObject
    {
        private static readonly ObjectIdentifier Identifier = new ObjectIdentifier("1.3.6.1.2.1.1.6.0");
        private OctetString _location = OctetString.Empty;

        public ISnmpData Data
        {
            get { return _location; }
            set
            {
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("wrong data type", "value");
                }

                _location = (OctetString)value;
            }
        }

        public ObjectIdentifier Id
        {
            get { return Identifier; }
        }
    }
}
