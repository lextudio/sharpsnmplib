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
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Lexer class that parses MIB files into symbol list.
    /// </summary>
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
            assignAhead = false;
            assignSection = false;
            stringSection = false;
            
            string line;
            int i = 0;
            while ((line = stream.ReadLine()) != null)
            {
                if (!stringSection && line.TrimStart().StartsWith("--", StringComparison.Ordinal))
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

        private int index;

        /// <summary>
        /// Next <see cref="Symbol"/> which is not <see cref="Symbol.EOL"/>.
        /// </summary>
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
                if (index < _symbols.Count)
                {
                    return _symbols[index++];
                }

                return null;
            }
        }
        
        ///<summary>
        ///</summary>
        ///<param name="last"></param>
        ///<exception cref="ArgumentException"></exception>
        public void Restore(Symbol last)
        {
            index--;
            if (last != _symbols[index])
            {
                throw new ArgumentException("wrong last symbol", "last");
            }
        }

        internal int SymbolCount
        {
            get
            {
                return _symbols.Count;
            }
        }

        private StringBuilder temp = new StringBuilder();
        private bool stringSection;
        private bool assignSection;
        private bool assignAhead;

        /// <summary>
        /// Parses a list of <see cref="char"/> to <see cref="Symbol"/>.
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="current">Current <see cref="char"/></param>
        /// <param name="row">Row number</param>
        /// <param name="column">Column number</param>
        /// <param name="list"></param>
        /// <returns><code>true</code> if no need to process this line. Otherwise, <code>false</code> is returned.</returns>
        public bool Parse(string file, IList<Symbol> list, char current, int row, int column)
        {
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
                    if (!stringSection)
                    {
                        bool moveNext = ParseLastSymbol(file, list, ref temp, row, column);
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
                    stringSection = !stringSection;
                    break;
                case '\r':
                    return false;
                default:
                    if (char.IsWhiteSpace(current) && !assignSection && !stringSection)
                    {                     
                        bool moveNext = ParseLastSymbol(file, list, ref temp, row, column);
                        if (moveNext)
                        {
                            list.Add(CreateSpecialSymbol(file, '\n', row, column));
                            return true;
                        }

                        return false;
                    }
                    
                    if (assignAhead)
                    {
                        assignAhead = false;
                        ParseLastSymbol(file, list, ref temp, row, column);
                        break;
                    }
                    
                    if (current == ':' && !stringSection)
                    {    
                        if (!assignSection)
                        {
                            ParseLastSymbol(file, list, ref temp, row, column);
                        }
                        
                        assignSection = true;
                    }
                    
                    if (current == '=' && !stringSection)
                    {
                        assignSection = false; 
                        assignAhead = true;
                    }

                    break;
            }

            temp.Append(current);
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
    }
}