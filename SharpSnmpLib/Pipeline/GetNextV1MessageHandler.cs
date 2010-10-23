using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// GET NEXT message handler.
    /// </summary>    
    /// <remarks>
    /// Follows RFC 1157, 4.1.3
    /// </remarks>
// ReSharper disable UnusedMember.Global
    public class GetNextV1MessageHandler : IMessageHandler
// ReSharper restore UnusedMember.Global
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public void Handle(SnmpContext context, ObjectStore store)
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
                    if (next == null)
                    {
                        status = ErrorCode.NoSuchName;
                    }
                    else
                    {
                        // TODO: how to handle write only object here?
                        result.Add(next.Variable);
                    }
                }
                catch (Exception)
                {
                    context.CopyRequest(ErrorCode.GenError, index);
                    return;
                }
                
                if (status != ErrorCode.NoError)
                {
                    context.CopyRequest(status, index);
                    return;
                }
            }

            context.GenerateResponse(result);
        }
    }
}