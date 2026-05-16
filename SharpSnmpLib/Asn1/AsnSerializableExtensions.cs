using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides helper methods for AsnSerializableExtensions.
    /// </summary>
    public static class AsnSerializableExtensions
    {
        /// <summary>
        /// Encodes the value to BER bytes.
        /// </summary>
        public static byte[] Encode(this ISnmpData obj)
        {
            var writer = new AsnWriter(AsnEncodingRules.BER);
            obj.WriteTo(writer);
            return writer.Encode();
        }

        /// <summary>
        /// Encodes the value to BER bytes.
        /// </summary>
        public static int Encode(this ISnmpData obj, Span<byte> destination)
        {
            var writer = new AsnWriter(AsnEncodingRules.BER);
            obj.WriteTo(writer);
            return writer.Encode(destination);
        }
    }
}
