namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class OctetStringType : ISmiType
    {
        public string Name { get; set; }

        public Constraint Constraint { get; set; }
    }
}