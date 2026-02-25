using System.Formats.Asn1;

namespace DotNetSnmp.Asn1
{
    /// <summary>
    /// Try to consume an OctetString, optionally asserting an expected value
    /// </summary>
    public static class AsnReaderExtensions
    {
        public static void ConsumeOctetString(
            this AsnReader reader,
            ReadOnlyMemory<byte>? expectedValue = null)
        {
            ReadOnlyMemory<byte> octetString = reader.ReadOctetString();

            if (expectedValue.HasValue)
            {
                var span = expectedValue.Value.Span;
                if (span.SequenceEqual(octetString.Span) == false)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Try to consume an Int32, optionally asserting an expected value
        /// </summary>
        public static void ConsumeInt32(
            this AsnReader reader,
            int? expectedValue = null)
        {
            if (reader.TryReadInt32(out var value))
            {
                if (expectedValue.HasValue == false
                    || expectedValue == value)
                {
                    return;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
