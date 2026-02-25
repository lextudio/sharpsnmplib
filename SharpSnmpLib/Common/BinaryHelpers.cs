using System.Buffers.Binary;

namespace DotNetSnmp.Common.Helpers
{
    /// <summary>
    /// Provides helpers for writing integer values in network byte order (big-endian).
    /// Use these helpers when building SNMP BER payloads and headers.
    /// </summary>
    public static class BinaryHelpers
    {
        /// <summary>
        /// Returns a 4-byte big-endian representation of <paramref name="i"/>.
        /// </summary>
        public static byte[] GetBytesMostSignificantFirst(int i)
        {
            var value = BitConverter.IsLittleEndian
                ? BinaryPrimitives.ReverseEndianness(i)
                : i;

            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns an 8-byte big-endian representation of <paramref name="i"/>.
        /// </summary>
        public static byte[] GetBytesMostSignificantFirst(long i)
        {
            var value = BitConverter.IsLittleEndian
                ? BinaryPrimitives.ReverseEndianness(i)
                : i;

            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Copies <paramref name="i"/> to <paramref name="destination"/> as a big-endian 2-byte value.
        /// </summary>
        /// <remarks>
        /// <paramref name="destination"/> must provide at least 2 bytes.
        /// </remarks>
        public static void CopyBytesMostSignificantFirst(short i, Span<byte> destination)
        {
            if (BitConverter.IsLittleEndian)
            {
                destination[0] = (byte)(i >> 24);
                destination[1] = (byte)(i >> 16);
            }
            else
            {
                destination[0] = (byte)i;
                destination[1] = (byte)(i >> 8);
            }
        }

        /// <summary>
        /// Copies <paramref name="i"/> to <paramref name="destination"/> as a big-endian 4-byte value.
        /// </summary>
        /// <remarks>
        /// <paramref name="destination"/> must provide at least 4 bytes.
        /// </remarks>
        public static void CopyBytesMostSignificantFirst(int i, Span<byte> destination)
        {
            if (BitConverter.IsLittleEndian)
            {
                destination[0] = (byte)(i >> 24);
                destination[1] = (byte)(i >> 16);
                destination[2] = (byte)(i >> 8);
                destination[3] = (byte)i;
            }
            else
            {
                destination[0] = (byte)i;
                destination[1] = (byte)(i >> 8);
                destination[2] = (byte)(i >> 16);
                destination[3] = (byte)(i >> 24);
            }
        }

        /// <summary>
        /// Copies <paramref name="i"/> to <paramref name="destination"/> as a big-endian 8-byte value.
        /// </summary>
        /// <remarks>
        /// <paramref name="destination"/> must provide at least 8 bytes.
        /// </remarks>
        public static void CopyBytesMostSignificantFirst(long i, Span<byte> destination)
        {
            if (BitConverter.IsLittleEndian)
            {
                destination[0] = (byte)(i >> 56);
                destination[1] = (byte)(i >> 48);
                destination[2] = (byte)(i >> 40);
                destination[3] = (byte)(i >> 32);
                destination[4] = (byte)(i >> 24);
                destination[5] = (byte)(i >> 16);
                destination[6] = (byte)(i >> 8);
                destination[7] = (byte)i;
            }
            else
            {
                destination[7] = (byte)(i >> 56);
                destination[6] = (byte)(i >> 48);
                destination[5] = (byte)(i >> 40);
                destination[4] = (byte)(i >> 32);
                destination[3] = (byte)(i >> 24);
                destination[2] = (byte)(i >> 16);
                destination[1] = (byte)(i >> 8);
                destination[0] = (byte)i;
            }
        }
    }
}
