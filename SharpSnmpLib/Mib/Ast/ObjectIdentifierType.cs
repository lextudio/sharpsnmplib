namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectIdentifierType : ISmiType, IEntity
    {
        public ObjectIdentifierType(string moduleName, string name, string parent, uint value)
        {
            ModuleName = moduleName;
            Name = name;
            Value = value;
            Parent = parent;
        }

        public ObjectIdentifierType()
        {
        }

        public uint Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
    }
}