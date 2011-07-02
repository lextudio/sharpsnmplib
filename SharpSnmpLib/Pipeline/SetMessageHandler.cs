// SET message handler class.
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
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SET message handler.
    /// </summary>    
    /// <remarks>
    /// Follows RFC 3416 4.2.1.
    /// Not supported scenarios are, 
    ///   wrongLength, 
    ///   wrongEncoding, 
    ///   wrongValue, 
    ///   noCreation,
    ///   inconsistentName,
    ///   inconsistentValue,
    ///   resourceUnavailable,
    ///   commitFailed,
    ///   undoFailed
    /// </remarks>
    public class SetMessageHandler : IMessageHandler
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
            
            context.CopyRequest(ErrorCode.InconsistentName, int.MaxValue);
            if (context.TooBig)
            {
                context.GenerateTooBig();
                return;
            }

            var index = 0;
            var status = ErrorCode.NoError;

            IList<Variable> result = new List<Variable>();
            foreach (var v in context.Request.Pdu().Variables)
            {
                index++;
                var obj = store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        obj.Data = v.Data;
                    }
                    catch (AccessFailureException)
                    {
                        status = ErrorCode.NoAccess;
                    }
                    catch (ArgumentException)
                    {
                        status = ErrorCode.WrongType;
                    }
                    catch (Exception)
                    {
                        status = ErrorCode.GenError;
                    }
                }
                else
                {
                    status = ErrorCode.NotWritable;
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
