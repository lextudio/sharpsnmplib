using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetSnmp.Utils
{
    /// <summary>
    /// Provides helper methods for Dump.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Dump
    {
        /*
            Sending 43 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
            0000: 30 29 02 01  00 04 06 70  75 62 6C 69  63 A0 1C 02    0).....public...
            0016: 04 0C BB 47  10 02 01 00  02 01 00 30  0E 30 0C 06    ...G.......0.0..
            0032: 08 2B 06 01  02 01 01 01  00 05 00                    .+.........
        */

        private static readonly Regex _headerRegex = new(@"^(?:Received|Sending) (?<bytes>\d+) (?:.*) (:?from|to)");

        /// <summary>
        /// Parses a textual packet dump into raw bytes.
        /// </summary>
        public static byte[] BytesFromDumpString(string textualDump)
        {
            var lines = Regex.Split(textualDump.TrimStart(), @"\r*\n")
                .Select(x => x.Trim())
                .ToList();

            var headerLine = lines.First();

            var hexData = lines
                .Skip(1)
                .Select(x => x.Split(':')[1])
                .Select(x => x.Split("    ")[0])
                .SelectMany(l => l.Split())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(b => Convert.ToByte(b, 16))
                .ToArray();

            var m = _headerRegex.Match(headerLine);

            var bytes = int.Parse(m.Groups["bytes"].Value);

            if (bytes != hexData.Length)
            {
                throw new Exception("Dump parse error");
            }

            return hexData;
        }

        /// <summary>
        /// Parses a hexadecimal string into raw bytes.
        /// </summary>
        public static byte[] BytesFromHexString(string hexString)
        {
            hexString = hexString.Replace(" ", string.Empty); // Remove any spaces
            var bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// Converts raw bytes to an uppercase hexadecimal string without separators.
        /// </summary>
        public static string BytesToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            var hexBuilder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                hexBuilder.Append(b.ToString("X2"));
            }

            return hexBuilder.ToString();
        }
    }
}
