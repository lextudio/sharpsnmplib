/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Linq;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Message handler factory.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class MessageHandlerFactory
    {
        private readonly HandlerMapping[] _mappings;
        private readonly NullMessageHandler _nullHandler = new NullMessageHandler();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerFactory"/> class.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        public MessageHandlerFactory(HandlerMapping[] mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }

            _mappings = mappings;
        }

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public IMessageHandler GetHandler(ISnmpMessage message)
        {
            foreach (var mapping in _mappings.Where(mapping => mapping.CanHandle(message)))
            {
                return mapping.Handler;
            }

            return _nullHandler;
        }
    }
}
