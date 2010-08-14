namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Membership provider interface.
    /// </summary>
    public interface IMembershipProvider
    {
        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        bool AuthenticateRequest(SnmpContext context);
    }
}