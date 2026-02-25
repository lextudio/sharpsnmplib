using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Transport;
using DotNetSnmp.Transport.Targets;
using System.Net;

namespace DotNetSnmp.Client
{
    /// <summary>
    /// Defines the contract for ISnmpDispatcher.
    /// </summary>
    public interface ISnmpDispatcher
    {
        /// <summary>
        /// Sends pdu.
        /// </summary>
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
