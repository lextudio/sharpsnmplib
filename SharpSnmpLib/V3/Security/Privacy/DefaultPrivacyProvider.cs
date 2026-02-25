using System.Buffers;
using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy;

public class DefaultPrivacyProvider : IPrivacyProvider
{
    public int PrivacyParametersLength => throw new NotImplementedException();

    public int EngineTime => throw new NotImplementedException();

    public int EngineBoots => throw new NotImplementedException();

    public IAuthenticationProvider AuthenticationProvider { get; }

    public DefaultPrivacyProvider()
    : this(DefaultAuthenticationProvider.Instance)
    {
    }

    public DefaultPrivacyProvider(IAuthenticationProvider authenticationProvider)
    {
        AuthenticationProvider = authenticationProvider;
    }

    public void DecryptScopedPdu(in ReadOnlyMemory<byte> encryptedPdu, in UsmSecurityParameters parameters, Span<byte> decryptedPdu)
    {

    }

    public int EncryptScopedPdu(in ReadOnlyMemory<byte> scopedPdu, in UsmSecurityParameters parameters, Span<byte> encryptedScopedPdu)
    {
        return 0;
    }

    public void DecryptMessage(SnmpV3Message message)
    {

    }

    public void EncryptMessage(SnmpV3Message message)
    {
        // No encryption needed for NoPrivacyService
    }

    public byte[] PrepareBuffer(int encodedLength)
    {
        return ArrayPool<byte>.Shared.Rent(encodedLength);
    }
}
