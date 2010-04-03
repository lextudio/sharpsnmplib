using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Response data.
    /// </summary>
    internal class ResponseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseData"/> class.
        /// </summary>
        /// <param name="variables">The variables.</param>
        /// <param name="status">The status.</param>
        /// <param name="index">The index.</param>
        public ResponseData(IList<Variable> variables, ErrorCode status, int index)
        {
            Variables = variables;
            ErrorStatus = status;
            ErrorIndex = index;
        }

        /// <summary>
        /// Gets or sets the variables.
        /// </summary>
        /// <value>The variables.</value>
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
