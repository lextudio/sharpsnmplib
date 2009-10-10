using System;
using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib.Compiler
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mib")]
    public class MibCompilerException : Exception
    {
        /// <summary>
        /// Creates a <see cref="MibCompilerException"/> instance.
        /// </summary>
        public MibCompilerException() 
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="MibCompilerException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        public MibCompilerException(string message) : base(message) 
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="MibCompilerException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner exception.</param>
        public MibCompilerException(string message, Exception inner) 
            : base(message, inner) 
        {
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Creates a <see cref="MibCompilerException"/> instance.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        protected MibCompilerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        } 
#endif
    }
}