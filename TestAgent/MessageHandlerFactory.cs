/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Description of MessageHandlerFactory.
    /// </summary>
    internal class MessageHandlerFactory
    {
        private readonly HandlerMapping[] _mappings;
        private readonly NullMessageHandler _nullHandler = new NullMessageHandler();

        public MessageHandlerFactory(HandlerMapping[] mappings)
        {
            _mappings = mappings;
        }
        
        public IMessageHandler GetHandler(ISnmpMessage message)
        {
            foreach (HandlerMapping mapping in _mappings)
            {
                if (mapping.CanHandle(message))
                {
                    return mapping.Handler;
                }
            }

            return _nullHandler;
        }
    }
}
