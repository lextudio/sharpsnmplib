using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Malformed message for v3 due to decryption failures or wrong user names.
    /// </summary>
    public sealed class MalformedMessage : ISnmpMessage
    {
        private static readonly Scope DefaultScope = new(OctetString.Empty, OctetString.Empty, new MalformedPdu());

        /// <summary>
        /// Initializes a new instance of <see cref="MalformedMessage"/>.
        /// </summary>
        public MalformedMessage(int messageId, OctetString user, ISnmpData data)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Header = new Header(messageId);
            Parameters = SecurityParameters.Create(user);
            Scope = DefaultScope;
            EncryptedScope = data;
        }

        /// <summary>
        /// Encrypted scope data from the original packet.
        /// </summary>
        public ISnmpData EncryptedScope { get; set; }

        /// <summary>
        /// Gets security parameters.
        /// </summary>
        public new SecurityParameters Parameters { get; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        public new Scope Scope { get; }

        /// <summary>
        /// Gets the v3 header.
        /// </summary>
        public new Header Header { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public VersionCode Version => VersionCode.V3;

        /// <inheritdoc/>
        public VersionCode ProtocolVersion => VersionCode.V3;

        /// <inheritdoc/>
        IScope? ISnmpMessage.Scope => Scope;

        /// <summary>
        /// Gets the privacy provider.
        /// </summary>
        public IPrivacyProvider Privacy => DefaultPrivacyProvider.DefaultPair;

        /// <summary>
        /// Returns an empty byte array.
        /// </summary>
        public byte[] ToBytes() => Array.Empty<byte>();

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer) { }

        /// <inheritdoc/>
        public override string ToString()
            => $"Malformed message: message id: {Header.MessageId}; user: {Parameters.UserName}";
    }
}
