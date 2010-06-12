/*
 * Created by SharpDevelop.
 * User: lexli
 * Date: 2009-1-1
 * Time: 13:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;

namespace System.Net.Sockets
{
    /// <summary>
    /// Description of UdpClientExtensions.
    /// </summary>
    public static class UdpClientExtensions
    {
        private static byte[] recvbuffer;        		
   
        public static IAsyncResult BeginReceive (UdpClient client,
                                          AsyncCallback callback,
						  object state)
		{
			recvbuffer = new byte[8192];
			
			EndPoint ep;
			
			if (client.Client.AddressFamily == AddressFamily.InterNetwork) {
				ep = new IPEndPoint (IPAddress.Any, 0);
			} else {
				ep = new IPEndPoint (IPAddress.IPv6Any, 0);
			}
			
			return(client.Client.BeginReceiveFrom (recvbuffer, 0, 8192,
							SocketFlags.None,
							ref ep,
							callback, state));
		}
		
		public static byte[] EndReceive (UdpClient client, 
                                  IAsyncResult asyncResult,
					  ref IPEndPoint remoteEP)
		{
			if (asyncResult == null) {
				throw new ArgumentNullException ("asyncResult is a null reference");
			}
			
			EndPoint ep;
			
			if (client.Client.AddressFamily == AddressFamily.InterNetwork) {
				ep = new IPEndPoint (IPAddress.Any, 0);
			} else {
				ep = new IPEndPoint (IPAddress.IPv6Any, 0);
			}
			
			int bytes = client.Client.EndReceiveFrom (asyncResult,
							   ref ep);
			remoteEP = (IPEndPoint)ep;

			/* Need to copy into a new array here, because
			 * otherwise the returned array length is not
			 * 'bytes'
			 */
			byte[] buf = new byte[bytes];
			Array.Copy (recvbuffer, buf, bytes);
			
			return(buf);
		}
    }
}
