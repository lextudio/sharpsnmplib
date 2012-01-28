using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
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

        [CLSCompliant(false)]
        public uint Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }

        public IList<ISmiValue> Objects
        {
            get {
                return _objects;
            }
        }
    }
}