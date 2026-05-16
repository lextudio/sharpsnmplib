using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Helper utility class for v12 API compatibility.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Converts <see cref="IP"/> to <see cref="IPAddress"/>.
        /// </summary>
        public static IPAddress ToIPAddress(this IP ip)
        {
            return new IPAddress(ip.AddressBytes);
        }

        /// <summary>
        /// Converts <see cref="OctetString"/> to <see cref="PhysicalAddress"/>.
        /// </summary>
        public static PhysicalAddress ToPhysicalAddress(this OctetString address)
        {
            var raw = address.Octets.ToArray();
            if (raw.Length != 6)
            {
                throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "the data length is not equal to 6: {0}", raw.Length));
            }

            return new PhysicalAddress(raw);
        }
    }
}
