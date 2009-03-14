using System.Threading;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// A counter that generates PDU sequence number.
    /// </summary>
    /// <remarks>The sequence number is used to identifier PDU sessions.</remarks>
    public static class PduCounter
    {      
        /// <summary>
        /// Returns next number for request ID.
        /// </summary>
        public static int NextCount
        {
            get
            {
                return Interlocked.Increment(ref count);
            }
        }
        
        internal static void Clear()
        {
            Interlocked.Exchange(ref count, 0);
        }

        private static int count;
    }
}
