using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class GetMessageHandler : IMessageHandler
    {
        private readonly ISnmpMessage _message;
        private readonly ObjectStore _store;
        private ErrorCode _status;
        private int _index;

        public GetMessageHandler(ISnmpMessage message, ObjectStore store)
        {
            _store = store;
            _message = message;
        }

        public IList<Variable> Handle()
        {
            _status = ErrorCode.NoError;
            _index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in _message.Pdu.Variables)
            {
                _index++;
                ISnmpObject obj = _store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        Variable item = new Variable(v.Id, obj.Data);
                        result.Add(item);
                    }
                    catch (AccessFailureException)
                    {
                        _status = ErrorCode.NoSuchName;
                    }
                    catch (Exception)
                    {
                        _status = ErrorCode.GenError;
                    }
                }
                else
                {
                    _status = ErrorCode.NoSuchName;
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