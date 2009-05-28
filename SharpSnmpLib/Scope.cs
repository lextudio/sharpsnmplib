using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Scope segment.
    /// </summary>
    public sealed class Scope : ISegment
    {
        private ISnmpPdu _pdu;
        private OctetString _contextEngineId;
        private OctetString _contextName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Scope(Sequence data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            
            _contextEngineId = (OctetString)data[0];
            _contextName = (OctetString)data[1];
            _pdu = (ISnmpPdu)data[2];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="contextEngineId">The context engine ID.</param>
        /// <param name="contextName">Name of the context.</param>
        /// <param name="pdu">The PDU.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pdu", Justification = "definition")]
        public Scope(OctetString contextEngineId, OctetString contextName, ISnmpPdu pdu)
        {
            _contextEngineId = contextEngineId;
            _contextName = contextName;
            _pdu = pdu;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="pdu">The pdu.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pdu", Justification = "definition")]
        public Scope(ISnmpPdu pdu)
        {
            _pdu = pdu;
        }

        /// <summary>
        /// Gets the PDU.
        /// </summary>
        /// <value>The PDU.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu", Justification = "definition")]
        public ISnmpPdu Pdu
        {
            get { return _pdu; }
        }

        #region ISegment Members

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

            return Pdu;
        }

        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        /// <value>The name of the context.</value>
        public OctetString ContextName
        {
            get { return _contextName; }
            set { _contextName = value; }
        }

        /// <summary>
        /// Gets or sets the context engine id.
        /// </summary>
        /// <value>The context engine id.</value>
        public OctetString ContextEngineId
        {
            get { return _contextEngineId; }
            set { _contextEngineId = value; }
        }
        
        /// <summary>
        /// Converts to <see cref="Sequence"/> object.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return new Sequence(_contextEngineId, _contextName, _pdu);
        }

        #endregion
    }
}
