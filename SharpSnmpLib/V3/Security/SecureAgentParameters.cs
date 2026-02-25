using System.Security.Cryptography;
using System.Text;

namespace DotNetSnmp.Protocol.V3.Security
{
    /// <summary>
    /// Represents the SecureAgentParameters type.
    /// </summary>
    public class SecureAgentParameters
    {
        private string? _securityName;
        private byte[]? _userPassphraseBytes;

        /// <summary>
        /// Gets hash Algorithm.
        /// </summary>
        public HashAlgorithmName? HashAlgorithm { get; set; }

        /// <summary>
        /// Represents this member.
        /// </summary>
        public string? SecurityName
        {
            get => _securityName;
            set
            {
                _securityName = value;
            }
        }

        /// <summary>
        /// Represents this member.
        /// </summary>
        public string UserPassphrase
        {
            set
            {
                _userPassphraseBytes = Encoding.UTF8.GetBytes(value);
            }
        }

        /// <summary>
        /// Stores user Passphrase Bytes.
        /// </summary>
        public ReadOnlyMemory<byte> UserPassphraseBytes => _userPassphraseBytes;
    }
}
