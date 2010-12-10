// Composed membership provider class.
// Copyright (C) 2009-2010 Lex Li
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

using System.Linq;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Composed membership provider, who owns internal providers. If the request is authenticated by any of the internal providers, it is considered as authenticated.
    /// </summary>    
    public sealed class ComposedMembershipProvider : IMembershipProvider
    {
        private readonly IMembershipProvider[] _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComposedMembershipProvider"/> class.
        /// </summary>
        /// <param name="providers">The internal providers.</param>
        public ComposedMembershipProvider(IMembershipProvider[] providers)
        {
            _providers = providers;
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(ISnmpContext context)
        {
            return _providers.Any(provider => provider.AuthenticateRequest(context));
        }
    }
}
