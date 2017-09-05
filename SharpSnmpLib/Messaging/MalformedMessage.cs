// Malformed message type.
// Copyright (C) 2010 Lex Li.
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
using System.Globalization;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Malformed message for v3 due to decryption failures or wrong user names. 
    /// </summary>
    public sealed class MalformedMessage : ISnmpMessage
    {
        private static readonly Scope DefaultScope = new Scope(new MalformedPdu());
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedMessage"/> class.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="user">The user.</param>
        [Obsolete("Please use the new overloading constructor.")]
        public MalformedMessage(int messageId, OctetString user)
            : this(messageId, user, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedMessage"/> class.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="user">The user.</param>
        /// <param name="data">The data encrypted.</param>
        public MalformedMessage(int messageId, OctetString user, ISnmpData data)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Header = new Header(messageId);
            Parameters = SecurityParameters.Create(user);
            Scope = DefaultScope;
            EncryptedScope = data;
        }

        /// <summary>
        /// Encrypted scope data.
        /// </summary>
        /// <value>The original scope data from packet, which remains encrypted.</value>
        public ISnmpData EncryptedScope { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public SecurityParameters Parameters { get; private set; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The fake scope, which is used to avoid exceptions in message handling.</value>
        public Scope Scope
        {
            get; private set;
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        public Header Header { get; private set; }

        /// <summary>
        /// Converts to the bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return new byte[0];
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version
        {
            get { return VersionCode.V3; }
        }

        /// <summary>
        /// Gets the privacy provider.
        /// </summary>
        /// <value>The privacy provider.</value>
        public IPrivacyProvider Privacy
        {
            get { return null; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Malformed message: message id: {0}; user: {1}", this.MessageId(), Parameters.UserName);
        }
    }
}