using System;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class AgentObjects
    {
        // TODO: make engine ID configurable from outside and unique.
        private readonly OctetString _engineId = new OctetString(new byte[] { 4, 13, 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244, 73 });
        public uint ReportCount;
        private const int engineBoots = 0;

        internal OctetString EngineId
        {
            get { return _engineId; }
        }

        internal int EngineBoots
        {
            get { return engineBoots; }
        }

        public int EngineTime
        {
            get { return Environment.TickCount; }
        }
    }
}