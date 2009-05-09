using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Lextm.SharpSnmpLib.Security
{
    public class SHA1AuthenticationProvider : IAuthenticationProvider
    {
        private byte[] _password;

        public SHA1AuthenticationProvider(OctetString phrase)
        {
            _password = phrase.GetRaw();
        }

        internal static byte[] PasswordToKey(byte[] userPassword, byte[] engineID)
        {
            // key length has to be at least 8 bytes long (RFC3414)
            if (userPassword == null || userPassword.Length < 8)
                throw new ArgumentException("Secret key is too short.", "userPassword");

            int password_index = 0;
            int count = 0;
            SHA1 sha = new SHA1CryptoServiceProvider();

            /* Use while loop until we've done 1 Megabyte */
            byte[] sourceBuffer = new byte[1048576];
            byte[] buf = new byte[64];
            while (count < 1048576)
            {
                for (int i = 0; i < 64; ++i)
                {
                    // Take the next octet of the password, wrapping
                    // to the beginning of the password as necessary.
                    buf[i] = userPassword[password_index++ % userPassword.Length];
                }
                Array.Copy(buf, 0, sourceBuffer, count, buf.Length);
                count += 64;
            }

            byte[] digest = sha.ComputeHash(sourceBuffer);

            MemoryStream buffer = new MemoryStream();
            buffer.Write(digest, 0, digest.Length);
            buffer.Write(engineID, 0, engineID.Length);
            buffer.Write(digest, 0, digest.Length);
            return sha.ComputeHash(buffer.ToArray());
        }

        #region IAuthenticationProvider Members

        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        public OctetString CleanDigest
        {
            get { return new OctetString(new byte[12]); }
        }

        public OctetString ComputeHash(GetRequestMessage message)
        {
            byte[] key = PasswordToKey(_password, message.Parameters.EngineId.GetRaw());
            HMACSHA1 sha1 = new HMACSHA1(key);
            byte[] buffer = message.ToBytes();
            byte[] hash = sha1.ComputeHash(buffer);
            byte[] result = new byte[12];
            Array.Copy(hash, result, result.Length);
            return new OctetString(result);
        }

        #endregion
    }
}
