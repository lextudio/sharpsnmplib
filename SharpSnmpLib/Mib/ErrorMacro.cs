namespace Lextm.SharpSnmpLib.Mib
{
    public class ErrorMacro : ISmiType
    {
        public string Identifier;
        public ISmiType Subtype;
        public string Name { get; set; }
    }
}