using System.Security.Cryptography;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Represents the SecureAgentParameters type.
    /// </summary>
    public class SecureAgentParameters
    {
        private byte[]? _userPassphraseBytes;

        /// <summary>
        /// Gets hash Algorithm.
        /// </summary>
        public HashAlgorithmName? HashAlgorithm { get; set; }

        /// <summary>
        /// Represents this member.
        /// </summary>
        public string? SecurityName { get; set; }

        /// <summary>
        /// Represents this member.
        /// </summary>
        public string UserPassphrase
        {
            get => _userPassphraseBytes != null ? Encoding.UTF8.GetString(_userPassphraseBytes) : string.Empty;
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
