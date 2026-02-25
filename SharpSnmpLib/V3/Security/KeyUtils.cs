using System.Security.Cryptography;

namespace DotNetSnmp.Protocol.V3.Security
{
    public static class KeyUtils
    {
        public const int UsmExpandedPassphraseLengthInBytes = 1024 * 1024; // 1 Mb - 1048576 bytes

        public const int UsmHashBlockSizeInBytes = 64; // bytes

        public const int UsmMinPassPhraseLen = 8; // in characters

        /// <summary>
        /// Convert <paramref name="passphrase"/> 
        /// into a localized master user key, Kul, according to the
        /// algorithm given in RFC 2274 concerning the SNMPv3 User Security Model(USM)
        /// as follows:
        /// 
        /// Expand the <paramref name="passphrase"/> to fill the passphrase buffer space, if necessary,
        /// concatenation as many duplicates as possible of P to itself.
        /// If P is larger than the buffer space, truncate it to fit.
        /// Then hash the result with the given <paramref name="hash"/> provider.
        /// Finally localize the produced digest with <paramref name="engineId"/>
        /// </summary>
        public static void GenerateLocalizedKey(
            in ReadOnlySpan<byte> passphrase,
            in ReadOnlySpan<byte> engineId,
            IncrementalHash hash,
            Span<byte> destination)
        {
            if (passphrase.Length < UsmMinPassPhraseLen)
            {
                throw new ArgumentException($"{nameof(passphrase)} must have length >= 8");
            }

            var numBytes = UsmExpandedPassphraseLengthInBytes;
            var pwdLen = passphrase.Length;
            var pwdIndex = 0;

            Span<byte> block = stackalloc byte[UsmHashBlockSizeInBytes];
            var count = 0;
            while (count < numBytes)
            {
                for (var i = 0; i < UsmHashBlockSizeInBytes; i++)
                {
                    block[i] = passphrase[pwdIndex++ % pwdLen];
                }

                hash.AppendData(block);
                count += UsmHashBlockSizeInBytes;
            }

            var digest = destination;

            // store the intermediate hash in 'destination'
            // to avoid allocating another buffer
            hash.GetHashAndReset(digest);

            hash.AppendData(digest);
            hash.AppendData(engineId);
            hash.AppendData(digest);

            hash.GetHashAndReset(digest);
        }
    }
}
