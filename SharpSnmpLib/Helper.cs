using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Class Helper.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Converts <see cref="IP" /> to <see cref="IPAddress" />.
        /// </summary>
        /// <param name="ip">The <see cref="IP" /> object.</param>
        /// <returns>The <see cref="IPAddress" /> object.</returns>
        public static IPAddress ToIPAddress(this IP ip)
        {
            return new IPAddress(ip.GetRaw());
        }

        /// <summary>
        /// Converts <see cref="OctetString" /> to <see cref="PhysicalAddress" />.
        /// </summary>
        /// <param name="address">The <see cref="OctetString" /> object that holds the address.</param>
        /// <returns>The <see cref="PhysicalAddress" /> object.</returns>
        /// <exception cref="System.InvalidCastException"><paramref name="address"/> length is not equal to 6.</exception>
        public static PhysicalAddress ToPhysicalAddress(this OctetString address)
        {
            var raw = address.GetRaw();
            if (raw.Length != 6)
            {
                throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "the data length is not equal to 6: {0}", raw.Length));
            }

            return new PhysicalAddress(raw);
        }

        private static bool? desSupported;

        internal static bool DESSupported
        {
            get
            {
                if (desSupported != null)
                {
                    return desSupported.Value;
                }

                return (desSupported = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework")
                    || RuntimeInformation.FrameworkDescription.StartsWith(".NET 6.")
                    || RuntimeInformation.FrameworkDescription.StartsWith(".NET 5.")
                    || RuntimeInformation.FrameworkDescription.StartsWith(".NET Core 3.1.")).Value;
            }
        }

        private static bool? aesSupported;

        internal static bool AESSupported
        {
            get
            {
                if (aesSupported != null)
                {
                    return aesSupported.Value;
                }

                return (aesSupported = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework")
                    || RuntimeInformation.FrameworkDescription.StartsWith(".NET 6.")
                    || RuntimeInformation.FrameworkDescription.StartsWith(".NET 5.")).Value;
            }
        }
    }
}
