using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// GET message handler.
    /// </summary>
    internal class GetMessageHandler : IMessageHandler
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
            _status = ErrorCode.NoError;
            _index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Pdu.Variables)
            {
                _index++;
                ISnmpObject obj = store.GetObject(v.Id);
                if (obj != null)
                {
                    try
                    {
                        Variable item = new Variable(v.Id, obj.Data);
                        result.Add(item);
                    }
                    catch (AccessFailureException)
                    {
                        _status = ErrorCode.NoSuchName;
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