using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Transport;
using DotNetSnmp.Transport.Targets;
using System.Net;

namespace DotNetSnmp.Client
{
    public interface ISnmpDispatcher
    {
        public ValueTask<IScope> SendPdu(
            ISnmpTransport transport,
            ISnmpTarget target,
            IPEndPoint targetAddress,
            IScope scope,
            bool expectResponse,
            CancellationToken cancellationToken
        );
    }
}
