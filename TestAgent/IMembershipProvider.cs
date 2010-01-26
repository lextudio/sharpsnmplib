namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Membership provider interface.
    /// </summary>
    internal interface IMembershipProvider
    {
        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        bool AuthenticateRequest(ISnmpMessage message);
    }
}