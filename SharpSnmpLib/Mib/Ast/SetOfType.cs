namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class SetOfType : ISmiType
    {
        public string Name { get; set; }

        public Constraint Constraint { get; set; }

        public ISmiType Subtype { get; set; }
    }
}