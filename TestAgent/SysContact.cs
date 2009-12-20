using System;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysContact : IScalarObject
    {
        private static readonly ObjectIdentifier Identifier = new ObjectIdentifier("1.3.6.1.2.1.1.4.0");
        private OctetString _contact = new OctetString(Environment.UserName);

        public ISnmpData Data
        {
            get { return _contact; }
            set
            {
                if (value.TypeCode != SnmpType.OctetString)
                {
                    throw new ArgumentException("wrong data type", "value");
                }

                _contact = (OctetString)value;
            }
        }

        public ObjectIdentifier Id
        {
            get { return Identifier; }
        }
    }
}
