// Security parameters type.
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
using System.Globalization;

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
        /// <summary>
        /// Gets the engine ID.
        /// </summary>
        /// <value>The engine ID.</value>
        public OctetString EngineId { get; private set; }

        /// <summary>
        /// Gets the boot count.
        /// </summary>
        /// <value>The boot count.</value>
        public Integer32 EngineBoots { get; private set; }

        /// <summary>
        /// Gets the engine time.
        /// </summary>
        /// <value>The engine time.</value>
        public Integer32 EngineTime { get; private set; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        /// <value>The user name.</value>
        public OctetString UserName { get; private set; }

        /// <summary>
        /// Gets the authentication parameters.
        /// </summary>
        /// <value>The authentication parameters.</value>
        public OctetString AuthenticationParameters { get; set; }

        /// <summary>
        /// Gets the privacy parameters.
        /// </summary>
        /// <value>The privacy parameters.</value>
        public OctetString PrivacyParameters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        /// <param name="parameters">The <see cref="OctetString"/> that contains parameters.</param>
        public SecurityParameters(OctetString parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            Sequence data = (Sequence)DataFactory.CreateSnmpData(parameters.GetRaw());
            EngineId = (OctetString)data[0];
            EngineBoots = (Integer32)data[1];
            EngineTime = (Integer32)data[2];
            UserName = (OctetString)data[3];
            AuthenticationParameters = (OctetString)data[4];
            PrivacyParameters = (OctetString)data[5];
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
        /// <remarks>Only <paramref name="userName"/> cannot be null.</remarks>
        public SecurityParameters(OctetString engineId, Integer32 engineBoots, Integer32 engineTime, OctetString userName, OctetString authenticationParameters, OctetString privacyParameters)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            EngineId = engineId;
            EngineBoots = engineBoots;
            EngineTime = engineTime;
            UserName = userName;
            AuthenticationParameters = authenticationParameters;
            PrivacyParameters = privacyParameters;
        }

        /// <summary>
        /// Converts to <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return new Sequence(EngineId, EngineBoots, EngineTime, UserName, AuthenticationParameters, PrivacyParameters);
        }

        #region ISegment Members

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ISnmpData GetData(VersionCode version)
        {
            return version == VersionCode.V3 ? new OctetString(ToSequence().ToBytes()) : UserName;
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
            return string.Format(CultureInfo.InvariantCulture, "Security parameters: engineId: {0};engineBoots: {1};engineTime: {2};userName: {3}; authen hash: {4}; privacy hash: {5}", EngineId, EngineBoots, EngineTime, UserName, AuthenticationParameters.ToHexString(), PrivacyParameters.ToHexString());
        }
    }
}
