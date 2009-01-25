using System.Threading;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// A counter that generates PDU sequence number.
    /// </summary>
    /// <remarks>The sequence number is used to identifier PDU sessions.</remarks>
    internal static class PduCounter
    {
        private static readonly object syncRoot = new object();
        
        internal static Integer32 NextCount
        {
            get
            {
                Integer32 result;
                lock (syncRoot)
                {
                    unchecked
                    {
                        count += 10;
                    }
                    
                    result = new Integer32(count);
                }
                
                return result;
            }
        }
        
        internal static void Clear()
        {
            Interlocked.Exchange(ref count, 0);
        }

        private static int count;
    }
}
