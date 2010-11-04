// GET BULK message handler class.
// Copyright (C) 2009-2010 Lex Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Handle(SnmpContext context, ObjectStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
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