using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides helper methods for StringExtensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets bytes Span Or Default.
        /// </summary>
        public static ReadOnlySpan<byte> GetBytesSpanOrDefault(
            this string s,
            Encoding encoding)
        {
            return string.IsNullOrEmpty(s)
                ? Span<byte>.Empty
                : encoding.GetBytes(s);
        }

        /// <summary>
        /// Gets bytes Memory Or Default.
        /// </summary>
        public static ReadOnlyMemory<byte> GetBytesMemoryOrDefault(
            this string s,
            Encoding encoding)
        {
            return string.IsNullOrEmpty(s)
                ? Memory<byte>.Empty
                : encoding.GetBytes(s).AsMemory();
        }
    }
}
