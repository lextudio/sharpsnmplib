namespace Lextm.SharpSnmpLib.Mib
{
    public class ElementType : ISmiType
    {
        public string Name { get; set; }
        public Tag Tag;
        public TagDefault TagDefault;
        public ISmiType Subtype;
        public bool Optional;
        public bool Default;
        public ISmiValue Value;
    }
}