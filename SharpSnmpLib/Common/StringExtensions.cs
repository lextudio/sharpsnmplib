using System.Text;

namespace DotNetSnmp
{
    public static class StringExtensions
    {
        public static ReadOnlySpan<byte> GetBytesSpanOrDefault(
            this string s,
            Encoding encoding)
        {
            return string.IsNullOrEmpty(s)
                ? Span<byte>.Empty
                : encoding.GetBytes(s);
        }

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
