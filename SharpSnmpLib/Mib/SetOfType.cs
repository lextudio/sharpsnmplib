namespace Lextm.SharpSnmpLib.Mib
{
    public class SetOfType : ISmiType
    {
        public string Name { get; set; }

        public Constraint Constraint { get; set; }

        public ISmiType Subtype { get; set; }
    }
}