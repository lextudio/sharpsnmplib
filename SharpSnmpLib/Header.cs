// Header type.
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
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Header segment.
    /// </summary>
    public sealed class Header : ISegment
    {
        private readonly Integer32 _messageId;
        private readonly Integer32 _maxSize;
        private readonly OctetString _flags;
        private readonly Integer32 _securityModel;
        private static readonly Integer32 DefaultSecurityModel = new Integer32(3);
        private static readonly Integer32 DefaultMaxMessageSize = new Integer32(MaxMessageSize);
        private static readonly Header EmptyHeader = new Header();
        private readonly Sequence _container;

        /// <summary>
        /// Max message size used in #SNMP. 
        /// </summary>
        /// <remarks>
        /// You can use any value for your own application. 
        /// Also this value may be changed in #SNMP in future releases.
        /// </remarks>
        public const int MaxMessageSize = 0xFFE3;

        private Header() : this(null, DefaultMaxMessageSize, 0)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        public Header(int messageId) : this(new Integer32(messageId), DefaultMaxMessageSize, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Header(ISnmpData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _container = (Sequence)data;
            _messageId = (Integer32)_container[0];
            _maxSize = (Integer32)_container[1];
            _flags = (OctetString)_container[2];
            SecurityLevel = _flags.ToLevels();
            _securityModel = (Integer32)_container[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="securityLevel">The security level.</param>
        /// <remarks>If you want an empty header, please use <see cref="Empty"/>.</remarks>
        public Header(Integer32 messageId, Integer32 maxMessageSize, Levels securityLevel)
        {
            if (maxMessageSize == null)
            {
                throw new ArgumentNullException(nameof(maxMessageSize));
            }

            _messageId = messageId;
            _maxSize = maxMessageSize;
            SecurityLevel = securityLevel;
            _flags = new OctetString(SecurityLevel);
            _securityModel = DefaultSecurityModel;
        }
        
        /// <summary>
        /// Empty header.
        /// </summary>
        public static Header Empty
        {
            get { return EmptyHeader; }
        }        
        
        /// <summary>
        /// Security flags.
        /// </summary>
        public Levels SecurityLevel { get; private set; }

        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        public int MessageId
        {
            get { return _messageId.ToInt32(); }
        }

        #region ISegment Members

        /// <summary>
        /// Converts to <see cref="Sequence"/> object.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return _container ?? new Sequence(null, _messageId, _maxSize, _messageId == null ? null : _flags, _securityModel);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ISnmpData GetData(VersionCode version)
        {
            return version == VersionCode.V3 ? ToSequence() : null;
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Header: messageId: {0};maxMessageSize: {1};securityBits: 0x{2};securityModel: {3}", MessageId, _maxSize, _flags.ToHexString(), _securityModel);
        }

        /// <summary>
        /// Gets or sets the size of the max.
        /// </summary>
        /// <value>The size of the max.</value>
        public int MaxSize
        {
            get { return _maxSize.ToInt32(); }
        }
    }
}
