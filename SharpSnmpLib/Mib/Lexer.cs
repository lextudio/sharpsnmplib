/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 16:50
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Lexer class that parses MIB files into symbol list.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lexer")]
    public sealed class Lexer
    {
        private readonly IList<Symbol> _symbols = new List<Symbol>();

        internal void Parse(TextReader stream)
        {
            Parse(null, stream);
        }

        /// <summary>
        /// Parses MIB file to symbol list.
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="stream">File stream</param>
        public void Parse(string file, TextReader stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            
            _assignAhead = false;
            _assignSection = false;
            _stringSection = false;
            
            string line;
            int i = 0;
            while ((line = stream.ReadLine()) != null)
            {
                if (!_stringSection && line.TrimStart().StartsWith("--", StringComparison.Ordinal))
                {
                    i++;
                    continue; // commented line
                }

                ParseLine(file, line, i);
                i++;
            }
        }

        private void ParseLine(string file, string line, int row)
        {
            line = line + "\n";
            int count = line.Length;
            for (int i = 0; i < count; i++)
            {
                char current = line[i];
                bool moveNext = Parse(file, _symbols, current, row, i);
                if (moveNext)
                {
                    break;
                }
            }
        }

        private int _index;

        /// <summary>
        /// Next <see cref="Symbol"/> which is not <see cref="Symbol.EOL"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "EOL")]
        public Symbol NextNonEOLSymbol
        {
            get
            {
                Symbol result;
                while ((result = NextSymbol) == Symbol.EOL)
                {
                }

                return result;
            }
        }

        /// <summary>
        /// Next <see cref="Symbol"/>.
        /// </summary>
        public Symbol NextSymbol
        {
            get
            {
                return _index < _symbols.Count ? _symbols[_index++] : null;
            }
        }
        
        /// <summary>
        /// </summary>
        /// <param name="last"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Restore(Symbol last)
        {
            _index--;
            if (last != _symbols[_index])
            {
                throw new ArgumentException(@"wrong last symbol", "last");
            }
        }

        internal int SymbolCount
        {
            get
            {
                return _symbols.Count;
            }
        }

        private StringBuilder _temp = new StringBuilder();
        private bool _stringSection;
        private bool _assignSection;
        private bool _assignAhead;

        /// <summary>
        /// Parses a list of <see cref="char"/> to <see cref="Symbol"/>.
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="current">Current <see cref="char"/></param>
        /// <param name="row">Row number</param>
        /// <param name="column">Column number</param>
        /// <param name="list"></param>
        /// <returns><code>true</code> if no need to process this line. Otherwise, <code>false</code> is returned.</returns>
        private bool Parse(string file, ICollection<Symbol> list, char current, int row, int column)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            switch (current)
            {
                case '\n':
                case '{':
                case '}':
                case '(':
                case ')':
                case '[':
                case ']':
                case ';':
                case ',':
                    if (!_stringSection)
                    {
                        bool moveNext = ParseLastSymbol(file, list, ref _temp, row, column);
                        if (moveNext)
                        {
                            list.Add(CreateSpecialSymbol(file, '\n', row, column));
                            return true;
                        }

                        list.Add(CreateSpecialSymbol(file, current, row, column));
                        return false;
                    }

                    break;
                case '"':
                    _stringSection = !_stringSection;
                    break;
                case '\r':
                    return false;
                default:
                    if (Char.IsWhiteSpace(current) && !_assignSection && !_stringSection)
                    {                     
                        bool moveNext = ParseLastSymbol(file, list, ref _temp, row, column);
                        if (moveNext)
                        {
                            list.Add(CreateSpecialSymbol(file, '\n', row, column));
                            return true;
                        }

                        return false;
                    }
                    
                    if (_assignAhead)
                    {
                        _assignAhead = false;
                        ParseLastSymbol(file, list, ref _temp, row, column);
                        break;
                    }
                    
                    if (current == ':' && !_stringSection)
                    {    
                        if (!_assignSection)
                        {
                            ParseLastSymbol(file, list, ref _temp, row, column);
                        }
                        
                        _assignSection = true;
                    }
                    
                    if (current == '=' && !_stringSection)
                    {
                        _assignSection = false; 
                        _assignAhead = true;
                    }

                    break;
            }

            _temp.Append(current);
            return false;
        }

        private static bool ParseLastSymbol(string file, ICollection<Symbol> list, ref StringBuilder builder, int row, int column)
        {
            if (builder.Length > 0)
            {
                string content = builder.ToString();
                builder.Length = 0;
                Symbol s = new Symbol(file, content, row, column);
                if (s == Symbol.Comment)
                {
                    // ignore the rest symbols on this line because they are in comment.
                    return true;
                }

                list.Add(s);
            }

            return false;
        }

        private static Symbol CreateSpecialSymbol(string file, char value, int row, int column)
        {
            string str;
            switch (value)
            {
                case '\n':
                    str = Environment.NewLine;
                    break;
                case '{':
                    str = "{";
                    break;
                case '}':
                    str = "}";
                    break;
                case '(':
                    str = "(";
                    break;
                case ')':
                    str = ")";
                    break;
                case '[':
                    str = "[";
                    break;
                case ']':
                    str = "]";
                    break;
                case ';':
                    str = ";";
                    break;
                case ',':
                    str = ",";
                    break;
                default:
                    throw new ArgumentException("value is not a special character");
            }

            return new Symbol(file, str, row, column);
        }

        internal void ParseOidValue(out string parent, out uint value)
        {
            parent = null;
            value = 0;
            Symbol previous = null;
            
            Symbol temp = NextNonEOLSymbol;
            temp.Expect(Symbol.OpenBracket);
            StringBuilder longParent = new StringBuilder();
            temp = NextNonEOLSymbol;
            longParent.Append(temp);
            
            while ((temp = NextNonEOLSymbol) != null)
            {
                if (temp == Symbol.OpenParentheses)
                {
                    longParent.Append(temp);
                    temp = NextNonEOLSymbol;
                    bool succeed = UInt32.TryParse(temp.ToString(), out value);
                    temp.Validate(!succeed, "not a decimal");
                    longParent.Append(temp);
                    temp = NextNonEOLSymbol;
                    temp.Expect(Symbol.CloseParentheses);
                    longParent.Append(temp);
                    continue;
                }
                
                if (temp == Symbol.CloseBracket)
                {
                    parent = longParent.ToString();
                    return;
                }
                
                bool succeeded = UInt32.TryParse(temp.ToString(), out value);
                if (succeeded)
                {
                    // numerical way
                    while ((temp = NextNonEOLSymbol) != Symbol.CloseBracket)
                    {
                        longParent.Append(".").Append(value);
                        succeeded = UInt32.TryParse(temp.ToString(), out value);
                        temp.Validate(!succeeded, "not a decimal");
                    }
                    
                    temp.Expect(Symbol.CloseBracket);
                    parent = longParent.ToString();
                    return;
                }
                
                longParent.Append(".");
                longParent.Append(temp);
                temp = NextNonEOLSymbol;
                temp.Expect(Symbol.OpenParentheses);
                longParent.Append(temp);
                temp = NextNonEOLSymbol;
                succeeded = UInt32.TryParse(temp.ToString(), out value);
                temp.Validate(!succeeded, "not a decimal");
                longParent.Append(temp);
                temp = NextNonEOLSymbol;
                temp.Expect(Symbol.CloseParentheses);
                longParent.Append(temp);
                previous = temp;
            }
            
            throw MibException.Create("end of file reached", previous);
        }
    }
}