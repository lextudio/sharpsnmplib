using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V3.Security
{
    public class UsmSecurityParameters : IAsnSerializable
    {
        public Memory<byte> EngineId { get; set; }

        /// <summary>
        /// count of the number of times the
        /// SNMP engine has re-booted/re-initialized since snmpEngineID
        /// was last configured
        /// </summary>
        public int EngineBoots { get; set; }

        /// <summary>
        /// the number of seconds since the
        /// snmpEngineBoots counter was last incremented
        /// </summary>
        public int EngineTime { get; set; }

        public OctetString SecurityName { get; set; }

        public Memory<byte> AuthParams { get; set; } = Memory<byte>.Empty;

        public Memory<byte> PrivParams { get; set; } = Memory<byte>.Empty;

        public void WriteTo(AsnWriter writer)
        {
            var innerSeq = new AsnWriter(AsnEncodingRules.BER);

            using (_ = innerSeq.PushSequence())
            {
                innerSeq.WriteOctetString(EngineId.Span);

                innerSeq.WriteInteger(EngineBoots);

                innerSeq.WriteInteger(EngineTime);

                innerSeq.WriteOctetString(SecurityName.Octets);

                // Don't initialize AuthParams here - this should be handled by the authentication service
                // before message encoding to ensure consistent digest calculation
                innerSeq.WriteOctetString(AuthParams.Span);

                innerSeq.WriteOctetString(PrivParams.Span);
            }

            var innerSequence = innerSeq.Encode();

            writer.WriteOctetString(innerSequence);
        }

        public static UsmSecurityParameters ReadFrom(AsnReader reader)
        {
            var innerSeqBytes = reader.ReadOctetString();

            var innerSeqReader = new AsnReader(innerSeqBytes, AsnEncodingRules.BER);

            var innerSeq = innerSeqReader.ReadSequence();

            var engineId = innerSeq.ReadOctetString();

            innerSeq.TryReadInt32(out var engineBoots);

            innerSeq.TryReadInt32(out var engineTime);

            var userName = innerSeq.ReadOctetString();

            var authParams = innerSeq.ReadOctetString();

            var privParams = innerSeq.ReadOctetString();

            return new()
            {
                EngineId = engineId,
                EngineBoots = engineBoots,
                EngineTime = engineTime,
                SecurityName = new OctetString(userName),
                AuthParams = authParams,
                PrivParams = privParams
            };
        }
    }
}
