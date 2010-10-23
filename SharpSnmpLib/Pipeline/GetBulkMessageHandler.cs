using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// GET BULK message handler.
    /// </summary>    
    public class GetBulkMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public void Handle(SnmpContext context, ObjectStore store)
        {
            IList<Variable> result = new List<Variable>();
            int index = 0;
            int nonrepeaters = context.Request.Pdu.ErrorStatus.ToInt32();
            for (int i = 0; i < nonrepeaters; i++)
            {
                Variable v = context.Request.Pdu.Variables[i];
                index++;
                try
                {
                    ScalarObject next = store.GetNextObject(v.Id);
                    result.Add(next == null ? new Variable(v.Id, new EndOfMibView()) : next.Variable);
                }
                catch (Exception)
                {
                    context.CopyRequest(ErrorCode.GenError, index);
                    return;
                }
            }

            for (int j = nonrepeaters; j < context.Request.Pdu.Variables.Count; j++)
            {
                Variable v = context.Request.Pdu.Variables[j];
                index++;
                Variable temp = v;
                int repetition = context.Request.Pdu.ErrorIndex.ToInt32();
                while (repetition-- > 0)
                {
                    try
                    {
                        ScalarObject next = store.GetNextObject(temp.Id);
                        if (next == null)
                        {
                            temp = new Variable(temp.Id, new EndOfMibView());
                            result.Add(temp);
                            break;
                        }

                        // TODO: how to handle write only object here?
                        result.Add(next.Variable);
                        temp = next.Variable;
                    }
                    catch (Exception)
                    {
                        context.CopyRequest(ErrorCode.GenError, index);
                        return;
                    }
                }
            }

            context.GenerateResponse(result);
        }
    }
}