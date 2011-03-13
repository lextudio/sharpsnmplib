using System;
using System.Collections.Generic;
using System.Linq;

namespace Lextm.SharpSnmpLib.Mib
{
    abstract class AbstractTypeAssignment : ITypeAssignment
    {
        protected Symbol Next(object o)
        {
            var lexer = o as Lexer;
            if (lexer != null)
            {
                return lexer.NextNonEOLSymbol;
            }

            var enumerator = o as IEnumerator<Symbol>;
            return enumerator != null ? enumerator.NextNonEOLSymbol() : null;
        }

        protected IList<ValueRange> DecodeRanges(object enumerator)
        {
            Symbol temp = null;
            var ranges = new List<ValueRange>();

            var size = false;

            while (temp != Symbol.CloseParentheses)
            {
                var value1 = Next(enumerator);
                Symbol value2 = null;

                if (value1 == Symbol.Size)
                {
                    size = true;
                    Next(enumerator).Expect(Symbol.OpenParentheses);
                    continue;
                }

                temp = Next(enumerator);
                if (temp == Symbol.DoubleDot)
                {
                    value2 = Next(enumerator);
                    temp = Next(enumerator);
                }

                var range = new ValueRange(value1, value2);

                if (size)
                {
                    value1.Validate(range.Start < 0, "invalid sub-typing; size must be greater than 0");
                }

                value1.Validate(Contains(range.Start, ranges), "invalid sub-typing");
                if (value2 != null)
                {
                    value2.Validate(Contains((int)range.End, ranges), "invalid sub-typing");
                }

                foreach (var other in ranges)
                {
                    value1.Validate(range.Contains(other.Start), "invalid sub-typing");
                    if (other.End != null)
                    {
                        value1.Validate(range.Contains((int)other.End), "invalid sub-typing");
                    }
                }

                ranges.Add(range);
            }

            if (size)
            {
                Next(enumerator).Expect(Symbol.CloseParentheses);
            }
            return ranges;
        }

        protected IDictionary<int, string> DecodeEnumerations(object enumerator)
        {
            var map = new Dictionary<int, string>();

            do
            {
                var identifier = Next(enumerator).ToString();

                Next(enumerator).Expect(Symbol.OpenParentheses);

                var value = Next(enumerator);

                int signedNumber;
                if (int.TryParse(value.ToString(), out signedNumber))
                {
                    try
                    {
                        // Have to include the number as it seems repeated identifiers are allowed ??
                        map.Add(signedNumber, String.Format("{0}({1})", identifier, signedNumber));
                    }
                    catch (ArgumentException ex)
                    {
                        value.Validate(true, ex.Message);
                    }
                }
                else
                {
                    // Need to get "DefinedValue".
                }

                Next(enumerator).Expect(Symbol.CloseParentheses);
            } while (Next(enumerator) != Symbol.CloseBracket);

            return map;
        }

        private static bool Contains(Int64 value, IEnumerable<ValueRange> ranges)
        {
            return ranges.Any(range => range.Contains(value));
        }

        public abstract string Name { get; }
    }
}
