// AES privacy provider
// Copyright (C) 2009-2010 Lex Li, Milan Sinadinovic
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/30/2009
 * Time: 8:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider for AES 128.
    /// </summary>
    /// <remarks>
    /// This is an experimental port from SNMP#NET project. As AES is not part of SNMP RFC, this class is provided as it is.
    /// If you want other AES providers, you can port them from SNMP#NET in a similar manner.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "AES", Justification = "definition")]
    public class AESPrivacyProvider : IPrivacyProvider
    {        
        private readonly SaltGenerator _salt = new SaltGenerator();
        private readonly OctetString _phrase;
        private const int KeyBytes = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="AESPrivacyProvider"/> class.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="auth">The auth.</param>
        public AESPrivacyProvider(OctetString phrase, IAuthenticationProvider auth)
        {
            if (auth == null)
            {
                throw new ArgumentNullException("auth");
            }
            
            if (phrase == null)
            {
                throw new ArgumentNullException("phrase");
            }
            
            // IMPORTANT: in this way privacy cannot be non-default.
            if (auth == DefaultAuthenticationProvider.Instance)
            {
                throw new ArgumentException("if authentication is off, then privacy cannot be used");
            }
            
            _phrase = phrase;
            AuthenticationProvider = auth;
        }

        /// <summary>
        /// Corresponding <see cref="IAuthenticationProvider"/>.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; private set; }
        
        /// <summary>
        /// Encrypt ScopedPdu using DES encryption protocol
        /// </summary>
        /// <param name="unencryptedData">Unencrypted ScopedPdu byte array</param>
        /// <param name="key">Encryption key. Key has to be at least 32 bytes is length</param>
        /// <param name="engineBoots">Engine boots.</param>
        /// <param name="engineTime">Engine time.</param>
        /// <param name="privacyParameters">Privacy parameters out buffer. This field will be filled in with information
        /// required to decrypt the information. Output length of this field is 8 bytes and space has to be reserved
        /// in the USM header to store this information</param>
        /// <returns>Encrypted byte array</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when encryption key is null or length of the encryption key is too short.</exception>
        private static byte[] Encrypt(byte[] unencryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
        {
            // check the key before doing anything else
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            
            if (key.Length < KeyBytes)
            {
                throw new ArgumentOutOfRangeException("key", "Invalid key length");
            }
            
            if (unencryptedData == null)
            {
                throw new ArgumentNullException("unencryptedData");
            }
            
            byte[] iv = new byte[16];
            
            // Set privacy parameters to the local 64 bit salt value
            byte[] bootsBytes = BitConverter.GetBytes(engineBoots);
            iv[0] = bootsBytes[3];
            iv[1] = bootsBytes[2];
            iv[2] = bootsBytes[1];
            iv[3] = bootsBytes[0];
            byte[] timeBytes = BitConverter.GetBytes(engineTime);
            iv[4] = timeBytes[3];
            iv[5] = timeBytes[2];
            iv[6] = timeBytes[1];
            iv[7] = timeBytes[0];

            // Copy salt value to the iv array
            Array.Copy(privacyParameters, 0, iv, 8, PrivacyParametersLength);

            using (Rijndael rm = new RijndaelManaged())
            {
                rm.KeySize = KeyBytes * 8;
                rm.FeedbackSize = 128;
                rm.BlockSize = 128;
                
                // we have to use Zeros padding otherwise we get encrypt buffer size exception
                rm.Padding = PaddingMode.Zeros;
                rm.Mode = CipherMode.CFB;
                
                // make sure we have the right key length
                byte[] pkey = new byte[MinimumKeyLength];
                Array.Copy(key, pkey, MinimumKeyLength);
                rm.Key = pkey;
                rm.IV = iv;
                using (ICryptoTransform cryptor = rm.CreateEncryptor())
                {
                    byte[] encryptedData = cryptor.TransformFinalBlock(unencryptedData, 0, unencryptedData.Length);
                    
                    // check if encrypted data is the same length as source data
                    if (encryptedData.Length != unencryptedData.Length)
                    {
                        // cut out the padding
                        byte[] tmp = new byte[unencryptedData.Length];
                        Array.Copy(encryptedData, tmp, unencryptedData.Length);
                        return tmp;
                    }
                    
                    return encryptedData;
                }
            }
        }

        /// <summary>
        /// Decrypt DES encrypted ScopedPdu
        /// </summary>
        /// <param name="encryptedData">Source data buffer</param>
        /// <param name="engineBoots">Engine boots.</param>
        /// <param name="engineTime">Engine time.</param>
        /// <param name="key">Decryption key. Key length has to be 32 bytes in length or longer (bytes beyond 32 bytes are ignored).</param>
        /// <param name="privacyParameters">Privacy parameters extracted from USM header</param>
        /// <returns>Decrypted byte array</returns>
        /// <exception cref="ArgumentNullException">Thrown when encrypted data is null or length == 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when encryption key length is less then 32 byte or if privacy parameters
        /// argument is null or length other then 8 bytes</exception>
        private static byte[] Decrypt(byte[] encryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            
            if (encryptedData == null)
            {
                throw new ArgumentNullException("encryptedData");
            }
            
            if (key.Length < KeyBytes)
            {
                throw new ArgumentOutOfRangeException("key", "Invalid key length");
            }

            byte[] iv = new byte[16];
            byte[] bootsBytes = BitConverter.GetBytes(engineBoots);
            iv[0] = bootsBytes[3];
            iv[1] = bootsBytes[2];
            iv[2] = bootsBytes[1];
            iv[3] = bootsBytes[0];
            byte[] timeBytes = BitConverter.GetBytes(engineTime);
            iv[4] = timeBytes[3];
            iv[5] = timeBytes[2];
            iv[6] = timeBytes[1];
            iv[7] = timeBytes[0];

            // Copy salt value to the iv array
            Array.Copy(privacyParameters, 0, iv, 8, PrivacyParametersLength);

            // now do CFB decryption of the encrypted data
            using (Rijndael rm = Rijndael.Create())
            {
                rm.KeySize = KeyBytes * 8;
                rm.FeedbackSize = 128;
                rm.BlockSize = 128;
                rm.Padding = PaddingMode.Zeros;
                rm.Mode = CipherMode.CFB;
                if (key.Length > KeyBytes)
                {
                    byte[] normKey = new byte[KeyBytes];
                    Array.Copy(key, normKey, KeyBytes);
                    rm.Key = normKey;
                }
                else
                {
                    rm.Key = key;
                }
                
                rm.IV = iv;
                using (ICryptoTransform cryptor = rm.CreateDecryptor())
                {
                    // We need to make sure that cryptedData is a collection of 128 byte blocks
                    byte[] decryptedData;
                    if ((encryptedData.Length % KeyBytes) != 0)
                    {
                        byte[] buffer = new byte[encryptedData.Length];
                        Array.Copy(encryptedData, 0, buffer, 0, encryptedData.Length);
                        int div = (int)Math.Floor(buffer.Length / (double)16);
                        int newLength = (div + 1) * 16;
                        byte[] decryptBuffer = new byte[newLength];
                        Array.Copy(buffer, decryptBuffer, buffer.Length);
                        decryptedData = cryptor.TransformFinalBlock(decryptBuffer, 0, decryptBuffer.Length);
                        
                        // now remove padding
                        Array.Copy(decryptedData, buffer, encryptedData.Length);
                        return buffer;
                    }

                    decryptedData = cryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return decryptedData;
                }
            }
        }
        
        /// <summary>
        /// Returns the length of privacyParameters USM header field. For AES, field length is 8.
        /// </summary>
        private static int PrivacyParametersLength
        {
            get { return 8; }
        }

        /// <summary>
        /// Returns minimum encryption/decryption key length. For DES, returned value is 16.
        /// 
        /// DES protocol itself requires an 8 byte key. Additional 8 bytes are used for generating the
        /// encryption IV. For encryption itself, first 8 bytes of the key are used.
        /// </summary>
        private static int MinimumKeyLength
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
                throw new SnmpException("cannot decrypt the scope data");
            }
            
            OctetString octets = (OctetString)data;
            byte[] bytes = octets.GetRaw();
            byte[] pkey = AuthenticationProvider.PasswordToKey(_phrase.GetRaw(), parameters.EngineId.GetRaw());

            // decode encrypted packet
            byte[] decrypted = Decrypt(bytes, pkey, parameters.EngineBoots.ToInt32(), parameters.EngineTime.ToInt32(), parameters.PrivacyParameters.GetRaw());
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
            
            byte[] pkey = AuthenticationProvider.PasswordToKey(_phrase.GetRaw(), parameters.EngineId.GetRaw());
            byte[] bytes = data.ToBytes();
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
            
            byte[] encrypted = Encrypt(bytes, pkey, parameters.EngineBoots.ToInt32(), parameters.EngineTime.ToInt32(), parameters.PrivacyParameters.GetRaw());
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
        
        /// <summary>
        /// Returns a string that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AES privacy provider";
        }
    }
}
