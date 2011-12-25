using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NotificationTypeMacro : ISmiType, IEntity
    {
        public IList<ISmiValue> Objects = new List<ISmiValue>();
        public EntityStatus Status;
        public string Description;
        public string Reference;
        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
    }
}