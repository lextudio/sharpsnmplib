using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    internal static class Extensions
    {
        public static Symbol NextSymbol(this IEnumerator<Symbol> enumerator)
        {
            return enumerator.MoveNext() ? enumerator.Current : null;
        }

        public static Symbol NextNonEOLSymbol(this IEnumerator<Symbol> enumerator)
        {
            var continuing = enumerator.MoveNext();
            while (continuing && enumerator.Current == Symbol.EOL)
            {
                continuing = enumerator.MoveNext();
            }

            return continuing ? enumerator.Current : null;
        }
    }
}
