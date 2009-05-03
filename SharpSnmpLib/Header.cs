using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Header segment.
    /// </summary>
    public class Header : ISegment
    {
        private Integer32 _messageId;
        private Integer32 _maxMessageSize;
        private OctetString _flags;
        private Integer32 _securityModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Header(Sequence data)
        {
            _messageId = (Integer32) data[0];
            _maxMessageSize = (Integer32) data[1];
            _flags = (OctetString) data[2];
            _securityModel = (Integer32) data[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="messageId">The message id.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="securityModel">The security model.</param>
        public Header(Integer32 messageId, Integer32 maxMessageSize, OctetString flags, Integer32 securityModel)
        {
            _messageId = messageId;
            _maxMessageSize = maxMessageSize;
            _flags = flags;
            _securityModel = securityModel;
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
            return new Sequence(_messageId, _maxMessageSize, _flags, _securityModel);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ISnmpData GetData(VersionCode version)
        {
            if (version == VersionCode.V3)
            {
                return ToSequence();
            }

            return null;
        }

        #endregion
    }
}
