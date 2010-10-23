/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SET message handler.
    /// </summary>    
    /// <remarks>
    /// Follows RFC 1157, 4.1.5
    /// </remarks>
// ReSharper disable UnusedMember.Global
    public class SetV1MessageHandler : IMessageHandler
// ReSharper restore UnusedMember.Global
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Handle(SnmpContext context, ObjectStore store)
        {
            int index = 0;
            ErrorCode status = ErrorCode.NoError;

            IList<Variable> result = new List<Variable>();
            foreach (Variable v in context.Request.Pdu.Variables)
            {
                index++;
                ScalarObject obj = store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        obj.Data = v.Data;
                    }
                    catch (AccessFailureException)
                    {
                        status = ErrorCode.NoSuchName;
                    }
                    catch (ArgumentException)
                    {
                        status = ErrorCode.BadValue;
                    }
                    catch (Exception)
                    {
                        status = ErrorCode.GenError;
                    }
                }
                else
                {
                    status = ErrorCode.NoSuchName;
                }

                if (status != ErrorCode.NoError)
                {
                    context.CopyRequest(status, index);
                    return;
                }

                result.Add(v);
            }

            context.GenerateResponse(result);
        }
    }
}
