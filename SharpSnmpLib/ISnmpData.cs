/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 11:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP data entity.
    /// </summary>
    public interface ISnmpData
    {
        /// <summary>
        /// Type code
        /// </summary>
        SnmpType TypeCode
        {
            get;
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        void AppendBytesTo(Stream stream);
    }
}
