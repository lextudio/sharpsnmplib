namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// A placeholder.
    /// </summary>
    internal class NullMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public ResponseData Handle(ISnmpMessage message, ObjectStore store)
        {
            return new ResponseData(null, ErrorCode.NoError, 0);
        }
    }
}
