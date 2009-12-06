using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class GetNextMessageHandler : IMessageHandler
    {
        private ErrorCode _status;
        private int _index;

        public IList<Variable> Handle(ISnmpMessage message, ObjectStore store)
        {
            _status = ErrorCode.NoError;
            _index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Pdu.Variables)
            {
                _index++;
                ISnmpObject next = store.GetNextObject(v.Id);
                if (next == null)
                {
                    _status = ErrorCode.NoSuchName;
                }
                else
                {
                    // TODO: how to handle write only object here?
                    result.Add(new Variable(next.Id, next.Data));
                }

                if (_status != ErrorCode.NoError)
                {
                    return null;
                }
            }

            return result;
        }

        public ErrorCode ErrorStatus
        {
            get { return _status; }
        }

        public int ErrorIndex
        {
            get { return _index; }
        }
    }
}