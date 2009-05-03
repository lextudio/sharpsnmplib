/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/3/2009
 * Time: 1:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

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
            get { return _engineId; }
        }
        
        private Integer32 _engineBoots;

        /// <summary>
        /// Gets the boot count.
        /// </summary>
        /// <value>The boot count.</value>
        public Integer32 EngineBoots 
        {
            get { return _engineBoots; }
        }
        
        private Integer32 _engineTime;

        /// <summary>
        /// Gets the engine time.
        /// </summary>
        /// <value>The engine time.</value>
        public Integer32 EngineTime
        {
            get { return _engineTime; }
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
            get { return _authenticationParameters; }
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
        /// <param name="parameters">The <see cref="Sequence"/> that contains parameters.</param>
        public SecurityParameters(Sequence parameters)
        {
            _engineId = (OctetString)parameters[0];
            _engineBoots = (Integer32)parameters[1];
            _engineTime = (Integer32)parameters[2];
            _userName = (OctetString)parameters[3];
            _authenticationParameters = (OctetString)parameters[4];
            _privacyParameters = (OctetString)parameters[5];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="reboot">The reboot.</param>
        /// <param name="ticks">The ticks.</param>
        /// <param name="user">The user.</param>
        /// <param name="string1">The string1.</param>
        /// <param name="string2">The string2.</param>
        public SecurityParameters(OctetString source, Integer32 reboot, Integer32 ticks, OctetString user, OctetString string1, OctetString string2)
        {
            _engineId = source;
            _engineBoots = reboot;
            _engineTime = ticks;
            _userName = user;
            _authenticationParameters = string1;
            _privacyParameters = string2;
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
                return ToSequence();
            }

            return _userName;
        }

        #endregion
    }
}
