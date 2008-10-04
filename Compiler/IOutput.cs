/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/29
 * Time: 10:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Compiler
{
    /// <summary>
    /// IOutput interface.
    /// </summary>
    public interface IOutput
    {        
        /// <summary>
        /// Reports a message.
        /// </summary>
        /// <param name="message">Message.</param>
        void ReportMessage(string message); 
    }
}
