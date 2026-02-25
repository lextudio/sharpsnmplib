using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security;
using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V3
{
    /// <summary>
    /// Represents the HeaderData type.
    /// </summary>
    public class HeaderData : IAsnSerializable
    {
        /// <summary>
        /// Gets msg Id.
        /// </summary>
        public int MsgId { get; set; }

        /// <summary>
        /// Gets msg Max Size.
        /// </summary>
        [Range(384, int.MaxValue)]
        public int MsgMaxSize { get; set; } = 65536;

        /// <summary>
        /// Gets msg Flags.
        /// </summary>
        public MsgFlags MsgFlags { get; set; }

        /// <summary>
        /// Gets msg Security Model.
        /// </summary>
        [Range(1, int.MaxValue)]
        public SecurityModel MsgSecurityModel { get; set; } = SecurityModel.Usm;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence())
            {
                writer.WriteInteger(MsgId);

                writer.WriteInteger(MsgMaxSize);

                Span<byte> flags = stackalloc byte[1] { (byte)MsgFlags };

                writer.WriteOctetString(flags);

                writer.WriteInteger((byte)MsgSecurityModel);
            }
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static HeaderData ReadFrom(AsnReader reader)
        {
            var rootSeq = reader.ReadSequence();

            rootSeq.TryReadInt32(out var msgId);

            rootSeq.TryReadInt32(out var msgMaxSize);

            var flags = (MsgFlags)rootSeq.ReadOctetString()[0];

            rootSeq.TryReadInt32(out var msgSecurityModel);

            return new HeaderData
            {
                MsgId = msgId,
                MsgMaxSize = msgMaxSize,
                MsgFlags = flags,
                MsgSecurityModel = (SecurityModel)msgSecurityModel
            };
        }
    }
}
