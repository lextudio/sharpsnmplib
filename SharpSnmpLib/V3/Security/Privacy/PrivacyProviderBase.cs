using DotNetSnmp.Asn1;
using DotNetSnmp.Common.Helpers;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using System.Buffers;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    /// <summary>
    /// Represents the PrivacyProviderBase type.
    /// </summary>
    public abstract class PrivacyProviderBase : IPrivacyProvider
    {
        private int _salt = -1;
        /// <summary>
        /// Represents engine Boots Memory.
        /// </summary>
        protected Memory<byte> EngineBootsMemory;
        /// <summary>
        /// Represents engine Time Memory.
        /// </summary>
        protected Memory<byte> EngineTimeMemory;
        /// <summary>
        /// Gets the authentication provider associated with this privacy provider.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; }
        /// <summary>
        /// Represents passcode.
        /// </summary>
        protected ReadOnlyMemory<byte> Passcode;

        /// <summary>
        /// Initializes a new instance of PrivacyProviderBase.
        /// </summary>
        protected PrivacyProviderBase(int privacyParametersLength, IAuthenticationProvider authenticationService, ReadOnlyMemory<byte> passcode)
        {
            PrivacyParametersLength = privacyParametersLength;
            AuthenticationProvider = authenticationService;
            Passcode = passcode;
        }

        /// <inheritdoc/>
        public int PrivacyParametersLength { get; }
        /// <inheritdoc/>
        public int EngineTime { get; protected set; }
        /// <inheritdoc/>
        public int EngineBoots { get; protected set; }
        /// <summary>
        /// Decrypts an encrypted scoped PDU payload into plaintext bytes.
        /// </summary>
        public abstract void DecryptScopedPdu(in ReadOnlyMemory<byte> encryptedPdu, in UsmSecurityParameters parameters, Span<byte> decryptedPdu);
        /// <summary>
        /// Allocates a temporary buffer for scoped-PDU encryption output.
        /// </summary>
        public virtual byte[] PrepareBuffer(int encodedLength)
        {
            return ArrayPool<byte>.Shared.Rent(encodedLength);
        }

        /// <summary>
        /// Encrypts a scoped PDU payload and returns the number of bytes written.
        /// </summary>
        public abstract int EncryptScopedPdu(in ReadOnlyMemory<byte> scopedPdu, in UsmSecurityParameters parameters, Span<byte> encryptedScopedPdu);

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <summary>
        /// Gets next Salt.
        /// </summary>
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

        /// <summary>
        /// Updates engine Boots.
        /// </summary>
        protected void UpdateEngineBoots(int authoritativeEngineBoots)
        {
            EngineBoots = authoritativeEngineBoots;

            BinaryHelpers.CopyBytesMostSignificantFirst(
                authoritativeEngineBoots,
                EngineBootsMemory.Span);
        }

        /// <summary>
        /// Updates engine Time.
        /// </summary>
        protected void UpdateEngineTime(int authoritativeEngineTime)
        {
            EngineTime = authoritativeEngineTime;

            BinaryHelpers.CopyBytesMostSignificantFirst(
                authoritativeEngineTime,
                EngineTimeMemory.Span);
        }
    }
}
