using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class ResponseData
    {
        public ResponseData(IList<Variable> variables, ErrorCode status, int index)
        {
            Variables = variables;
            ErrorStatus = status;
            ErrorIndex = index;
        }

        public IList<Variable> Variables { get; private set; }

        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
        public ErrorCode ErrorStatus { get; private set; }

        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
        public int ErrorIndex { get; private set; }
    }
}
