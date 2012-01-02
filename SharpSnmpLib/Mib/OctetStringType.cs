namespace Lextm.SharpSnmpLib.Mib
{
    public class OctetStringType : ISmiType
    {
        public string Name { get; set; }

        public Constraint Constraint { get; set; }
    }
}