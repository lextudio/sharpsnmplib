// SHA-256 authentication provider.
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
#if !NETFX_CORE
using System;
using System.Globalization;
using System.IO;

using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider using SHA-256.
    /// </summary>
    /// <remarks>Defined in https://tools.ietf.org/html/rfc7630#page-3.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SHA", Justification = "definition")]
    public sealed class SHA256AuthenticationProvider : IAuthenticationProvider
    {
        private const int Sha256KeyCacheCapacity = 100; 
        private static readonly CryptoKeyCache Sha256KeyCache = new CryptoKeyCache(Sha256KeyCacheCapacity);
        private static readonly Object Sha256KeyCacheLock = new object();

        private readonly byte[] _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="SHA256AuthenticationProvider"/> class.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        public SHA256AuthenticationProvider(OctetString phrase)
        {
            if (phrase == null)
            {
                throw new ArgumentNullException(nameof(phrase));
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
                throw new ArgumentNullException(nameof(password));
            }

            if (engineId == null)
            {
                throw new ArgumentNullException(nameof(engineId));
            }

            if (password.Length < 8)
            {
                throw new ArgumentException($"Secret key is too short. Must be >= 8. Current: {password.Length}.", nameof(password));
            }

            lock (Sha256KeyCacheLock)
            {
                byte[] cachedKey;
                if (Sha256KeyCache.TryGetCachedValue(password, engineId, out cachedKey))
                {
                    return cachedKey;
                }
                 
                byte[] keyToCache = _PasswordToKey(password, engineId);
                //Value not in cache compute and cache the value
                Sha256KeyCache.AddValueToCache(password, engineId, keyToCache);
                return keyToCache;
            }
        }

        private byte[] _PasswordToKey(byte[] password, byte[] engineId)
        {          
            using (SHA256 sha = SHA256.Create())
            {
                var passwordIndex = 0;
                var count = 0;
                /* Use while loop until we've done 1 Megabyte */
                var sourceBuffer = new byte[1048576];
                var buf = new byte[64];
                while (count < 1048576)
                {
                    for (var i = 0; i < 64; ++i)
                    {
                        // Take the next octet of the password, wrapping
                        // to the beginning of the password as necessary.
                        buf[i] = password[passwordIndex++ % password.Length];
                    }
                    
                    Buffer.BlockCopy(buf, 0, sourceBuffer, count, buf.Length);
                    count += 64;
                }

                var digest = sha.ComputeHash(sourceBuffer);

                using (var buffer = new MemoryStream())
                {
                    buffer.Write(digest, 0, digest.Length);
                    buffer.Write(engineId, 0, engineId.Length);
                    buffer.Write(digest, 0, digest.Length);
                    return sha.ComputeHash(buffer.ToArray());
                }
            }
        }

        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        public OctetString CleanDigest
        {
            get { return new OctetString(new byte[DigestLength]); }
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="data">The scope bytes.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="length">The length bytes.</param>
        /// <returns></returns>
        public OctetString ComputeHash(VersionCode version, ISegment header, SecurityParameters parameters, ISnmpData data, IPrivacyProvider privacy, byte[] length)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }
            
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            var key = PasswordToKey(_password, parameters.EngineId.GetRaw());
            using (var sha256 = new HMACSHA256(key))
            {
                var hash = sha256.ComputeHash(ByteTool.PackMessage(length, version, header, parameters, data).ToBytes());
#if NET452
                sha256.Clear();
#endif
                var result = new byte[DigestLength];
                Buffer.BlockCopy(hash, 0, result, 0, result.Length);
                return new OctetString(result);
            }
        }

        public int DigestLength => 24;

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "SHA-256 authentication provider";
        }
    }
}
#endif
