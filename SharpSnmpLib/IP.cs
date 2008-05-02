using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpSnmpLib
{
	public struct IP: ISnmpData, IEquatable<IP>
	{
		byte[] _raw;
		
		public IP(byte[] raw)
		{
			if (raw.Length != 4)
			{
				throw new ArgumentException("bytes must contain 4 elements");
			}
			_raw = raw;
			_ip = new IPAddress(raw);
            _bytes = null;
		}
		
		public IP(string ip)
		{
			_ip = IPAddress.Parse(ip);
			_raw = _ip.GetAddressBytes();
            _bytes = null;
		}
		
		static Regex regex = new Regex(
			"^(?<First>2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.(?<Second>2[0-4]"+
			"\\d|25[0-5]|[01]?\\d\\d?)\\.(?<Third>2[0-4]\\d|25[0-5]|[01]?"+
			"\\d\\d?)\\.(?<Fourth>2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$",
			RegexOptions.IgnoreCase
			| RegexOptions.CultureInvariant
			| RegexOptions.IgnorePatternWhitespace
			| RegexOptions.Compiled);

		IPAddress _ip;

		public override string ToString()
		{
			return _ip.ToString();
		}

		public IPAddress ToIPAddress()
		{
			return _ip;
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.IpAddress;
			}
		}

        byte[] _bytes;
		
		public byte[] ToBytes()
		{
            if (_bytes == null)
            {
                _bytes = ByteTool.ToBytes(TypeCode, _raw);
            }
            return _bytes;
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}
			if (object.ReferenceEquals(this, obj)) {
				return true;
			}
			if (GetType() != obj.GetType()) {
				return false;
			}
			return Equals((IP)obj);
		}
		
		public bool Equals(IP other)
		{
			return _ip.Equals(other._ip);
		}
		
		public override int GetHashCode()
		{
			return _ip.GetHashCode();
		}
		
		public static bool operator == (IP left, IP right)
		{
			return left.Equals(right);
		}
		
		public static bool operator != (IP left, IP right)
		{
			return !(left == right);
		}
	}
}
