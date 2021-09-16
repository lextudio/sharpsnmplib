using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Unit
{
    public class SocketTestFixture
    {
        [Fact]
        public async Task TestGetAsyncCanBeCancelled()
        {
            var receiver = new IPEndPoint(IPAddress.Loopback, 11337);
            using (new UdpClient(receiver))//listen to prevent ICMP unreachable
            {
                using (var cts = new CancellationTokenSource())
                {
                    var getTask = Messenger.GetAsync(VersionCode.V2, receiver, OctetString.Empty, new List<Variable>(), cts.Token);
                    cts.Cancel();
                    await Assert.ThrowsAsync<TaskCanceledException>(() => getTask);
                }
            }
        }
    }
}
#pragma warning restore 1591
