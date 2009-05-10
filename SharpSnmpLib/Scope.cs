
namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Scope segment.
    /// </summary>
    public class Scope : ISegment
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
        public Scope(OctetString contextEngineId, OctetString contextName, ISnmpPdu pdu)
        {
            _contextEngineId = contextEngineId;
            _contextName = contextName;
            _pdu = pdu;
        }
        
        public Scope(ISnmpPdu pdu)
        {
            _pdu = pdu;
        }

        /// <summary>
        /// Gets the PDU.
        /// </summary>
        /// <value>The PDU.</value>
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

        public OctetString ContextName
        {
            get { return _contextName; }
            set { _contextName = value; }
        }

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
