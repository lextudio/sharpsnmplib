using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Common.Helpers;
using System.Formats.Asn1;
using System.Net;
using System.Text.RegularExpressions;

namespace DotNetSnmp.Transport
{
    public class UdpEndpoint : IAsnSerializable
    {
        private IPEndPoint Endpoint { get; init; }

        public int Port => Endpoint?.Port ?? 0;

        // match only, no validation
        private static Regex _udpAddrRegex =
            new(@"^(?<ip>(?:\d{1,3}\.){3}\d{1,3})(?<port>(?::|\/)\d+)*$");

        public UdpEndpoint(int port)
        {
            Endpoint = new(IPAddress.Any, port);
        }

        public UdpEndpoint(IPEndPoint endpoint)
        {
            Endpoint = endpoint;
        }

        public UdpEndpoint(string address, int port)
        {
            Endpoint = new IPEndPoint(
                IPAddress.Parse(address),
                port);
        }
        public UdpEndpoint(string address)
        {
            var m = _udpAddrRegex.Match(address);

            if (!m.Success)
            {
                throw new ArgumentException($"{address} must be in 0.0.0.0/0 format.");
            }

            short port = 161;

            var ipAddr = IPAddress.None;

            if (m.Groups.TryGetValue("ip", out var ipGroup))
            {
                if (!IPAddress.TryParse(ipGroup.Value, out ipAddr))
                {
                    throw new ArgumentException($"invalid op {ipGroup.Value}");
                }
            }

            if (m.Groups.TryGetValue("port", out var portGroup) && portGroup != null)
            {
                if (!short.TryParse(portGroup.Value, out port))
                {
                    throw new ArgumentException($"invalid port {portGroup.Value}");
                }
            }

            Endpoint = new IPEndPoint(ipAddr, port);
        }

        public void WriteTo(AsnWriter writer)
        {
            Span<byte> octets = stackalloc byte[6];
            Endpoint?.Address.TryWriteBytes(octets[0..4], out _);
            // TODO: check byte-order
            BinaryHelpers.CopyBytesMostSignificantFirst((short)Port, octets[4..6]);
            writer.WriteOctetString(octets);
        }
    }
}
