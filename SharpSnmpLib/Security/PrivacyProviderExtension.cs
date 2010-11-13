/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 10/10/2010
 * Time: 6:59 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Extension class for IPrivacyProvider.
    /// </summary>
    public static class PrivacyProviderExtension
    {
        /// <summary>
        /// Toes the security level.
        /// </summary>
        /// <returns></returns>
        public static Levels ToSecurityLevel(this IPrivacyProvider privacy)
        {
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }
                
            Levels flags;
            if (privacy.AuthenticationProvider == DefaultAuthenticationProvider.Instance)
            {
                flags = 0;
            }
            else if (privacy is DefaultPrivacyProvider)
            {
                flags = Levels.Authentication;
            }
            else
            {
                flags = Levels.Authentication | Levels.Privacy;
            }

            return flags;
        }
    }
}
