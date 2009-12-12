namespace Lextm.SharpSnmpLib.Agent
{
    internal interface IMembershipProvider
    {
        bool AuthenticateRequest(ISnmpMessage message);
    }
}