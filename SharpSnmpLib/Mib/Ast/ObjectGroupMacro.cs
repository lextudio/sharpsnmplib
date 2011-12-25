using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectGroupMacro : ISmiType, IEntity
    {
        private readonly IList<ISmiValue> _objects = new List<ISmiValue>();
        public EntityStatus Status;
        public string Description;
        public string Reference;

        public ObjectGroupMacro(ISmiValue value)
        {
            _objects.Add(value);
        }

        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }

        public IList<ISmiValue> Objects
        {
            get {
                return _objects;
            }
        }
    }
}