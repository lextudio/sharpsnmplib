namespace Lextm.SharpSnmpLib.Mib
{
    public class DefinedValue : ISmiValue
    {
        public string Module { get; set; }

        public string Value { get; set; }
    }
}