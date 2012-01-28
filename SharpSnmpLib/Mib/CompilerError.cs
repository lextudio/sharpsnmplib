/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 16:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of CompilerError.
    /// </summary>
    [Serializable]
    public class CompilerError
    {
        private readonly RecognitionException _error;

        /// <summary>
        /// Creates a <see cref="CompilerError"/> instance with a specific <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">Compiler exception.</param>
        public CompilerError(RecognitionException exception)
        {
            _error = exception;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="CompilerError"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "CompilerError: " + Details;
        }

        public string Details
        {
            get
            {
                var token = _error.Token;
                return string.IsNullOrEmpty(FileName)
                           ? string.Format(
                               "error S0001 : invalid token {0}", token.Text)
                           : string.Format(
                               "{0} ({1},{2}) : error S0001 : invalid token {3}",
                               FileName,
                               token.Line, token.CharPositionInLine + 1, token.Text);
            }
        }

        public string FileName { private get; set; }

        public IToken Token
        {
            get { return _error.Token; }
        }
    }
}