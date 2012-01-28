namespace Lextm.SharpSnmpLib.Mib
{
    public class CharacterStringType : ISmiType
    {
        public string Name { get; set; }

        public CharacterSet CharacterSet { get; set; }

        public Constraint Constraint { get; set; }
    }
}