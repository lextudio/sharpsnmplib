using System.Threading;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// A counter that generates PDU sequence number.
    /// </summary>
    /// <remarks>The sequence number is used to identifier PDU sessions.</remarks>
    internal static class PduCounter
    {      
        internal static Integer32 NextCount
        {
            get
            {
                int latest = Interlocked.Increment(ref count);                    
                return new Integer32(latest);               
            }
        }
        
        internal static void Clear()
        {
            Interlocked.Exchange(ref count, 0);
        }

        private static int count;
    }
}
