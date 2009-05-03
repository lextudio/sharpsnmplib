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
        private OctetString _source;

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>The source.</value>
        public OctetString Source
        {
            get { return _source; }
        }
        
        private Integer32 _reboot;

        /// <summary>
        /// Gets the reboot count.
        /// </summary>
        /// <value>The reboot count.</value>
        public Integer32 Reboot 
        {
            get { return _reboot; }
        }
        
        private Integer32 _ticks;

        /// <summary>
        /// Gets the ticks.
        /// </summary>
        /// <value>The ticks.</value>
        public Integer32 Ticks
        {
            get { return _ticks; }
        }
        
        private OctetString _user;

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        public OctetString User 
        {
            get { return _user; }
        }
        
        private OctetString _string1;

        /// <summary>
        /// Gets the string1.
        /// </summary>
        /// <value>The string1.</value>
        public OctetString String1
        {
            get { return _string1; }
        }
        
        private OctetString _string2;

        /// <summary>
        /// Gets the string2.
        /// </summary>
        /// <value>The string2.</value>
        public OctetString String2
        {
            get { return _string2; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityParameters"/> class.
        /// </summary>
        /// <param name="parameters">The <see cref="Sequence"/> that contains parameters.</param>
        public SecurityParameters(Sequence parameters)
        {
            _source = (OctetString)parameters[0];
            _reboot = (Integer32)parameters[1];
            _ticks = (Integer32)parameters[2];
            _user = (OctetString)parameters[3];
            _string1 = (OctetString)parameters[4];
            _string2 = (OctetString)parameters[5];
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
            _source = source;
            _reboot = reboot;
            _ticks = ticks;
            _user = user;
            _string1 = string1;
            _string2 = string2;
        }

        /// <summary>
        /// Converts to <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return new Sequence(_source, _reboot, _ticks, _user, _string1, _string2);
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

            return _user;
        }

        #endregion
    }
}
