namespace Lextm.SharpSnmpLib.Mib
{
    public class ObjectIdentityMacro : ISmiType, IEntity
    {
        public string Reference;

        public ObjectIdentityMacro(EntityStatus status, string description)
        {
            
        }

        public uint Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
    }
}