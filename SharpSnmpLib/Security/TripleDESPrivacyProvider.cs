﻿// DES privacy provider.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider for 3DES.
    /// </summary>
    /// <remarks>Ported from SNMP#NET Privacy3DES class.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DES", Justification = "definition")]
    public sealed class TripleDESPrivacyProvider : IPrivacyProvider
    {
        private readonly SaltGenerator _salt = new SaltGenerator();
        private readonly OctetString _phrase;

        /// <summary>
        /// Initializes a new instance of the <see cref="DESPrivacyProvider"/> class.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="auth">The authentication provider.</param>
        public TripleDESPrivacyProvider(OctetString phrase, IAuthenticationProvider auth)
        {
            if (auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }

            // IMPORTANT: in this way privacy cannot be non-default.
            if (auth == DefaultAuthenticationProvider.Instance)
            {
                throw new ArgumentException("If authentication is off, then privacy cannot be used.", nameof(auth));
            }

            _phrase = phrase ?? throw new ArgumentNullException(nameof(phrase));
            AuthenticationProvider = auth;
        }

        /// <summary>
        /// Corresponding <see cref="IAuthenticationProvider"/>.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; private set; }

        /// <summary>
        /// Engine ID.
        /// </summary>
        /// <remarks>This is an optional field, and only used by TRAP v2 authentication.</remarks>
        public OctetString EngineId { get; set; }

        /// <summary>
        /// Encrypt scoped PDU using DES encryption protocol
        /// </summary>
        /// <param name="unencryptedData">Unencrypted scoped PDU byte array</param>
        /// <param name="key">Encryption key. Key has to be at least 32 bytes is length</param>
        /// <param name="privacyParameters">Privacy parameters out buffer. This field will be filled in with information
        /// required to decrypt the information. Output length of this field is 8 bytes and space has to be reserved
        /// in the USM header to store this information</param>
        /// <returns>Encrypted byte array</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when encryption key is null or length of the encryption key is too short.</exception>
        public static byte[] Encrypt(byte[] unencryptedData, byte[] key, byte[] privacyParameters)
        {
            if (unencryptedData == null)
            {
                throw new ArgumentNullException(nameof(unencryptedData));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (privacyParameters == null)
            {
                throw new ArgumentNullException(nameof(privacyParameters));
            }

            if (key.Length < MinimumKeyLength)
            {
                throw new ArgumentException($"Encryption key length has to 32 bytes or more. Current: {key.Length}.", nameof(key));
            }

            //privacyParameters = GetSalt(engineBoots);
            //byte[] privParamHash = authDigest.ComputeHash(privacyParameters, 0, privacyParameters.Length);
            //privacyParameters = new byte[8];
            //Buffer.BlockCopy(privParamHash, 0, privacyParameters, 0, 8);

            var iv = GetIV(key, privacyParameters);

            // DES uses 8 byte keys but we need 16 to encrypt ScopedPdu. Get first 8 bytes and use them as encryption key
            var outKey = GetKey(key);
            byte[] encryptedData = null;
            using (TripleDES des = TripleDES.Create())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.None;

                using (var transform = des.CreateEncryptor(outKey, iv))
                {
                    if ((unencryptedData.Length % 8) == 0)
                    {
                        encryptedData = transform.TransformFinalBlock(unencryptedData, 0, unencryptedData.Length);
                    }
                    else
                    {
                        byte[] tmpbuffer = new byte[8 * ((unencryptedData.Length / 8) + 1)];
                        Buffer.BlockCopy(unencryptedData, 0, tmpbuffer, 0, unencryptedData.Length);
                        encryptedData = transform.TransformFinalBlock(tmpbuffer, 0, tmpbuffer.Length);
                    }
                }
            }

            return encryptedData;
        }

        /// <summary>
        /// Decrypt DES encrypted scoped PDU.
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
                throw new ArgumentNullException(nameof(encryptedData));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (privacyParameters == null)
            {
                throw new ArgumentNullException(nameof(privacyParameters));
            }

            if (encryptedData.Length == 0)
            {
                throw new ArgumentException("Empty encrypted data.", nameof(encryptedData));
            }

            if ((encryptedData.Length % 8) != 0)
            {
                throw new ArgumentException("Encrypted data buffer has to be divisible by 8.", nameof(encryptedData));
            }

            if (privacyParameters.Length != PrivacyParametersLength)
            {
                throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
            }

            if (key.Length < MinimumKeyLength)
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Decryption key has to be at least 16 bytes long.");
            }

            var iv = GetIV(key, privacyParameters);
            using (TripleDES des = TripleDES.Create())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.Zeros;

                // normalize key - generated key is 32 bytes long, we need 24 bytes to encrypt
                var outKey = new byte[24];
                Buffer.BlockCopy(key, 0, outKey, 0, outKey.Length);

                using (var transform = des.CreateDecryptor(outKey, iv))
                {
                    var decryptedData = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return decryptedData;
                }
            }
        }

        /// <summary>
        /// Generate IV from the privacy key and salt value returned by GetSalt method.
        /// </summary>
        /// <param name="privacyKey">32 byte privacy key</param>
        /// <param name="salt">Salt value returned by GetSalt method</param>
        /// <returns>IV value used in the encryption process</returns>
        private static byte[] GetIV(IList<byte> privacyKey, IList<byte> salt)
        {
            if (privacyKey.Count < MinimumKeyLength)
            {
                throw new ArgumentException("Invalid privacy key length", nameof(privacyKey));
            }

            var iv = new byte[8];
            for (var i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)(salt[i] ^ privacyKey[24 + i]);
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
            if (privacyPassword.Length < 32)
            {
                throw new ArgumentException("Invalid privacy key length.", nameof(privacyPassword));
            }

            var key = new byte[24];
            Buffer.BlockCopy(privacyPassword, 0, key, 0, key.Length);
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
            get { return MaximumKeyLength; }
        }

        /// <summary>
        /// Return maximum encryption/decryption key length. For DES, returned value is 16.
        /// 
        /// DES protocol itself requires an 8 byte key. Additional 8 bytes are used for generating the
        /// encryption IV. For encryption itself, first 8 bytes of the key are used.
        /// </summary>
        public static int MaximumKeyLength
        {
            get { return 32; }
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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var code = data.TypeCode;
            if (code != SnmpType.OctetString)
            {
                throw new ArgumentException($"Cannot decrypt the scope data: {code}.", nameof(data));
            }

            var octets = (OctetString)data;
            var bytes = octets.GetRaw();
            var pkey = PasswordToKey(_phrase.GetRaw(), parameters.EngineId.GetRaw());

            try
            {
                // decode encrypted packet
                var decrypted = Decrypt(bytes, pkey, parameters.PrivacyParameters.GetRaw());            
                var result = DataFactory.CreateSnmpData(decrypted);
                if (result.TypeCode != SnmpType.Sequence)
                {
                    var newException = new DecryptionException("DES decryption failed");
                    newException.SetBytes(bytes);
                    throw newException;
                }

                return result;
            }
            catch (Exception ex)
            {
                var newException = new DecryptionException("DES decryption failed", ex);
                newException.SetBytes(bytes);
                throw newException;
            }
        }

        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="data">The scope data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Encrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (data.TypeCode != SnmpType.Sequence && !(data is ISnmpPdu))
            {
                throw new ArgumentException("Invalid data type.", nameof(data));
            }

            var pkey = PasswordToKey(_phrase.GetRaw(), parameters.EngineId.GetRaw());
            var bytes = data.ToBytes();
            var reminder = bytes.Length % 8;
            var count = reminder == 0 ? 0 : 8 - reminder;
            using (var stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                for (var i = 0; i < count; i++)
                {
                    stream.WriteByte(1);
                }

                bytes = stream.ToArray();
            }

            var encrypted = Encrypt(bytes, pkey, parameters.PrivacyParameters.GetRaw());
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

        /// <summary>
        /// Passwords to key.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="engineId">The engine identifier.</param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] secret, byte[] engineId)
        {
            byte[] encryptionKey = AuthenticationProvider.PasswordToKey(secret, engineId);
            if (encryptionKey.Length < MinimumKeyLength)
            {
                encryptionKey = ExtendShortKey(encryptionKey, secret, engineId, AuthenticationProvider);
            }

            return encryptionKey;
        }

        private byte[] ExtendShortKey(byte[] shortKey, byte[] password, byte[] engineID, IAuthenticationProvider authProtocol)
        {
            int length = shortKey.Length;
            byte[] extendedKey = new byte[MinimumKeyLength];
            Buffer.BlockCopy(shortKey, 0, extendedKey, 0, shortKey.Length);

            while (length < MinimumKeyLength)
            {
                byte[] key =
                    authProtocol.PasswordToKey(shortKey,
                                               engineID);
                int copyBytes = Math.Min(MaximumKeyLength - length,
                                         authProtocol.DigestLength);
                Buffer.BlockCopy(key, 0, extendedKey, length, copyBytes);
                length += copyBytes;
            }

            return extendedKey;
        }

        #endregion

        /// <summary>
        /// Returns a string that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "3DES privacy provider";
        }
    }
}
