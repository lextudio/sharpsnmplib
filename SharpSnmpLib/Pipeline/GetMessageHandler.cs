// GET message handler class.
// Copyright (C) 2010 Lex Li
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
    /// GET message handler.
    /// </summary>
    /// <remarks>
    /// Follows RFC 3416 4.2.1.
    /// </remarks>
    public class GetMessageHandler : IMessageHandler
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
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            int index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in context.Request.Pdu.Variables)
            {
                index++;
                try
                {
                    ScalarObject obj = store.GetObject(v.Id);
                    if (obj == null)
                    {
                        result.Add(new Variable(v.Id, new NoSuchInstance()));
                    }
                    else
                    {
                        Variable item = obj.Variable;
                        result.Add(item);
                    }
                }
                catch (AccessFailureException)
                {
                    result.Add(new Variable(v.Id, new NoSuchObject()));
                }
                catch (Exception)
                {
                    context.CopyRequest(ErrorCode.GenError, index);
                    return;
                }
            }

            context.GenerateResponse(result);
        }
    }
}
