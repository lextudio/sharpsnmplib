namespace Lextm.SharpSnmpLib.Agent
{
    internal interface MembershipProvider
    {
        bool AuthenticateRequest(ISnmpMessage message);
    }
}