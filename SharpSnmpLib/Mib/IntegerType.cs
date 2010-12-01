/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/25
 * Time: 20:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// The INTEGER type represents a list of alternatives, or a range of numbers..
    /// Includes Integer32 as it's indistinguishable from INTEGER.
    /// </summary>
    internal sealed class IntegerType : ITypeAssignment
    {
        private bool _isEnumeration;
        private IDictionary<int, string> _mapIntToString;
        private IDictionary<string, int> _mapStringToInt;
        private IList<ValueRange> _ranges;

        /// <summary>
        /// Creates an <see cref="IntegerType"/> instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="name"></param>
        /// <param name="lexer"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "module")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "name")]
        public IntegerType(string module, string name, Lexer lexer)
        {
            Symbol temp = lexer.NextNonEOLSymbol;
            if (temp == Symbol.OpenBracket)
            {
                lexer.Restore(temp);
                DecodeEnumerations(lexer);
            }
            else if (temp == Symbol.OpenParentheses)
            {
                lexer.Restore(temp);
                DecodeRanges(lexer);
            }
            else
            {
                lexer.Restore(temp);
            }
        }

        public bool IsEnumeration
        {
            get
            {
                return _isEnumeration;
            }
        }

        public int? this[string identifier]
        {
            get
            {
                return _isEnumeration ? (int?)_mapStringToInt[identifier] : null;
            }
        }

        public string this[int value]
        {
            get
            {
                return _isEnumeration ? _mapIntToString[value] : null;
            }
        }

        internal bool Contains(int p)
        {
            foreach (ValueRange range in _ranges)
            {
                if (range.Contains(p))
                {
                    return true;
                }
            }

            return false;
        }

        private void DecodeRanges(Lexer lexer)
        {
            Symbol temp = lexer.NextSymbol;
            _isEnumeration = false;
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

                _ranges.Add(new ValueRange(value1, value2));
            }
        }

        private void DecodeEnumerations(Lexer lexer)
        {
            _isEnumeration = true;

            _mapIntToString = new Dictionary<int, string>();
            _mapStringToInt = new Dictionary<string, int>();

            int signedNumber;
            while (lexer.NextNonEOLSymbol != Symbol.CloseBracket)
            {
                string identifier = lexer.NextNonEOLSymbol.ToString();

                lexer.NextNonEOLSymbol.Expect(Symbol.OpenParentheses);

                Symbol value = lexer.NextNonEOLSymbol;

                if (int.TryParse(value.ToString(), out signedNumber))
                {
                    _mapIntToString.Add(signedNumber, identifier);
                    _mapStringToInt.Add(identifier, signedNumber);
                }
                else
                {
                    // Need to get "DefinedValue".
                }

                lexer.NextNonEOLSymbol.Expect(Symbol.CloseParentheses);
            }
        }
    }
}