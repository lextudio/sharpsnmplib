namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class LiteralValue : ISmiValue
    {
        public string Value { get; set; }

        public LiteralValue(string value)
        {
            Value = value;
        }
    }
}