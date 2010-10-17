using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// GET NEXT message handler.
    /// </summary>    
    /// <remarks>
    /// Follows RFC 3416 4.2.2.
    /// </remarks>
    public class GetNextMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public ResponseData Handle(SnmpContext context, ObjectStore store)
        {
            ErrorCode status = ErrorCode.NoError;
            int index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in context.Request.Pdu.Variables)
            {
                index++;
                try
                {
                    ScalarObject next = store.GetNextObject(v.Id);
                    result.Add(next == null ? new Variable(v.Id, new EndOfMibView()) : next.Variable);
                }
                catch (Exception)
                {
                    status = ErrorCode.GenError;
                }

                if (status != ErrorCode.NoError)
                {
                    return new ResponseData(null, status, index);
                }
            }

            return new ResponseData(result, status, index);
        }
    }
}