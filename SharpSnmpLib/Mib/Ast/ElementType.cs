namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ElementType
    {
        public string Name;
        public Tag Tag;
        public TagDefault TagDefault;
        public ISmiType Subtype;
        public bool Optional;
        public bool Default;
        public ISmiValue Value;
    }
}