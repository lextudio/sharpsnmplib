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
    /// Description of MessageHandlerFactory.
    /// </summary>
    internal class MessageHandlerFactory
    {
        private readonly IList<HandlerMapping> _mappings = new List<HandlerMapping>();
        private readonly NullMessageHandler _nullHandler = new NullMessageHandler(null);

        public MessageHandlerFactory(ObjectStore store)
        {
            _mappings.Add(new HandlerMapping("v1", "GET", "Lextm.SharpSnmpLib.Agent.GetMessageHandler", "snmpd", store));
            _mappings.Add(new HandlerMapping("v1", "SET", "Lextm.SharpSnmpLib.Agent.SetMessageHandler", "snmpd", store));
            _mappings.Add(new HandlerMapping("v1", "SET", "Lextm.SharpSnmpLib.Agent.GetNextMessageHandler", "snmpd", store));
            _mappings.Add(new HandlerMapping("*", "*", "Lextm.SharpSnmpLib.Agent.NullMessageHandler", "snmpd", store));
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
