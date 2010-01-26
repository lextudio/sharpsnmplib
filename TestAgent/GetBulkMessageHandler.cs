using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// GET BULK message handler.
    /// </summary>
    internal class GetBulkMessageHandler : IMessageHandler
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
            // TODO: implement this to conform to RFC.
            _status = ErrorCode.NoError;
            _index = 0;
            IList<Variable> result = new List<Variable>();
            Variable v = message.Pdu.Variables[0];

            Variable temp = v;
            int total = message.Pdu.ErrorIndex.ToInt32();
            while (total-- > 0)
            {
                ISnmpObject next = store.GetNextObject(temp.Id);
                if (next == null)
                {
                    temp = new Variable(temp.Id, new EndOfMibView());
                    result.Add(temp);
                    break;
                }

                // TODO: how to handle write only object here?
                temp = new Variable(next.Id, next.Data);
                result.Add(temp);
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