/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/10/1
 * Time: 20:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// String utility.
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Extracts the name.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ExtractName(string input)
        {
            int left = input.IndexOf('(');
            return left == -1 ? input : input.Substring(0, left);
        }

        /// <summary>
        /// Extracts the value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ExtractValue(string input)
        {
            int left = input.IndexOf('(');
            int right = input.IndexOf(')');
            if (left >= right)
            {
                throw new FormatException("input does not contain a value");
            }
            
            return uint.Parse(input.Substring(left + 1, right - left - 1), CultureInfo.InvariantCulture);
        }
        
        internal static string GetAlternativeTextualForm(IDefinition definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }
            
            List<string> names = new List<string>();
            IDefinition current = definition;
            while (current.ParentDefinition != null)
            {
                names.Add(current.Name);
                current = current.ParentDefinition;
            }
            
            if (names.Count == 0)
            {
                return string.Empty;
            }
            
            names.Reverse();
            StringBuilder result = new StringBuilder(names[0]);
            for (int i = 1; i < names.Count; i++)
            {
                result.Append(".").Append(names[i]);
            }
            
            return result.ToString();
        }
    }
}
