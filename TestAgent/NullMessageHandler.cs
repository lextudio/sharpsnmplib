using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class NullMessageHandler : IMessageHandler
    {
        public NullMessageHandler(ObjectStore store)
        {}

        public IList<Variable> Handle(ISnmpMessage message)
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
