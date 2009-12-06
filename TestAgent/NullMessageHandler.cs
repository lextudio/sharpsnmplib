using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class NullMessageHandler : IMessageHandler
    {
        public IList<Variable> Handle(ISnmpMessage message, ObjectStore _store)
        {
            return null;
        }

        public ErrorCode ErrorStatus
        {
            get { return ErrorCode.NoError; }
        }

        public int ErrorIndex
        {
            get { return 0; }
        }
    }
}
