// AES privacy provider
// Copyright (C) 2009-2010 Lex Li, Milan Sinadinovic
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/30/2009
 * Time: 8:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider for AES 256.
    /// </summary>
    /// <remarks>
    /// This is an experimental port from SNMP#NET project. As AES is not part of SNMP RFC, this class is provided as it is.
    /// If you want other AES providers, you can port them from SNMP#NET in a similar manner.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "AES", Justification = "definition")]
    public sealed class AES256PrivacyProvider : AESPrivacyProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AES256PrivacyProvider"/> class.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="auth">The authentication provider.</param>
        public AES256PrivacyProvider(OctetString phrase, IAuthenticationProvider auth)
            : base(32, phrase, auth)
        { }

        /// <summary>
        /// Returns a string that represents this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AES 256 privacy provider";
        }
    }
}
