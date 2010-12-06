using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal static class Extensions
    {
        public static Symbol NextSymbol(this IEnumerator<Symbol> enumerator)
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            else
            {
                return null;
            }
        }

        public static Symbol NextNonEOLSymbol(this IEnumerator<Symbol> enumerator)
        {
            bool continuing = enumerator.MoveNext();
            while (continuing && enumerator.Current == Symbol.EOL)
            {
                continuing = enumerator.MoveNext();
            }
            if (continuing)
            {
                return enumerator.Current;
            }
            else
            {
                return null;
            }
        }
    }
}
