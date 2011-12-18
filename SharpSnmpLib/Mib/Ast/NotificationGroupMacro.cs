using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NotificationGroupMacro : ISmiType
    {
        private readonly IList<ISmiValue> _notifications = new List<ISmiValue>();
        public EntityStatus Status;
        public string Description;
        public string Reference;

        public NotificationGroupMacro(ISmiValue value)
        {
            Notifications.Add(value);
        }

        public string Name { get; set; }

        public IList<ISmiValue> Notifications
        {
            get { return _notifications; }
        }
    }
}