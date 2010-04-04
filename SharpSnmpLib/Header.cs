// Header type.
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
        private static readonly Header EmptyHeader = new Header();
        
        private Header()
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
                throw new ArgumentNullException("data");
            }

            Sequence container = (Sequence)data;
            _messageId = (Integer32) container[0];
            _maxSize = (Integer32) container[1];
            _flags = (OctetString) container[2];
            _securityModel = (Integer32) container[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="securityBits">The flags.</param>
        /// <param name="securityModel">The security model.</param>
        /// <remarks>If you want an empty header, please use <see cref="Empty"/>.</remarks>
        public Header(Integer32 messageId, Integer32 maxMessageSize, OctetString securityBits, Integer32 securityModel)
        {
            if (messageId == null)
            {
                throw new ArgumentNullException("messageId");
            }

            if (maxMessageSize == null)
            {
                throw new ArgumentNullException("maxMessageSize");
            }

            if (securityBits == null)
            {
                throw new ArgumentNullException("securityBits");
            }

            if (securityModel == null)
            {
                throw new ArgumentNullException("securityModel");
            }

            _messageId = messageId;
            _maxSize = maxMessageSize;
            _flags = securityBits;
            _securityModel = securityModel;
        }
        
        /// <summary>
        /// Empty header.
        /// </summary>
        public static Header Empty
        {
            get { return EmptyHeader; }
        }

        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        public int MessageId
        {
            get
            {
                return _messageId.ToInt32();
            }
        }

        #region ISegment Members

        /// <summary>
        /// Converts to <see cref="Sequence"/> object.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return new Sequence(_messageId, _maxSize, _flags, _securityModel);
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
    }
}
