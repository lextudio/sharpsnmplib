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
    /// Description of StringUtility.
    /// </summary>
    internal static class StringUtility
    {
        internal static string ExtractName(string input)
        {
            int left = input.IndexOf('(');
            return left == -1 ? input : input.Substring(0, left);
        }
        
        internal static uint ExtractValue(string input)
        {
            int left = input.IndexOf('(');
            int right = input.IndexOf(')');
            if (left >= right)
            {
                throw new FormatException("input does not contain a value");
            }
            
            return uint.Parse(input.Substring(left + 1, right - left - 1), CultureInfo.InvariantCulture);
        }
    }
}
