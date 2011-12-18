using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NotificationTypeMacro : ISmiType
    {
        public IList<ISmiValue> Objects;
        public EntityStatus Status;
        public string Description;
        public string Reference;
        public string Name { get; set; }
    }
}