// Engine group class.
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

using System;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Engine group which contains all related objects.
    /// </summary>
    public sealed class EngineGroup
    {
        // TODO: make engine ID configurable from outside and unique.
        private readonly OctetString _engineId =
            new OctetString(new byte[] { 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244 });

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineGroup"/> class.
        /// </summary>
        public EngineGroup()
        {
            EngineBoots = 0;
        }
        
        /// <summary>
        /// Count of handled REPORT messages.
        /// </summary>
        [CLSCompliant(false)]
        public uint ReportCount { get; set; }

        /// <summary>
        /// Gets the engine id.
        /// </summary>
        /// <value>The engine id.</value>
        internal OctetString EngineId
        {
            get { return _engineId; }
        }

        /// <summary>
        /// Gets or sets the engine boots.
        /// </summary>
        /// <value>The engine boots.</value>
        internal int EngineBoots { get; set; }

        /// <summary>
        /// Gets the engine time.
        /// </summary>
        /// <value>The engine time.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int EngineTime
        {
            get { return Environment.TickCount; }
        }
    }
}