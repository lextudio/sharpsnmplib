using System;
using System.Collections.Generic;
using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    partial class SmiParser
    {
        public IList<Exception> Exceptions = new List<Exception>();

        public MibDocument GetDocument()
        {
            return statement().result;
        }

        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
        {
            throw e;
        }

        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
        {
            throw new MismatchedTokenException(ttype, input);
        }
    }
}
