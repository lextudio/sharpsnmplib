namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class DefinedValue : ISmiValue
    {
        public string Module { get; set; }

        public string Value { get; set; }
    }
}