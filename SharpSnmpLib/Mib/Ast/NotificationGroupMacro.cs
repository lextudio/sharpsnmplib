using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NotificationGroupMacro : ISmiType, IEntity
    {
        private readonly IList<ISmiValue> _notifications = new List<ISmiValue>();
        public EntityStatus Status;
        public string Description;
        public string Reference;

        public NotificationGroupMacro(ISmiValue value)
        {
            Notifications.Add(value);
        }

        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }

        public IList<ISmiValue> Notifications
        {
            get { return _notifications; }
        }
    }
}