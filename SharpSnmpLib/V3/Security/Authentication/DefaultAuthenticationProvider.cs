namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IAuthenticationProvider"/> interface.
    /// This class implements a pass-through authentication method that does not perform
    /// any actual authentication.
    /// </summary>
    /// <remarks>
    /// This provider is suitable for scenarios where authentication is not required
    /// or is handled by another component. This class follows the Singleton pattern
    /// to ensure only one instance exists throughout the application.
    /// </remarks>
    public class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        // Singleton instance
        private static DefaultAuthenticationProvider? _instance;

        // Lock object for thread safety
        private static readonly object _lock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultAuthenticationProvider"/> class from being created.
        /// </summary>
        private DefaultAuthenticationProvider()
        {
        }

        /// <summary>
        /// Represents this member.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static DefaultAuthenticationProvider Instance
        {
            get
            {
                // Double-check locking pattern for thread safety and performance
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new DefaultAuthenticationProvider();
                    }
                }
                return _instance;
            }
        }

        /// <inheritdoc/>
        public int DigestSize => 0;

        /// <inheritdoc/>
        public int TruncatedDigestSize => 0;

        /// <inheritdoc/>
        public void AuthenticateOutgoingMsg(SnmpV3Message message, Memory<byte> newAuthParams)
        {
            // Default implementation does not perform any authentication
        }

        /// <inheritdoc/>
        public bool AuthenticateIncomingMsg(SnmpV3Message message)
        {
            // Default implementation always authenticates successfully
            return true;
        }

        /// <inheritdoc/>
        public void PasswordToKey(in ReadOnlyMemory<byte> secret, in ReadOnlyMemory<byte> engineId, Span<byte> destination)
        {
            // Simply copies the secret to the destination without transformation
            secret.Span.CopyTo(destination);
        }
    }
}
