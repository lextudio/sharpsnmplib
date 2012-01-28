using System;

namespace Lextm.SharpSnmpLib.Mib
{
    public class NumberLiteralValue : ISmiValue
    {
        public long? Value { get; set; }
        [CLSCompliant(false)]
        public ulong? UnsignedValue { get; set; }

        public NumberLiteralValue(long value)
        {
            Value = value;
            UnsignedValue = null;
        }

        [CLSCompliant(false)]
        public NumberLiteralValue(ulong value)
        {
            if (value <= long.MaxValue)
            {
                Value = (long)value;
                UnsignedValue = null;
            }
            else
            {
                Value = null;
                UnsignedValue = value;
            }
        }
    }
}