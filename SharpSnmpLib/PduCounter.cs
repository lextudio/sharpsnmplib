using System.Threading;

// TODO: move this class to Controls after removing obsolete items.
namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// A counter that generates request ID.
    /// </summary>
    /// <remarks>The request ID is used to identifier sessions.</remarks>
    public static class RequestCounter
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
