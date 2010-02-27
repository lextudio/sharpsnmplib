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

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SET message handler.
    /// </summary>
    internal class SetMessageHandler : IMessageHandler
    {
        private ErrorCode _status;
        private int _index;

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IList<Variable> Handle(ISnmpMessage message, ObjectStore store)
        {
            _index = 0;
            _status = ErrorCode.NoError;

            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Pdu.Variables) 
            {
                _index++;
                ScalarObject obj = store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        obj.Data = v.Data;
                    }
                    catch (AccessFailureException)
                    {
                        _status = ErrorCode.NoSuchName;
                    }
                    catch (ArgumentException)
                    {
                        _status = ErrorCode.BadValue;
                    }
                    catch (Exception)
                    {
                        _status = ErrorCode.GenError;
                    }
                }
                else
                {
                    _status = ErrorCode.NoSuchName;
                }
                
                if (_status != ErrorCode.NoError)
                {
                    return null;
                }

                result.Add(v);
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
