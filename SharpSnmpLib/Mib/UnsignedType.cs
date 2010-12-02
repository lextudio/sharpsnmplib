using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /**
     * As this type is used for Counter32 and TimeTicks as well as Unsigned32
     * and Gauge32 it incorrectly allows range restrictions of Counter32 and
     * TimeTicks.  This is ok as currently we do not care about detecting
     * incorrect MIBs and this doesn't block the decoding of correct MIBs.
     */
    class UnsignedType : ITypeAssignment
    {
        private string _module;
        private string _name;
        private List<ValueRange> _ranges;

        public UnsignedType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;

            Symbol temp = lexer.NextNonEOLSymbol;
            if (temp == Symbol.OpenParentheses)
            {
                lexer.Restore(temp);
                DecodeRanges(lexer);
            }
            else
            {
                lexer.Restore(temp);
            }
        }

        private void DecodeRanges(Lexer lexer)
        {
            Symbol temp = lexer.NextSymbol;
            _ranges = new List<ValueRange>();

            while (temp != Symbol.CloseParentheses)
            {
                Symbol value1 = lexer.NextSymbol;
                Symbol value2 = null;

                temp = lexer.NextSymbol;
                if (temp == Symbol.DoubleDot)
                {
                    value2 = lexer.NextSymbol;
                    temp = lexer.NextSymbol;
                }

                ValueRange range = new ValueRange(value1, value2);

                value1.Validate(Contains(range.Start), "invalid sub-typing");
                if (value2 != null)
                {
                    value2.Validate(Contains((int)range.End), "invalid sub-typing");
                }

                foreach (ValueRange other in _ranges)
                {
                    value1.Validate(range.Contains(other.Start), "invalid sub-typing");
                    if (other.End != null)
                    {
                        value1.Validate(range.Contains((int)other.End), "invalid sub-typing");
                    }
                }

                _ranges.Add(range);
            }
        }

        internal bool Contains(int value)
        {
            foreach (ValueRange range in _ranges)
            {
                if (range.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
