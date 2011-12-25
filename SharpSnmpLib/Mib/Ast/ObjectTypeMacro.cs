using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectTypeMacro : ISmiType, IEntity
    {
        public ISmiType Syntax;
        public IList<NamedBit> NamedBits;
        public string Units;
        public AccessType AccessType;
        public Access MibAccess;
        public PibAccess PibAccess;
        public IList<NamedBit> InstallErrors;
        public ISmiValue PibReference;
        public ISmiValue PibTag;
        public EntityStatus Status;
        public string Reference;
        public IList<ISmiValue> UniquenessValues;
        public ISmiValue PibExtends;
        public IList<ISmiValue> MibIndex;
        public IList<string> DefaultValueBits;
        public ISmiValue MibArguments;
        public ISmiValue PibIndex;
        public ISmiValue DefaultValue;
        public string Description { get; set; }
        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
    }
}