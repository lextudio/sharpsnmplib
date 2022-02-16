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
    public class Net6MessengerTestFixture
    {
        #if NET6_0
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
                    await Assert.ThrowsAsync<OperationCanceledException>(() => getTask);
                }
            }
        }

        [Fact]
        public async Task TestSetAsyncCanBeCancelled()
        {
            var receiver = new IPEndPoint(IPAddress.Loopback, 11337);
            using (new UdpClient(receiver))//listen to prevent ICMP unreachable
            {
                using (var cts = new CancellationTokenSource())
                {
                    var getTask = Messenger.SetAsync(VersionCode.V2, receiver, OctetString.Empty, new List<Variable>(), cts.Token);
                    cts.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(() => getTask);
                }
            }
        }

        [Fact]
        public async Task TestWalkAsyncCanBeCancelled()
        {
            var receiver = new IPEndPoint(IPAddress.Loopback, 11337);
            using (new UdpClient(receiver))//listen to prevent ICMP unreachable
            {
                using (var cts = new CancellationTokenSource())
                {
                    var getTask = Messenger.WalkAsync(VersionCode.V2, receiver, OctetString.Empty, new ObjectIdentifier("0.0"), new List<Variable>(), WalkMode.WithinSubtree, cts.Token);
                    cts.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(() => getTask);
                }
            }
        }

        [Fact]
        public async Task TestBulkWalkAsyncCanBeCancelled()
        {
            var receiver = new IPEndPoint(IPAddress.Loopback, 11337);
            using (new UdpClient(receiver))//listen to prevent ICMP unreachable
            {
                using (var cts = new CancellationTokenSource())
                {
                    var getTask = Messenger.BulkWalkAsync(VersionCode.V2, receiver, OctetString.Empty, OctetString.Empty, new ObjectIdentifier("0.0"), new List<Variable>(), 5, WalkMode.WithinSubtree, null, null, cts.Token);
                    cts.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(() => getTask);
                }
            }
        }
        #endif
    }
}
#pragma warning restore 1591
