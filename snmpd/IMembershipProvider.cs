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
        /// <param name="context">The context.</param>
        /// <returns></returns>
        bool AuthenticateRequest(SnmpContext context);
    }
}