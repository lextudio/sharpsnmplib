using System;

namespace Lextm.SharpSnmpLib.Mib
{
    public class ObjectIdentifierType : ISmiType, IEntity
    {
    	[CLSCompliant(false)]
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

        [CLSCompliant(false)]
        public uint Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
    }
}