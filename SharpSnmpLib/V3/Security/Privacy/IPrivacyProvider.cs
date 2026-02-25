using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    public interface IPrivacyProvider
    {
        public int PrivacyParametersLength { get; }
        int EngineTime { get; }
        int EngineBoots { get; }
        IAuthenticationProvider AuthenticationProvider { get; }

        void EncryptMessage(SnmpV3Message message);

        void DecryptMessage(SnmpV3Message message);
    }
}
