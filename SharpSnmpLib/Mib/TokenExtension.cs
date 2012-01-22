using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib
{
    internal static class TokenExtension
    {
        internal static bool IsValidIdentifier(this IToken token, SmiParser parser)
        {
            string message;
            string name = token.Text;
            if (UseStricterValidation && (name.Length < 1 || name.Length > 64))
            {
                message = string.IsNullOrEmpty(parser.FileName)
                              ? string.Format(
                                  "warning N0006 : an identifier must consist of 1 to 64 letters, digits, and hyphens. {0}",
                                  token.Text)
                              : string.Format(
                                  "{0} ({1},{2}) : warning N0006 : an identifier must consist of 1 to 64 letters, digits, and hyphens. {3}",
                                  parser.FileName,
                                  token.Line, token.CharPositionInLine + 1, token.Text);
                parser.Warnings.Add(new CompilerWarning(token, parser.FileName, message));
                return false;
            }

            if (!Char.IsLetter(name[0]))
            {
                message = string.IsNullOrEmpty(parser.FileName)
                              ? string.Format(
                                  "warning N0007 : the initial character must be a letter. {0}", token.Text)
                              : string.Format(
                                  "{0} ({1},{2}) : warning N0007 : the initial character must be a letter. {3}",
                                  parser.FileName,
                                  token.Line, token.CharPositionInLine + 1, token.Text);
                parser.Warnings.Add(new CompilerWarning(token, parser.FileName, message));
                return false;
            }

            if (name.EndsWith("-", StringComparison.Ordinal))
            {
                message = string.IsNullOrEmpty(parser.FileName)
                              ? string.Format(
                                  "warning N0008 : a hyphen cannot be the last character of an identifier. {0}",
                                  token.Text)
                              : string.Format(
                                  "{0} ({1},{2}) : warning N0008 : a hyphen cannot be the last character of an identifier. {3}",
                                  parser.FileName,
                                  token.Line, token.CharPositionInLine + 1, token.Text);
                parser.Warnings.Add(new CompilerWarning(token, parser.FileName, message));
                return false;
            }

            if (name.Contains("--"))
            {
                message = string.IsNullOrEmpty(parser.FileName)
                              ? string.Format(
                                  "warning N0009 : a hyphen cannot be immediately followed by another hyphen in an identifier. {0}",
                                  token.Text)
                              : string.Format(
                                  "{0} ({1},{2}) : warning N0009 : a hyphen cannot be immediately followed by another hyphen in an identifier. {3}",
                                  parser.FileName,
                                  token.Line, token.CharPositionInLine + 1, token.Text);
                parser.Warnings.Add(new CompilerWarning(token, parser.FileName, message));
                return false;
            }

            if (UseStricterValidation && name.Contains("_"))
            {
                message = string.IsNullOrEmpty(parser.FileName)
                              ? string.Format(
                                  "warning N0010 : underscores are not allowed in identifiers. {0}", token.Text)
                              : string.Format(
                                  "{0} ({1},{2}) : warning N0010 : underscores are not allowed in identifiers. {3}",
                                  parser.FileName,
                                  token.Line, token.CharPositionInLine + 1, token.Text);
                parser.Warnings.Add(new CompilerWarning(token, parser.FileName, message));
                return false;
            }

            // TODO: SMIv2 forbids "-" except in module names and keywords
            return true;
        }

        private static bool? _useStricterValidation;

        private static bool UseStricterValidation
        {
            get
            {
                if (_useStricterValidation == null)
                {
                    object setting = ConfigurationManager.AppSettings["StricterValidationEnabled"];
                    _useStricterValidation = setting != null && Convert.ToBoolean(setting.ToString(), CultureInfo.InvariantCulture);
                }

                return _useStricterValidation.Value;
            }
        }

        internal static bool IsUppercase(this IToken token)
        {
            var name = token.Text;
            if (String.IsNullOrEmpty(name))
            {
                return true;
            }

            return name.All(c => !Char.IsLower(c));
        }

        // check if name is PascalCased

        internal static bool IsPascalCase(this IToken token)
        {
            var name = token.Text;
            return String.IsNullOrEmpty(name) || Char.IsUpper(name[0]);
        }

        // convert name to PascalCase

        internal static string PascalCase(this string name)
        {
            if (String.IsNullOrEmpty(name))
                return String.Empty;

            if (name.Length == 1)
                return name.ToUpperInvariant();

            int index = IndexOfFirstCorrectChar(name);
            return Char.ToUpperInvariant(name[index]).ToString(CultureInfo.InvariantCulture) + name.Substring(index + 1);
        }

        // check if name is camelCased

        internal static bool IsCamelCase(this IToken token)
        {
            var name = token.Text;
            if (String.IsNullOrEmpty(name))
                return true;

            return Char.IsLower(name[0]);
        }

        // convert name to camelCase

        internal static string CamelCase(this string name)
        {
            if (String.IsNullOrEmpty(name))
                return String.Empty;

            if (name.Length == 1)
                return name.ToLowerInvariant();

            int index = IndexOfFirstCorrectChar(name);
            return Char.ToLowerInvariant(name[index]).ToString(CultureInfo.InvariantCulture) + name.Substring(index + 1);
        }

        private static int IndexOfFirstCorrectChar(string s)
        {
            int index = 0;
            while ((index < s.Length) && (s[index] == '_'))
                index++;
            // it's possible that we won't find one, e.g. something called "_"
            return (index == s.Length) ? 0 : index;
        }
    }
}