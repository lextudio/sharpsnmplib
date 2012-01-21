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
        /// <param name="token">The wrong token.</param>
        public SemanticException(IToken token)
        {
            Token = token;
        }
    }
}
