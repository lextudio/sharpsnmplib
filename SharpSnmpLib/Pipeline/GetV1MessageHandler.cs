// GET v1 message handler class.
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
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// GET message handler.
    /// </summary>
    /// <remarks>
    /// Follows RFC 1157, 4.1.2
    /// </remarks>
    public sealed class GetV1MessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Handle(ISnmpContext context, ObjectStore store)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }  
            
            var status = ErrorCode.NoError;
            var index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (var v in context.Request.Pdu().Variables)
            {
                index++;
                var obj = store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        var item = obj.Variable;
                        result.Add(item);
                    }
                    catch (AccessFailureException)
                    {
                        status = ErrorCode.NoSuchName;
                    }
                    catch (Exception)
                    {
                        context.CopyRequest(ErrorCode.GenError, index);
                        return;
                    }
                }
                else
                {
                    status = ErrorCode.NoSuchName;
                }

                if (status == ErrorCode.NoError)
                {
                    continue;
                }

                context.CopyRequest(status, index);
                return;
            }

            context.GenerateResponse(result);
        }
    }
}