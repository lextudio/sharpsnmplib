using System;
using System.IO;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider using MD5.
    /// </summary>
    public class MD5AuthenticationProvider : IAuthenticationProvider
    {
        private byte[] _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="MD5AuthenticationProvider"/> class.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        public MD5AuthenticationProvider(OctetString phrase)
        {
            if (phrase == null)
            {
                throw new ArgumentNullException("phrase");
            }
            
            _password = phrase.GetRaw();
        }
        
        #region IAuthenticationProvider Members

        /// <summary>
        /// Passwords to key.
        /// </summary>
        /// <param name="password">The user password.</param>
        /// <param name="engineId">The engine ID.</param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] password, byte[] engineId)
        {
            // key length has to be at least 8 bytes long (RFC3414)
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            
            if (password.Length < 8)
            {
                throw new ArgumentException("Secret key is too short.", "password");
            }
            
            if (engineId == null)
            {
                throw new ArgumentNullException("engineId");
            }

            int password_index = 0;
            int count = 0;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                /* Use while loop until we've done 1 Megabyte */
                byte[] sourceBuffer = new byte[1048576];
                byte[] buf = new byte[64];
                while (count < 1048576)
                {
                    for (int i = 0; i < 64; ++i)
                    {
                        // Take the next octet of the password, wrapping
                        // to the beginning of the password as necessary.
                        buf[i] = password[password_index++ % password.Length];
                    }
                    
                    Array.Copy(buf, 0, sourceBuffer, count, buf.Length);
                    count += 64;
                }

                byte[] digest = md5.ComputeHash(sourceBuffer);

                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(digest, 0, digest.Length);
                    buffer.Write(engineId, 0, engineId.Length);
                    buffer.Write(digest, 0, digest.Length);
                    return md5.ComputeHash(buffer.ToArray());
                }
            }
        }

        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        public OctetString CleanDigest
        {
            get { return new OctetString(new byte[12]); }
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public OctetString ComputeHash(ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            byte[] key = PasswordToKey(_password, message.Parameters.EngineId.GetRaw());
            using (HMACMD5 md5 = new HMACMD5(key))
            {
                byte[] buffer = message.ToBytes();
                byte[] hash = md5.ComputeHash(buffer);
                md5.Clear();                
                byte[] result = new byte[12];
                Array.Copy(hash, result, result.Length);
                return new OctetString(result);
            }
        }

        #endregion
    }
}
