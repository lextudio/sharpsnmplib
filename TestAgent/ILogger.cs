/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 3/14/2010
 * Time: 1:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Logger interface.
    /// </summary>
    internal interface ILogger
    {
        /// <summary>
        /// Logs an entry.
        /// </summary>
        /// <param name="context">Message context.</param>
        void Log(SnmpContext context);
    }
}
