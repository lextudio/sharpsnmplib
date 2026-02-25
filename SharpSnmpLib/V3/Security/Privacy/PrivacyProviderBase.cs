using DotNetSnmp.Asn1;
using DotNetSnmp.Common.Helpers;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using System.Buffers;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    public abstract class PrivacyProviderBase : IPrivacyProvider
    {
        private int _salt = -1;
        protected Memory<byte> EngineBootsMemory;
        protected Memory<byte> EngineTimeMemory;
        public IAuthenticationProvider AuthenticationProvider { get; }
        protected ReadOnlyMemory<byte> Passcode;

        protected PrivacyProviderBase(int privacyParametersLength, IAuthenticationProvider authenticationService, ReadOnlyMemory<byte> passcode)
        {
            PrivacyParametersLength = privacyParametersLength;
            AuthenticationProvider = authenticationService;
            Passcode = passcode;
        }

        public int PrivacyParametersLength { get; }
        public int EngineTime { get; protected set; }
        public int EngineBoots { get; protected set; }
        public abstract void DecryptScopedPdu(in ReadOnlyMemory<byte> encryptedPdu, in UsmSecurityParameters parameters, Span<byte> decryptedPdu);
        public virtual byte[] PrepareBuffer(int encodedLength)
        {
            return ArrayPool<byte>.Shared.Rent(encodedLength);
        }

        public abstract int EncryptScopedPdu(in ReadOnlyMemory<byte> scopedPdu, in UsmSecurityParameters parameters, Span<byte> encryptedScopedPdu);

        public void DecryptMessage(SnmpV3Message message)
        {
            byte[] rentedArray = ArrayPool<byte>.Shared.Rent(message.EncryptedScopedPdu.Length);

            try
            {
                DecryptScopedPdu(
                    message.EncryptedScopedPdu,
                    message.SecurityParameters,
                    rentedArray
                );

                // Parse the decrypted Scoped PDU
                var asnReader = new System.Formats.Asn1.AsnReader(rentedArray, System.Formats.Asn1.AsnEncodingRules.BER);
                message.Scope = Scope.ReadFrom(asnReader);
            }
            finally
            {
                // Always return the original rented array, not a new one
                ArrayPool<byte>.Shared.Return(rentedArray, clearArray: true);
            }
        }

        public void EncryptMessage(SnmpV3Message message)
        {
            var encodedScopedPdu = message.Scope!.Encode();
            var encryptedPdu = PrepareBuffer(encodedScopedPdu.Length);
            try
            {
                message.SecurityParameters.PrivParams = new byte[PrivacyParametersLength];

                var size = EncryptScopedPdu(encodedScopedPdu, message.SecurityParameters, encryptedPdu);

                message.Scope = null;
                Memory<byte> encryptedScopedPdu = new byte[size];
                encryptedPdu[..size].CopyTo(encryptedScopedPdu);
                message.EncryptedScopedPdu = encryptedScopedPdu;
            }
            finally
            {
                // Always return the original rented array, not a new one
                ArrayPool<byte>.Shared.Return(encryptedPdu, clearArray: true);
            }
        }

        private readonly object _saltLock = new object();

        protected int GetNextSalt()
        {
            lock (_saltLock)
            {
                if (_salt < 0)
                {
                    _salt = Random.Shared.Next();
                    return _salt;
                }

                _salt = (_salt + 1) % int.MaxValue;

                return _salt;
            }
        }

        protected void UpdateEngineBoots(int authoritativeEngineBoots)
        {
            EngineBoots = authoritativeEngineBoots;

            BinaryHelpers.CopyBytesMostSignificantFirst(
                authoritativeEngineBoots,
                EngineBootsMemory.Span);
        }

        protected void UpdateEngineTime(int authoritativeEngineTime)
        {
            EngineTime = authoritativeEngineTime;

            BinaryHelpers.CopyBytesMostSignificantFirst(
                authoritativeEngineTime,
                EngineTimeMemory.Span);
        }
    }
}
