using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Header segment.
    /// </summary>
    public class Header : ISegment
    {
        private Integer32 _messageId;
        private Integer32 _maxSize;
        private OctetString _flags;
        private Integer32 _securityModel;
        private static Header empty = new Header();
        
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
        public Header(Integer32 messageId, Integer32 maxMessageSize, OctetString securityBits, Integer32 securityModel)
        {
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
            get { return empty; }
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
            if (version == VersionCode.V3)
            {
                return ToSequence();
            }

            return null;
        }

        #endregion
    }
}
