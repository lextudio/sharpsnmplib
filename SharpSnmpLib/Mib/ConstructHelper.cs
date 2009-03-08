using System;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace Lextm.SharpSnmpLib.Mib
{
    internal static class ConstructHelper
    {
        internal static void ParseOidValue(Lexer lexer, out string parent, out uint value)
        {
            parent = null;
            value = 0;
            Symbol previous = null;
            
            Symbol temp = lexer.NextNonEOLSymbol;
            Expect(temp, Symbol.OpenBracket);
            StringBuilder longParent = new StringBuilder();
            temp = lexer.NextNonEOLSymbol;
            longParent.Append(temp);
            
            while ((temp = lexer.NextNonEOLSymbol) != null)
            {
                if (temp == Symbol.OpenParentheses)
                {
                    longParent.Append(temp);
                    temp = lexer.NextNonEOLSymbol;
                    bool succeed = uint.TryParse(temp.ToString(), out value);
                    Validate(temp, !succeed, "not a decimal");
                    longParent.Append(temp);
                    temp = lexer.NextNonEOLSymbol;
                    Expect(temp, Symbol.CloseParentheses);
                    longParent.Append(temp);
                    continue;
                }
                
                if (temp == Symbol.CloseBracket)
                {
                    parent = longParent.ToString();
                    return;
                }
                
                bool succeeded = uint.TryParse(temp.ToString(), out value);
                if (succeeded)
                {
                    // numerical way
                    while ((temp = lexer.NextNonEOLSymbol) != Symbol.CloseBracket)
                    {
                        longParent.Append(".");
                        longParent.Append(value);
                        succeeded = uint.TryParse(temp.ToString(), out value);
                        Validate(temp, !succeeded, "not a decimal");
                    }
                    
                    Expect(temp, Symbol.CloseBracket);
                    parent = longParent.ToString();
                    return;
                }
                
                longParent.Append(".");
                longParent.Append(temp);
                temp = lexer.NextNonEOLSymbol;
                Expect(temp, Symbol.OpenParentheses);
                longParent.Append(temp);
                temp = lexer.NextNonEOLSymbol;
                succeeded = uint.TryParse(temp.ToString(), out value);
                Validate(temp, !succeeded, "not a decimal");
                longParent.Append(temp);
                temp = lexer.NextNonEOLSymbol;
                Expect(temp, Symbol.CloseParentheses);
                longParent.Append(temp);
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
            if (UseStricterValidation && (name.Length < 1 || name.Length > 64))
            {
                message = "an identifier must consist of 1 to 64 letters, digits, and hyphens";
                return false;
            }

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

            if (UseStricterValidation && name.Contains("_"))
            {
                message = "underscores are not allowed in identifiers";
                return false;
            }

            // TODO: SMIv2 forbids "-" except in module names and keywords
            message = null;
            return true;
        }

        private static bool? useStricterValidation;

        private static bool UseStricterValidation
        {
            get
            {
                if (useStricterValidation == null)
                {
                    object setting = ConfigurationManager.AppSettings["StricterValidationEnabled"];
                    useStricterValidation = setting != null && Convert.ToBoolean(setting.ToString(), CultureInfo.InvariantCulture);
                }

                return useStricterValidation.Value;
            }
        }
    }
}
