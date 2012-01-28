namespace Lextm.SharpSnmpLib.Mib
{
    public class IncludeTypeConstraintElement : ConstraintElement
    {
        public ISmiType ConstraintType { get; set; }

        public bool Includes { get; set; }
    }
}