namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class TaggedType : ISmiType
    {
        public string Name { get; set; }

        public Tag Tag { get; set; }

        public TagDefault TagDefault { get; set; }

        public ISmiType Subtype { get; set; }
    }
}