// IAuthenticationProvider extension class.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
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

using System;
using System.IO;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider extension.
    /// </summary>
    public static class AuthenticationProviderExtension
    {
        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="authen">The authentication provider.</param>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="privacy">The privacy provider.</param>
        public static void ComputeHash(this IAuthenticationProvider authen, VersionCode version, Header header, SecurityParameters parameters, ISegment scope, IPrivacyProvider privacy)
        {
            if (authen == null)
            {
                throw new ArgumentNullException("authen");
            }
            
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }

            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            if (authen is DefaultAuthenticationProvider)
            {
                return;
            }

            if (0 == (header.SecurityLevel & Levels.Authentication))
            {
                return;
            }

            var scopeData = privacy.GetScopeData(header, parameters, scope.GetData(version));
            
            // replace the hash.
            parameters.AuthenticationParameters = authen.ComputeHash(version, header, parameters, scopeData, privacy);
        }

        /// <summary>
        /// Verifies the hash.
        /// </summary>
        public static bool VerifyHash(this IAuthenticationProvider authen, Stream originalStream, OctetString expected, OctetString engineId)
        {
            if (authen == null)
            {
                throw new ArgumentNullException("authen");
            }
            
            if (originalStream == null)
            {
                throw new ArgumentNullException("originalStream");
            }

            if (expected == null)
            {
                throw new ArgumentNullException("expected");
            }

            if (engineId == null)
            {
                throw new ArgumentNullException("expected");
            }

            if (authen is DefaultAuthenticationProvider)
            {
                return true;
            }

            originalStream.Position = 0; // Seek to the beginning of the stream
            
            byte[] bytes;
            using (Stream stream = new MemoryStream())
            {
                originalStream.CopyTo(stream);                
                CleanAuthenticationParameters(stream, authen.CleanDigest.GetRaw()); // Replace with clean digest
                bytes = new byte[stream.Length]; // Read stream into byte array
                stream.Position = 0;
                stream.Read(bytes, 0, bytes.Length);
            }

            // Compute hash
            return authen.ComputeHash(bytes, engineId) == expected;
        }

        /// <summary>
        /// Cleans the Authentication parameters in the supplied <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="System.IO.Stream"/> to be parsed.</param>
        /// <param name="cleanDigest">The clean digest.</param>
        /// <returns></returns>
        private static void CleanAuthenticationParameters(Stream stream, byte[] cleanDigest)
        {
            try
            {
                if (!stream.CanWrite)
                {
                    throw new NotSupportedException("stream not writable");
                }

                stream.Position = 0;

                // We look for an exact payload which is at a known position
                stream.IgnorePayloadStart(); // Enter the outer payload
                stream.IgnorePayloads(2); // Skip 2 payloads
                stream.IgnorePayloadStart();
                stream.IgnorePayloadStart();
                stream.IgnorePayloads(4);

                if ((SnmpType)stream.ReadByte() == SnmpType.OctetString)
                {
                    stream.ReadPayloadLength();
                    stream.Write(cleanDigest, 0, cleanDigest.Length);
                }
                else
                {
                    throw new OperationException("expected OctetString when finding Authentication Parameters");
                }
            }
            catch (Exception e)
            {
                throw new OperationException("could not clean Authentication Parameters", e);
            }
        }
    }
}
