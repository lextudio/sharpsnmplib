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
    /// Description of MibException.
    /// </summary>
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mib")]
    public class MibException : SnmpException
    {
        /// <summary>
        /// Creates a <see cref="MibException"/>.
        /// </summary>
        public MibException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SnmpException"/> instance with a specific <see cref="string"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public MibException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="MibException"/> instance with a specific <see cref="string"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public MibException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="MibException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "MibException: " + Details;
        }

        protected override string Details
        {
            get {
                var ex = InnerException as RecognitionException;
                if (ex == null)
                {
                    return Message;
                }

                return ex.Message;
            }
        }
    }
}