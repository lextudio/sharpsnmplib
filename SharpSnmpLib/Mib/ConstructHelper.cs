using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal static class ConstructHelper
    {
        internal static void ParseOidValue(Lexer lexer, out string parent, out uint value)
        {
            parent = null;
            Symbol temp = lexer.NextNonEOLSymbol;
            Expect(temp, Symbol.OpenBracket);
            string lastParent = parent;
            parent = lexer.NextSymbol.ToString();
            Symbol previous = null;
            value = 0;
            while ((temp = lexer.NextSymbol) != null) 
            {
                if (temp == Symbol.CloseBracket)
                {
                    parent = lastParent;
                    return;
                }

                bool succeeded = uint.TryParse(temp.ToString(), out value);
                if (succeeded) 
                {
                    while ((temp = lexer.NextNonEOLSymbol) != Symbol.CloseBracket)
                    {
                        parent = parent + "." + value.ToString(CultureInfo.InvariantCulture);
                        succeeded = uint.TryParse(temp.ToString(), out value);
                        Validate(temp, !succeeded, "not a decimal");  
                    }
                    
                    Expect(temp, Symbol.CloseBracket);
                    return;
                }

                lastParent = parent;
                parent = temp.ToString();
                temp = lexer.NextSymbol;
                Expect(temp, Symbol.OpenParentheses);
                temp = lexer.NextSymbol;
                succeeded = uint.TryParse(temp.ToString(), out value);
                Validate(temp, !succeeded, "not a decimal");
                temp = lexer.NextSymbol;
                Expect(temp, Symbol.CloseParentheses);
                previous = temp;
            }
            
            throw SharpMibException.Create("end of file reached", previous);
        }
        
        internal static void Expect(Symbol current, Symbol expected)
        {
            Validate(current, current != expected, expected + " expected");
        }
        
        internal static void Validate(Symbol current, bool condition, string message)
        {
            if (condition)
            {
                throw SharpMibException.Create(message, current);
            }
        }
        
        internal static void ValidateIdentifier(Symbol current)
        {
            string message;
            bool condition = !IsValidIdentifier(current.ToString(), out message);
            Validate(current, condition, message);
        }
        
        internal static bool IsValidIdentifier(string name, out string message)
        {
// TODO: enable this later.
//            if (name.Length < 1 || name.Length > 64) 
//            {
//                message = "an identifier must consist of 1 to 64 letters, digits, and hyphens";
//                return false;
//            }
            
            if (!char.IsLetter(name[0]))
            {
                message = "the initial character must be a letter";
                return false;
            }
            
            if (name.EndsWith("-", StringComparison.Ordinal))
            {
                message = "a hyphen cannot be the last character of an identifier";
                return false;
            }
            
            if (name.Contains("--")) 
            {
                message = "a hyphen cannot be immediately followed by another hyphen in an identifier";
                return false;
            }
// TODO: enable this later.            
//            if (name.Contains("_"))
//            {
//                message = "underscores are not allowed in identifiers";
//                return false;
//            }
            
            message = null;
            
            // TODO: SMIv2 forbids "-" except in module names and keywords
            return true;
        }
    }
}
