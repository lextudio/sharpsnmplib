namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ErrorMacro : ISmiType
    {
        public string Identifier;
        public ISmiType Subtype;
        public string Name { get; set; }
    }
}