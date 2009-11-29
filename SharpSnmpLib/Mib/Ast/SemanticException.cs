using System;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    /// <summary>
    /// Semantic exception.
    /// </summary>
    public class SemanticException : SharpSnmpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SemanticException(string message) : base(message)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
		public SemanticException(string message, Exception inner)
			: base(message, inner)
		{
		}
    }
}
