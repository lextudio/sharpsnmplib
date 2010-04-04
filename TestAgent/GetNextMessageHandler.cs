using System.Collections.Generic;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// GET NEXT message handler.
    /// </summary>
    internal class GetNextMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public ResponseData Handle(ISnmpMessage message, ObjectStore store)
        {
            ErrorCode status = ErrorCode.NoError;
            int index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Pdu.Variables)
            {
                index++;
                ScalarObject next = store.GetNextObject(v.Id);
                if (next == null)
                {
                    status = ErrorCode.NoSuchName;
                }
                else
                {
                    // TODO: how to handle write only object here?
                    result.Add(next.Variable);
                }

                if (status != ErrorCode.NoError)
                {
                    return null;
                }
            }

            return new ResponseData(result, status, index);
        }
    }
}