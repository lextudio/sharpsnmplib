namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class IncludeTypeConstraintElement : ConstraintElement
    {
        public ISmiType ConstraintType { get; set; }

        public bool Includes { get; set; }
    }
}