using System;
using System.IO;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    public class MD5AuthenticationProvider : IAuthenticationProvider
    {
        private byte[] _password;

        public MD5AuthenticationProvider(OctetString phrase)
        {
            _password = phrase.GetRaw();
        }

        public byte[] PasswordToKey(byte[] userPassword, byte[] engineID)
        {
            // key length has to be at least 8 bytes long (RFC3414)
            if (userPassword == null || userPassword.Length < 8)
                throw new ArgumentException("Secret key is too short.", "userPassword");

            int password_index = 0;
            int count = 0;
            MD5 md5 = new MD5CryptoServiceProvider();

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

            byte[] digest = md5.ComputeHash(sourceBuffer);

            MemoryStream buffer = new MemoryStream();
            buffer.Write(digest, 0, digest.Length);
            buffer.Write(engineID, 0, engineID.Length);
            buffer.Write(digest, 0, digest.Length);
            return md5.ComputeHash(buffer.ToArray());
        }

        #region IAuthenticationProvider Members

        public OctetString CleanDigest
        {
            get { return new OctetString(new byte[12]); }
        }

        public OctetString ComputeHash(GetRequestMessage message)
        {
            byte[] key = PasswordToKey(_password, message.Parameters.EngineId.GetRaw());
            HMACMD5 md5 = new HMACMD5(key);
            byte[] buffer = message.ToBytes();
            byte[] hash = md5.ComputeHash(buffer);
            byte[] result = new byte[12];
            Array.Copy(hash, result, result.Length);
            return new OctetString(result);
        }

        #endregion
    }
}
