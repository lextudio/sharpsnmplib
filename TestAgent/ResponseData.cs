using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class ResponseData
    {
        private IList<Variable> _variables;
        private ErrorCode _status;
        private int _index;

        public ResponseData(IList<Variable> variables, ErrorCode status, int index)
        {
            _variables = variables;
            _status = status;
            _index = index;
        }

        public IList<Variable> Variables
        {
            get { return _variables; }
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
