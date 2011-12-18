namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectIdentityMacro : ISmiType
    {
        public string Reference;

        public ObjectIdentityMacro(EntityStatus status, string description)
        {
            
        }

        public string Name { get; set; }
    }
}