namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class DefinedType : ISmiType
    {
        public string Name { get; set; }

        public string Module { get; set; }

        public Constraint Constraint { get; set; }
    }
}