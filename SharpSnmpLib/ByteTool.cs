/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 12:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// The signature of the callback method to use when calling BeginGetResponse,
	/// ie the method used when we receive a response.
	/// </summary>
	/// <param name="from">The <see cref="IPAddress"/> of the equipment from which comes the response.</param>
	/// <param name="responseMsg">The response itself.</param>
	public delegate void GetResponseCallback(IPAddress from, GetResponseMessage responseMsg);
	/// <summary>
	/// The signature of the callback method to use when calling BeginGetResponseRaw (ie before parsing bytes into a <see cref="ISnmpMessage"/>,
	/// ie the method used when we receive a response.
	/// </summary>
	/// <param name="from">The <see cref="IPAddress"/> of the equipment from which comes the response.</param>
	/// <param name="rawResponseBuffer">The buffer where the response is stored, non-deserialized and with extra bytes
	/// (the buffer may contain more bytes than the response itself).</param>
	/// <param name="byteReadCount">The number of useful byte to read from <paramref name="rawResponseBuffer"/>.</param>
	public delegate void GetResponseRawCallback(IPAddress from, byte[] rawResponseBuffer, int byteReadCount);

	/// <summary>
	/// Description of ByteTool.
	/// </summary>
	internal static class ByteTool
	{
		/// <summary>
		/// Sends an SNMP message and wait for its responses.
		/// </summary>
		/// <param name="receiver">The IP address and port of the target to talk to.</param>
		/// <param name="bytes">The byte array representing the SNMP message.</param>
		/// <param name="number">The <see cref="GetResponseMessage.SequenceNumber"/> of the SNMP message.</param>
		/// <param name="timeout">The timeout above which, if the response is not received, a <see cref="SharpTimeoutException"/> is thrown.</param>
		/// <returns>The response message (<see cref="GetResponseMessage"/>).</returns>
		internal static GetResponseMessage GetResponse(IPEndPoint receiver, byte[] bytes, int number, int timeout)
		{
			using (Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				return GetResponse(receiver, bytes, number, timeout, udpSocket);
			}
		}

		/// <summary>
		/// Sends an SNMP message and wait for its responses.
		/// </summary>
		/// <param name="receiver">The IP address and port of the target to talk to.</param>
		/// <param name="bytes">The byte array representing the SNMP message.</param>
		/// <param name="number">The <see cref="GetResponseMessage.SequenceNumber"/> of the SNMP message.</param>
		/// <param name="timeout">The timeout above which, if the response is not received, a <see cref="SharpTimeoutException"/> is thrown.</param>
		/// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
		/// <returns>The response message (<see cref="GetResponseMessage"/>).</returns>
		internal static GetResponseMessage GetResponse(IPEndPoint receiver, byte[] bytes, int number, int timeout, Socket udpSocket)
		{
			Capture(bytes); // log request

			//IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
			#if CF
			int bufSize = 8192;
			#else
			int bufSize = udpSocket.ReceiveBufferSize;
			#endif
			byte[] reply = new byte[bufSize];

			// Whatever you change, try to keep the Send and the BeginReceive close to each other.
			udpSocket.SendTo(bytes, receiver);
			IAsyncResult result = udpSocket.BeginReceive(reply, 0, bufSize, SocketFlags.None, null, null);
			result.AsyncWaitHandle.WaitOne(timeout, false);
			if (!result.IsCompleted)
			{
				throw SharpTimeoutException.Create(receiver.Address, timeout);
			}

			int count = udpSocket.EndReceive(result);

			ISnmpMessage message;
			// Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid a bug if parsing >1 response).
			using (MemoryStream m = new MemoryStream(reply, 0, count, false))
			{
				message = MessageFactory.ParseMessages(m)[0];
			}

			if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
			{
				throw SharpOperationException.Create("wrong response type", receiver.Address);
			}

			GetResponseMessage response = (GetResponseMessage)message;
			if (response.SequenceNumber != number)
			{
				throw SharpOperationException.Create("wrong response sequence", receiver.Address);
			}

			Capture(reply); // log response
			return response;
		}

		/// <summary>
		/// Sends an SNMP message and wait for its responses asynchronously.
		/// </summary>
		/// <param name="receiver">The IP address and port of the target to talk to.</param>
		/// <param name="bytes">The byte array representing the SNMP message.</param>
		/// <param name="number">The <see cref="GetResponseMessage.SequenceNumber"/> of the SNMP message.</param>
		/// <param name="timeout">The timeout above which, if the response is not received, a <see cref="SharpTimeoutException"/> is thrown.</param>
		/// <param name="callback">The callback called once the response has been received.</param>
		internal static void BeginGetResponse(IPEndPoint receiver, byte[] bytes, int number, int timeout, GetResponseCallback callback)
		{
			using (Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				BeginGetResponse(receiver, bytes, number, timeout, callback, udpSocket);
			}
		}

		/// <summary>
		/// Sends an SNMP message and wait for its responses asynchronously.
		/// </summary>
		/// <param name="receiver">The IP address and port of the target to talk to.</param>
		/// <param name="bytes">The byte array representing the SNMP message.</param>
		/// <param name="number">The <see cref="GetResponseMessage.SequenceNumber"/> of the SNMP message.</param>
		/// <param name="timeout">The timeout above which, if the response is not received, a <see cref="SharpTimeoutException"/> is thrown.</param>
		/// <param name="callback">The callback called once the response has been received.</param>
		/// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
		internal static void BeginGetResponse(IPEndPoint receiver, byte[] bytes, int number, int timeout, GetResponseCallback callback, Socket udpSocket)
		{
			Capture(bytes); // log request

			#if CF
			int bufSize = 8192;
			#else
			int bufSize = udpSocket.ReceiveBufferSize;
			#endif
			byte[] reply = new byte[bufSize];

			// Whatever you change, try to keep the Send and the BeginReceive close to each other.
			udpSocket.SendTo(bytes, receiver);
			udpSocket.BeginReceive(reply, 0, bufSize, SocketFlags.None, GetResponseInternalCallback,
			                       new object[] { udpSocket, reply, receiver, timeout, callback, number });
		}

		/// <summary>
		/// Sends an SNMP message and wait for its responses asynchronously.
		/// With this raw overload, the callback simply returns a byte array and do not process it back into a <see cref="GetResponseMessage"/>.
		/// </summary>
		/// <param name="receiver">The IP address and port of the target to talk to.</param>
		/// <param name="bytes">The byte array representing the SNMP message.</param>
		/// <param name="number">The <see cref="GetResponseMessage.SequenceNumber"/> of the SNMP message.</param>
		/// <param name="timeout">The timeout above which, if the response is not received, a <see cref="SharpTimeoutException"/> is thrown.</param>
		/// <param name="callback">The callback called once the response has been received.</param>
		/// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
		internal static void BeginGetResponseRaw(IPEndPoint receiver, byte[] bytes, int number, int timeout, GetResponseRawCallback callback, Socket udpSocket)
		{
			Capture(bytes); // log request

			#if CF
			int bufSize = 8192;
			#else
			int bufSize = udpSocket.ReceiveBufferSize;
			#endif
			byte[] reply = new byte[bufSize];

			// Whatever you change, try to keep the Send and the BeginReceive close to each other.
			udpSocket.SendTo(bytes, receiver);
			udpSocket.BeginReceive(reply, 0, bufSize, SocketFlags.None, GetResponseRawInternalCallback,
			                       new object[] { udpSocket, reply, receiver, timeout, callback });
		}

		static void GetResponseInternalCallback(IAsyncResult result)
		{
			object[] data = (object[])result.AsyncState;
			Socket udpSocket = (Socket)data[0];
			byte[] reply = (byte[])data[1];
			IPEndPoint receiver = (IPEndPoint)data[2];
			int timeout = (int)data[3];
			GetResponseCallback callback = (GetResponseCallback)data[4];
			int number = (int)data[5];

			result.AsyncWaitHandle.WaitOne(timeout, false);
			if (!result.IsCompleted)
			{
				// Timeout -> return null. Can't throw exception in this thread.
				//throw SharpTimeoutException.Create(receiver.Address, timeout);
				callback(receiver.Address, null);
				return;
			}

			int count = udpSocket.EndReceive(result);

			ISnmpMessage message;
			// Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid a bug if parsing >1 response).
			using (MemoryStream m = new MemoryStream(reply, 0, count, false))
			{
				message = MessageFactory.ParseMessages(m)[0];
			}

			if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
			{
				// Wrong response type -> return null. Can't throw exception in this thread.
				//throw SharpOperationException.Create("wrong response type", receiver.Address);
				callback(receiver.Address, null);
				return;
			}

			GetResponseMessage response = (GetResponseMessage)message;
			if (response.SequenceNumber != number)
			{
				// Wrong response sequence -> return null. Can't throw exception in this thread.
				//throw SharpOperationException.Create("wrong response sequence", receiver.Address);
				callback(receiver.Address, null);
				return;
			}

			Capture(reply); // log response

			callback(receiver.Address, response);
		}

		static void GetResponseRawInternalCallback(IAsyncResult result)
		{
			object[] data = (object[])result.AsyncState;
			Socket udpSocket = (Socket)data[0];
			byte[] reply = (byte[])data[1];
			IPEndPoint receiver = (IPEndPoint)data[2];
			int timeout = (int)data[3];
			GetResponseRawCallback callback = (GetResponseRawCallback)data[4];

			result.AsyncWaitHandle.WaitOne(timeout, false);
			if (!result.IsCompleted)
			{
				// Timeout -> return null. Can't throw exception in this thread.
				//throw SharpTimeoutException.Create(receiver.Address, timeout);
				callback(receiver.Address, null, 0);
				return;
			}

			int count = udpSocket.EndReceive(result);

			callback(receiver.Address, reply, count);
		}

		// TODO: add this method to all message exchanges.
		internal static void Capture(ISnmpMessage message)
		{
			object setting = ConfigurationManager.AppSettings["CaptureEnabled"];
			if (setting != null && Convert.ToBoolean(setting.ToString(), CultureInfo.InvariantCulture))
			{
				byte[] buffer = message.ToBytes();
				CaptureInner(buffer, buffer.Length);
			}
		}

		internal static void Capture(byte[] buffer)
		{
			Capture(buffer, buffer.Length);
		}

		internal static void Capture(byte[] buffer, int length)
		{
			object setting = ConfigurationManager.AppSettings["CaptureEnabled"];
			if (setting != null && Convert.ToBoolean(setting.ToString(), CultureInfo.InvariantCulture))
			{
				CaptureInner(buffer, length);
			}
		}
		
		private static void CaptureInner(byte[] buffer, int length)
		{
			TraceSource source = new TraceSource("Library");
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				builder.AppendFormat("{0:X2} ", buffer[i]);
			}
			
			source.TraceInformation("SNMP packet captured at {0}, length {1}", DateTime.Now, length);
			source.TraceInformation(builder.ToString());
			source.Flush();
			source.Close();
		}

		internal static byte[] ParseByteString(string bytes)
		{
			List<byte> result = new List<byte>();
			StringBuilder buffer = new StringBuilder(2);
			foreach (char c in bytes)
			{
				if (char.IsWhiteSpace(c))
				{
					continue;
				}
				
				if (!char.IsLetterOrDigit(c))
				{
					throw new ArgumentException("illegal character found", "bytes");
				}
				
				buffer.Append(c);
				if (buffer.Length == 2)
				{
					result.Add(byte.Parse(buffer.ToString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture));
					buffer.Length = 0;
				}
			}
			
			if (buffer.Length != 0)
			{
				throw new ArgumentException("not a complete byte string", "bytes");
			}
			
			return result.ToArray();
		}
		
		internal static bool CompareArray<T>(IList<T> left, IList<T> right) where T : IEquatable<T>
		{
			if (left.Count != right.Count)
			{
				return false;
			}
			
			for (int i = 0; i < left.Count; i++)
			{
				if (!left[i].Equals(right[i]))
				{
					return false;
				}
			}
			
			return true;
		}

		internal static byte[] ParseItems(params ISnmpData[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			MemoryStream result = new MemoryStream();
			foreach (ISnmpData item in items)
			{
				item.AppendBytesTo(result);
			}
			
			return result.ToArray();
		}
		
		internal static byte[] ParseItems(IEnumerable items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			if (!(items is IEnumerable<ISnmpData>))
			{
				throw new ArgumentException("items must be IEnumerable<ISnmpData>");
			}
			
			MemoryStream result = new MemoryStream();
			foreach (ISnmpData item in items)
			{
				item.AppendBytesTo(result);
			}
			
			return result.ToArray();
		}
		
		internal static void WritePayloadLength(Stream stream, int length) // excluding initial octet
		{
			if (length < 0)
			{
				throw new ArgumentException("length cannot be negative", "length");
			}
			
			if (length < 127)
			{
				stream.WriteByte((byte)length);
				return;
			}
			
			byte[] c = new byte[16];
			int j = 0;
			while (length > 0)
			{
				c[j++] = (byte)(length & 0xff);
				length = length >> 8;
			}
			
			stream.WriteByte((byte)(0x80 | j));
			while (j > 0)
			{
				int x = c[--j];
				stream.WriteByte((byte)x);
			}
		}
		
		internal static int ReadPayloadLength(Stream stream)
		{
			int first = stream.ReadByte();
			return ReadLength(stream, (byte)first);
		}

		internal static byte[] IgnoreBytes(Stream stream, int length)
		{
			byte[] bytes = new byte[length];
			stream.Read(bytes, 0, length);
			return bytes;
		}
		
		// copied from universal
		private static int ReadLength(Stream stream, byte first) // x is initial octet
		{
			if ((first & 0x80) == 0)
			{
				return first;
			}
			
			int result = 0;
			int octets = first & 0x7f;
			for (int j = 0; j < octets; j++)
			{
				result = (result << 8) + ReadByte(stream);
			}
			
			return result;
		}
		
		// copied from universal
		private static byte ReadByte(Stream s)
		{
			int n = s.ReadByte();
			if (n == -1)
			{
				throw new SharpSnmpException("BER end of file");
			}
			
			return (byte)n;
		}
		
		internal static void AppendBytes(Stream stream, SnmpType typeCode, byte[] raw)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			
			stream.WriteByte((byte)typeCode);
			WritePayloadLength(stream, raw.Length);
			stream.Write(raw, 0, raw.Length);
		}
		
		internal static byte[] GetRawBytes(byte[] orig, bool negative)
		{
			byte flag;
			byte sign;
			if (negative)
			{
				flag = 0xff;
				sign = 0x80;
			}
			else
			{
				flag = 0x0;
				sign = 0x0;
			}

			List<byte> list = new List<byte>(orig);
			while (list.Count > 1)
			{
				if (list[list.Count - 1] == flag)
				{
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					break;
				}
			}

			// if sign bit is not correct, add an extra byte
			if ((list[list.Count - 1] & 0x80) != sign)
			{
				list.Add(sign);
			}

			list.Reverse();
			return list.ToArray();
		}

		internal static Sequence PackMessage(VersionCode version, OctetString community, ISnmpPdu pdu)
		{
			return new Sequence(new Integer32((int)version), community, pdu);
		}
	}
}
