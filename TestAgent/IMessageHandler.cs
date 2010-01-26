/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Message handler interface.
    /// </summary>
	internal interface IMessageHandler
	{
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        IList<Variable> Handle(ISnmpMessage message, ObjectStore store);
        
        /// <summary>
        /// Gets the error status.
        /// </summary>
        /// <value>The error status.</value>
		ErrorCode ErrorStatus { get; }
        
        /// <summary>
        /// Gets the index of the error.
        /// </summary>
        /// <value>The index of the error.</value>
		int ErrorIndex { get; }
	}
}
