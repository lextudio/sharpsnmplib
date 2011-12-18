namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NumberLiteralValue : ISmiValue
    {
        public long Value { get; set; }

        public NumberLiteralValue(long value)
        {
            Value = value;
        }
    }
}