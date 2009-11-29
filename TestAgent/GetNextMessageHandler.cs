using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class GetNextMessageHandler : IMessageHandler
    {
        private readonly ISnmpMessage _message;
        private readonly ObjectStore _store;
        private ErrorCode _status;
        private int _index;

        public GetNextMessageHandler(ISnmpMessage message, ObjectStore store)
        {
            _message = message;
            _store = store;
        }

        public IList<Variable> Handle()
        {
            _status = ErrorCode.NoError;
            _index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in _message.Pdu.Variables)
            {
                _index++;
                ISnmpObject next = _store.GetNextObject(v.Id);
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