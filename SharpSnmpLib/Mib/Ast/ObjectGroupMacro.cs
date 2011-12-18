using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ObjectGroupMacro : ISmiType
    {
        private readonly IList<ISmiValue> _objects = new List<ISmiValue>();
        public EntityStatus Status;
        public string Description;
        public string Reference;

        public ObjectGroupMacro(ISmiValue value)
        {
            _objects.Add(value);
        }

        public string Name { get; set; }

        public IList<ISmiValue> Objects
        {
            get {
                return _objects;
            }
        }
    }
}