using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;

namespace Lextm.SharpSnmpLib
{
    public static class Helper
    {
        public static IPAddress ToIPAddress(this IP ip)
        {
            return new IPAddress(ip.GetRaw());
        }

        public static PhysicalAddress ToPhysicalAddress(this OctetString address)
        {
            var raw = address.GetRaw();
            if (raw.Length != 6)
            {
                throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "the data length is not equal to 6: {0}", raw.Length));
            }

            return new PhysicalAddress(raw);
        }
    }
}
