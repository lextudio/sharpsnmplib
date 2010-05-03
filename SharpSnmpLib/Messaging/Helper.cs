// Helper class.
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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Authenticates this message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="providers">The providers.</param>
        public static void Authenticate(ISnmpMessage message, ProviderPair providers)
        {
            // TODO: make extension method.
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            if (providers == null)
            {
                throw new ArgumentNullException("providers");
            }
            
            message.Parameters.AuthenticationParameters = providers.Authentication.ComputeHash(message);
        }		
			
        /// <summary>
        /// Tests if runnning on Mono. 
        /// </summary>
        /// <returns></returns>
		public static bool IsRunningOnMono()
  		{
    		return Type.GetType ("Mono.Runtime") != null;
  		}

        internal static Sequence PackMessage(VersionCode version, IPrivacyProvider privacy, ISegment header, SecurityParameters parameters, ISegment scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }
            
            List<ISnmpData> collection = new List<ISnmpData>(4)
                                             {
                                                 new Integer32((int) version),
                                                 header.GetData(version),
                                                 parameters.GetData(version),
                                                 privacy.Encrypt(scope.GetData(version), parameters)
                                             };
            return new Sequence(collection);
        }

        /// <summary>
        /// Packs the message.
        /// </summary>
        /// <param name="version">The protocol version.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Sequence PackMessage(VersionCode version, params ISnmpData[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            
            List<ISnmpData> collection = new List<ISnmpData>(1 + data.Length) {new Integer32((int) version)};
            collection.AddRange(data);
            return new Sequence(collection);
        }
        
        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public static Socket GetSocket(EndPoint endpoint)
        {
            // TODO: make extension method.
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            return new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        }        
    }
}
