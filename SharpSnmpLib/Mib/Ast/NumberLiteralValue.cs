namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NumberLiteralValue : ISmiValue
    {
        public long? Value { get; set; }
        public ulong? UnsignedValue { get; set; }

        public NumberLiteralValue(long value)
        {
            Value = value;
            UnsignedValue = null;
        }

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