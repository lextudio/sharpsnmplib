namespace Lextm.SharpSnmpLib.Mib
{
    public class DefinedType : ISmiType
    {
        public string Name { get; set; }

        public string Module { get; set; }

        public Constraint Constraint { get; set; }
    }
}