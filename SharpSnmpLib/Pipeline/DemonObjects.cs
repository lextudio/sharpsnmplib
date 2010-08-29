using System;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Demon objects.
    /// </summary>
    public class DemonObjects
    {
        // TODO: make engine ID configurable from outside and unique.
        private readonly OctetString _engineId = new OctetString(new byte[] { 4, 13, 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244, 73 });

        /// <summary>
        /// Initializes a new instance of the <see cref="DemonObjects"/> class.
        /// </summary>
        public DemonObjects()
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