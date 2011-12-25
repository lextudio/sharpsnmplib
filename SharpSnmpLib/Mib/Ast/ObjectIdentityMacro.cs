namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectIdentityMacro : ISmiType, IEntity
    {
        public string Reference;

        public ObjectIdentityMacro(EntityStatus status, string description)
        {
            
        }

        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
    }
}