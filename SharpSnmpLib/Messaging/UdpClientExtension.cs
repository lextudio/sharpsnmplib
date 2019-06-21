using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Messaging
{
    public static class UdpClientExtension
    {
        /// <summary>
        /// Extend the functionality of UdpClient with an timeoutable ReceiveAsync
        /// </summary>
        /// <param name="client"></param>
        /// <param name="receiver"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<UdpReceiveResult> ReceiveAsync(this UdpClient client, IPEndPoint receiver, int timeout)
        {
            // semapore the close a possible race condition (timeout and receive answer at the same time)
            SemaphoreSlim sem = new SemaphoreSlim(1, 1);
            Task<UdpReceiveResult> receiveTask = Task<UdpReceiveResult>.Factory.FromAsync(
                (callback, state) => client.BeginReceive(callback, state),
                (ar) => Receiver(client, ar, sem), null);

            Task resultTask = await Task.WhenAny(receiveTask, Task.Delay(timeout));

            await sem.WaitAsync();
            try
            {
                // terminate the updclient in case of an timeout - this will also therminate the receiver callback
                // in case it is called
                if (!receiveTask.Equals(resultTask))
                {
                    client.Close();
                    throw TimeoutException.Create(receiver.Address, timeout);
                }
            }
            finally
            {
                sem.Release();
            }
            return receiveTask.Result;
        }

        /// <summary>
        /// Handels the receiver callback and read the data
        /// it gracefully quits in case the UdpClient is already closed - no unhandled exceptoin tin this case
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ar"></param>
        /// <param name="sem"></param>
        /// <returns></returns>
        private static UdpReceiveResult Receiver(UdpClient client, IAsyncResult ar, SemaphoreSlim sem)
        {
            sem.Wait();
            try
            {
                IPEndPoint remoteEP = null;
                if (client.Client == null) return new UdpReceiveResult();
                Byte[] buffer = client.EndReceive(ar, ref remoteEP);
                return new UdpReceiveResult(buffer, remoteEP);
            }
            finally
            {
                sem.Release();
            }
        }
    }
}
