namespace Lextm.SharpSnmpLib.Mib
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