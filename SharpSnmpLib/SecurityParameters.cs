/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/3/2009
 * Time: 1:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Description of SecurityParameters.
    /// </summary>
    public class SecurityParameters : ISegment
    {
        private OctetString _engineId;

        /// <summary>
        /// Gets the engine ID.
        /// </summary>
        /// <value>The engine ID.</value>
        public OctetString EngineId
        {
            get
            {
                return _engineId;
            }
            set
            {
                _engineId = value;
            }
        }

        private Integer32 _engineBoots;

        /// <summary>
        /// Gets the boot count.
        /// </summary>
        /// <value>The boot count.</value>
        public Integer32 EngineBoots
        {
            get
            {
                return _engineBoots;
            }
            set
            {
                _engineBoots = value;
            }
        }

        private Integer32 _engineTime;

        /// <summary>
        /// Gets the engine time.
        /// </summary>
        /// <value>The engine time.</value>
        public Integer32 EngineTime
        {
            get
            {
                return _engineTime;
            }
            set
            {
                _engineTime = value;
            }
        }

        private OctetString _userName;

        /// <summary>
        /// Gets the user name.
        /// </summary>
        /// <value>The user name.</value>
        public OctetString UserName
        {
            get { return _userName; }
        }

        private OctetString _authenticationParameters;

        /// <summary>
        /// Gets the authentication parameters.
        /// </summary>
        /// <value>The authentication parameters.</value>
        public OctetString AuthenticationParameters
        {
            get
            {
                return _authenticationParameters;
            }
            set
            {
                _authenticationParameters = value;
            }
        }

        private OctetString _privacyParameters;

        /// <summary>
        /// Gets the privacy parameters.
        /// </summary>
        /// <value>The privacy parameters.</value>
        public OctetString PrivacyParameters
        {
            get { return _privacyParameters; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        /// <param name="parameters">The <see cref="OctetString"/> that contains parameters.</param>
        public SecurityParameters(OctetString parameters)
        {
            Sequence data = (Sequence)DataFactory.CreateSnmpData(parameters.GetRaw());
            _engineId = (OctetString)data[0];
            _engineBoots = (Integer32)data[1];
            _engineTime = (Integer32)data[2];
            _userName = (OctetString)data[3];
            _authenticationParameters = (OctetString)data[4];
            _privacyParameters = (OctetString)data[5];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        /// <param name="engineId">The engine ID.</param>
        /// <param name="engineBoots">The engine boots.</param>
        /// <param name="engineTime">The engine time.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="authenticationParameters">The authentication parameters.</param>
        /// <param name="privacyParameters">The privacy parameters.</param>
        public SecurityParameters(OctetString engineId, Integer32 engineBoots, Integer32 engineTime, OctetString userName, OctetString authenticationParameters, OctetString privacyParameters)
        {
            _engineId = engineId;
            _engineBoots = engineBoots;
            _engineTime = engineTime;
            _userName = userName;
            _authenticationParameters = authenticationParameters;
            _privacyParameters = privacyParameters;
        }

        /// <summary>
        /// Converts to <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return new Sequence(_engineId, _engineBoots, _engineTime, _userName, _authenticationParameters, _privacyParameters);
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
                return new OctetString(ToSequence().ToBytes());
            }

            return _userName;
        }

        #endregion

        internal SecurityParameters Clone()
        {
            return new SecurityParameters(
                _engineId,
                _engineBoots,
                _engineTime,
                _userName,
                new OctetString(new byte[12]),
                _privacyParameters);
        }
    }
}
