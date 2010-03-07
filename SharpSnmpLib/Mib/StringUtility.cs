/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/10/1
 * Time: 20:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;

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

        /// <summary>
        /// Gets the alternative textual form.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns></returns>
        [Obsolete("Use SearchResult.AlternativeText instead.")]
        public static string GetAlternativeTextualForm(IDefinition definition)
        {
            // TODO: delete this method.
            return string.Empty;
        }
    }
}
