// Message handler factory class.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Linq;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Message handler factory.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class MessageHandlerFactory
    {
        private readonly HandlerMapping[] _mappings;
        private readonly NullMessageHandler _nullHandler = new NullMessageHandler();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerFactory"/> class.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        public MessageHandlerFactory(HandlerMapping[] mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }

            _mappings = mappings;
        }

        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public IMessageHandler GetHandler(ISnmpMessage message)
        {
            foreach (var mapping in _mappings.Where(mapping => mapping.CanHandle(message)))
            {
                return mapping.Handler;
            }

            return _nullHandler;
        }
    }
}
