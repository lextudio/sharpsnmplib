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
        private readonly ObjectStore _store;
        
        public MessageHandlerFactory(ObjectStore store)
        {
            _store = store;
        }
        
        public IMessageHandler GetHandler(ISnmpMessage message)
        {
            switch (message.Pdu.TypeCode)
            {
                case SnmpType.GetRequestPdu:
                    return new GetMessageHandler(message, _store);
                case SnmpType.GetNextRequestPdu:
                    return new GetNextMessageHandler(message, _store);
                case SnmpType.SetRequestPdu:
                    return new SetMessageHandler(message, _store);
                default:
                    return null;
            }
        }
    }
}
