using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
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
