using System;
using System.IO;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider for DES.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DES", Justification = "definition")]
    public class DESPrivacyProvider : IPrivacyProvider
    {
        private readonly IAuthenticationProvider _auth;
        private readonly SaltGenerator _salt = new SaltGenerator();
        private readonly OctetString _phrase;

        /// <summary>
        /// Initializes a new instance of the <see cref="DESPrivacyProvider"/> class.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="auth">The auth.</param>
        public DESPrivacyProvider(OctetString phrase, IAuthenticationProvider auth)
        {
            _phrase = phrase;
            _auth = auth;
        }

        /// <summary>
        /// Encrypt ScopedPdu using DES encryption protocol
        /// </summary>
        /// <param name="unencryptedData">Unencrypted ScopedPdu byte array</param>
        /// <param name="key">Encryption key. Key has to be at least 32 bytes is length</param>
        /// <param name="privacyParameters">Privacy parameters out buffer. This field will be filled in with information
        /// required to decrypt the information. Output length of this field is 8 bytes and space has to be reserved
        /// in the USM header to store this information</param>
        /// <returns>Encrypted byte array</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when encryption key is null or length of the encryption key is too short.</exception>
        public static byte[] Encrypt(byte[] unencryptedData, byte[] key, byte[] privacyParameters)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            
            if (key.Length < MinimumKeyLength)
            {
                throw new ArgumentOutOfRangeException("key", "Encryption key length has to 32 bytes or more.");
            }
            
            if (unencryptedData == null)
            {
                throw new ArgumentNullException("unencryptedData");
            }

            byte[] iv = GetIV(key, privacyParameters);

            // DES uses 8 byte keys but we need 16 to encrypt ScopedPdu. Get first 8 bytes and use them as encryption key
            byte[] outKey = GetKey(key);

            int div = (int)Math.Floor(unencryptedData.Length / 8.0);
            if ((unencryptedData.Length % 8) != 0)
            {
                div += 1;
            }
            
            int newLength = div * 8;
            byte[] result = new byte[newLength];
            byte[] buffer = new byte[newLength];

            byte[] inbuffer = new byte[8];
            byte[] cipherText = iv;
            int posIn = 0;
            int posResult = 0;
            Array.Copy(unencryptedData, 0, buffer, 0, unencryptedData.Length);

            using (DES des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;

                using (ICryptoTransform transform = des.CreateEncryptor(outKey, null))
                {
                    for (int b = 0; b < div; b++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            inbuffer[i] = (byte)(buffer[posIn] ^ cipherText[i]);
                            posIn++;
                        }
                        
                        /*int byteCount =*/ transform.TransformBlock(inbuffer, 0, inbuffer.Length, cipherText, 0);
                        Array.Copy(cipherText, 0, result, posResult, cipherText.Length);
                        posResult += cipherText.Length;
                    }
                }
                
                des.Clear();
            }

            return result;
        }

        /// <summary>
        /// Decrypt DES encrypted ScopedPdu
        /// </summary>
        /// <param name="encryptedData">Source data buffer</param>
        /// <param name="key">Decryption key. Key length has to be 32 bytes in length or longer (bytes beyond 32 bytes are ignored).</param>
        /// <param name="privacyParameters">Privacy parameters extracted from USM header</param>
        /// <returns>Decrypted byte array</returns>
        /// <exception cref="ArgumentNullException">Thrown when encrypted data is null or length == 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when encryption key length is less then 32 byte or if privacy parameters
        /// argument is null or length other then 8 bytes</exception>
        public static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] privacyParameters)
        {
            if (encryptedData == null)
            {
                throw new ArgumentNullException("encryptedData");
            }
            
            if (encryptedData.Length == 0)
            {
                throw new ArgumentException("empty encrypted data", "encryptedData");
            }
            
            if ((encryptedData.Length % 8) != 0)
            {
                throw new ArgumentException("Encrypted data buffer has to be divisible by 8.", "encryptedData");
            }
            
            if (privacyParameters == null)
            {
                throw new ArgumentNullException("privacyParameters");
            }
            
            if (privacyParameters.Length != PrivacyParametersLength)
            {
                throw new ArgumentOutOfRangeException("privacyParameters", "Privacy parameters argument has to be 8 bytes long");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            
            if (key.Length < MinimumKeyLength)
            {
                throw new ArgumentOutOfRangeException("key", "Decryption key has to be at least 16 bytes long.");
            }

            byte[] iv = new byte[8];
            for (int i = 0; i < 8; ++i)
            {
                iv[i] = (byte)(key[8 + i] ^ privacyParameters[i]);
            }     
            
            using (DES des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.Zeros;

                // .NET implementation only takes an 8 byte key
                byte[] outKey = new byte[8];
                Array.Copy(key, outKey, 8);

                des.Key = outKey;
                des.IV = iv;
                using (ICryptoTransform transform = des.CreateDecryptor())
                {
                    byte[] decryptedData = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    des.Clear();
                    return decryptedData;
                }
            }
        }

        /// <summary>
        /// Generate IV from the privacy key and salt value returned by GetSalt method.
        /// </summary>
        /// <param name="privacyKey">16 byte privacy key</param>
        /// <param name="salt">Salt value returned by GetSalt method</param>
        /// <returns>IV value used in the encryption process</returns>
        private static byte[] GetIV(byte[] privacyKey, byte[] salt)
        {
            if (privacyKey.Length < 16)
            {
                throw new ArgumentException("Invalid privacy key length", "privacyKey");
            }
            
            byte[] iv = new byte[8];
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)(salt[i] ^ privacyKey[8 + i]);
            }
            
            return iv;
        }

        /// <summary>
        /// Extract and return DES encryption key.
        /// Privacy password is 16 bytes in length. Only the first 8 bytes are used as DES password. Remaining
        /// 8 bytes are used as pre-IV value.
        /// </summary>
        /// <param name="privacyPassword">16 byte privacy password</param>
        /// <returns>8 byte DES encryption password</returns>
        private static byte[] GetKey(byte[] privacyPassword)
        {
            if (privacyPassword == null || privacyPassword.Length < 16)
            {
                throw new ArgumentException("Invalid privacy key length.", "privacyPassword");
            }
            
            byte[] key = new byte[8];
            Array.Copy(privacyPassword, key, 8);
            return key;
        }
        
        /// <summary>
        /// Returns the length of privacyParameters USM header field. For DES, field length is 8.
        /// </summary>
        public static int PrivacyParametersLength
        {
            get { return 8; }
        }

        /// <summary>
        /// Returns minimum encryption/decryption key length. For DES, returned value is 16.
        /// 
        /// DES protocol itself requires an 8 byte key. Additional 8 bytes are used for generating the
        /// encryption IV. For encryption itself, first 8 bytes of the key are used.
        /// </summary>
        public static int MinimumKeyLength
        {
            get { return 16; }
        }
        
        /// <summary>
        /// Return maximum encryption/decryption key length. For DES, returned value is 16
        /// 
        /// DES protocol itself requires an 8 byte key. Additional 8 bytes are used for generating the
        /// encryption IV. For encryption itself, first 8 bytes of the key are used.
        /// </summary>
        public static int MaximumKeyLength
        {
            get { return 16; }
        }

        #region IPrivacyProvider Members

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Decrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            
            if (data.TypeCode != SnmpType.OctetString)
            {
                throw new ArgumentException("cannot decrypt the scope data", "data");
            }
            
            OctetString octets = (OctetString)data;
            byte[] bytes = octets.GetRaw();
            byte[] pkey = _auth.PasswordToKey(_phrase.GetRaw(), parameters.EngineId.GetRaw());

            // decode encrypted packet
            byte[] decrypted = Decrypt(bytes, pkey, parameters.PrivacyParameters.GetRaw());
            return DataFactory.CreateSnmpData(decrypted);
        }

        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="data">The scope data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Encrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            byte[] pkey = _auth.PasswordToKey(_phrase.GetRaw(), parameters.EngineId.GetRaw());
            byte[] bytes = ByteTool.ToBytes(data);
            int reminder = bytes.Length % 8;
            int count = reminder == 0 ? 0 : 8 - reminder;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                for (int i = 0; i < count; i++)
                {
                    stream.WriteByte(1);
                }

                bytes = stream.ToArray();
            }
            
            byte[] encrypted = Encrypt(bytes, pkey, parameters.PrivacyParameters.GetRaw());
            return new OctetString(encrypted);
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public OctetString Salt
        {
            get { return new OctetString(_salt.GetSaltBytes()); }
        }

        #endregion
    }
}
