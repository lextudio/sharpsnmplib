using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// GET NEXT message handler.
    /// </summary>
    internal class GetNextMessageHandler : IMessageHandler
    {
        private ErrorCode _status;
        private int _index;

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public ErrorCode ErrorStatus
        {
            get { return _status; }
        }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public int ErrorIndex
        {
            get { return _index; }
        }
    }
}