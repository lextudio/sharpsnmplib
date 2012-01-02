using System;
using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Semantic exception.
    /// </summary>
    public class SemanticException : RecognitionException
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
