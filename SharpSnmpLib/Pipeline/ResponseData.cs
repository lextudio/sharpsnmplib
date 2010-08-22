using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Response data.
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseData"/> class.
        /// </summary>
        /// <param name="variables">The variables.</param>
        /// <param name="status">The status.</param>
        /// <param name="index">The index.</param>
        public ResponseData(IList<Variable> variables, ErrorCode status, int index)
        {
            Variables = variables ?? new List<Variable>(0);
            ErrorStatus = status;
            ErrorIndex = index;
            HasResponse = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseData"/> class.
        /// </summary>
        public ResponseData()
        {
            HasResponse = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has response.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has response; otherwise, <c>false</c>.
        /// </value>
        public bool HasResponse { get; set; }

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
