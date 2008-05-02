using System;
using System.Collections.Generic;
using System.Text;
namespace SharpSnmpLib
{
    sealed class PduCounter
    {
        PduCounter() { }
        public static Int GetNextCount()
        {
            count += 10;
            return new Int(count);
        }
        
        internal static void Clear()
        {
        	count = 0;
        }

        private static int count;
    }
}
