namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectIdentifierType : ISmiType, IEntity
    {
        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
    }
}