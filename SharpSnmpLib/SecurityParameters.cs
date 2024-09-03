// Security parameters type.
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
    public sealed class SecurityParameters : ISegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        private SecurityParameters()
        {

        }

        /// <summary>
        /// Initializes an empty instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        public static SecurityParameters Empty => new();

        /// <summary>
        /// Gets the engine ID.
        /// </summary>
        /// <value>The engine ID.</value>
        public OctetString? EngineId { get; }

        /// <summary>
        /// Gets the boot count.
        /// </summary>
        /// <value>The boot count.</value>
        public Integer32? EngineBoots { get; }

        /// <summary>
        /// Gets the engine time.
        /// </summary>
        /// <value>The engine time.</value>
        public Integer32? EngineTime { get; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        /// <value>The user name.</value>
        public OctetString? UserName { get; }

        private OctetString? _authenticationParameters;
        private readonly byte[]? _length;

        /// <summary>
        /// Gets the authentication parameters.
        /// </summary>
        /// <value>The authentication parameters.</value>
        public OctetString? AuthenticationParameters
        {
            get => _authenticationParameters;

            set
            {
                if (_authenticationParameters == null || value == null)
                {
                    _authenticationParameters = value;
                    return;
                }

                if (value.GetRaw().Length != _authenticationParameters.GetRaw().Length)
                {
                    throw new ArgumentException(
                        $"Length of new authentication parameters is invalid: {value.GetRaw().Length} found while {_authenticationParameters.GetRaw().Length} expected.",
                        nameof(value));
                }

                if (value.GetLengthBytes() != _authenticationParameters.GetLengthBytes())
                {
                    value.SetLengthBytes(_authenticationParameters.GetLengthBytes());
                }

                _authenticationParameters = value;
            }
        }

        /// <summary>
        /// Gets the privacy parameters.
        /// </summary>
        /// <value>The privacy parameters.</value>
        public OctetString? PrivacyParameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        /// <param name="parameters">The <see cref="OctetString"/> that contains parameters.</param>
        public SecurityParameters(OctetString parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters == OctetString.Empty)
            {
                return;
            }

            var container = (Sequence)DataFactory.CreateSnmpData(parameters.GetRaw());
            EngineId = (OctetString)container[0];
            EngineBoots = (Integer32)container[1];
            EngineTime = (Integer32)container[2];
            UserName = (OctetString)container[3];
            AuthenticationParameters = (OctetString)container[4];
            PrivacyParameters = (OctetString)container[5];
            _length = container.GetLengthBytes();
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
        public SecurityParameters(OctetString? engineId, Integer32? engineBoots, Integer32? engineTime, OctetString userName, OctetString? authenticationParameters, OctetString? privacyParameters)
        {
            EngineId = engineId;
            EngineBoots = engineBoots;
            EngineTime = engineTime;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            AuthenticationParameters = authenticationParameters;
            PrivacyParameters = privacyParameters;
        }

        /// <summary>
        /// Creates an instance of <see cref="SecurityParameters"/>.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns></returns>
        public static SecurityParameters Create(OctetString userName)
        {
            if (userName is null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            return new SecurityParameters(null, null, null, userName, null, null);
        }

        /// <summary>
        /// Converts to <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return new Sequence(_length, EngineId, EngineBoots, EngineTime, UserName, AuthenticationParameters, PrivacyParameters);
        }

        #region ISegment Members

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ISnmpData GetData(VersionCode version)
        {
            //if empty SecurityParameters, return an empty OctetString
            if (_length == null && EngineId == null && EngineBoots == null && EngineTime == null && UserName == null && PrivacyParameters == null
                && (AuthenticationParameters == OctetString.Empty || AuthenticationParameters == null))
            {
                return OctetString.Empty;
            }

            return version == VersionCode.V3 ? new OctetString(ToSequence().ToBytes()) : (UserName ?? OctetString.Empty);
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
            return string.Format(CultureInfo.InvariantCulture, "Security parameters: engineId: {0};engineBoots: {1};engineTime: {2};userName: {3}; authen hash: {4}; privacy hash: {5}", EngineId, EngineBoots, EngineTime, UserName, AuthenticationParameters?.ToHexString(), PrivacyParameters?.ToHexString());
        }

        /// <summary>
        /// Gets a value that indicates whether the hashes are invalid.
        /// </summary>
        public bool IsInvalid { get; set; }
    }
}
