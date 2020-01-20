using DTLS;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    public static class SecureMessageExtensions
    {
        public static async Task<ISnmpMessage> GetSecureResponse(this ISnmpMessage request, int connectionTimeout, int responseTimeout, IPEndPoint receiver, Client client)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver is null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var registry = new UserRegistry();
            //if (request.Version == VersionCode.V3)
            //{
            //    registry.Add(request.Parameters.UserName, request.Privacy);
            //}

            return await request.GetSecureResponse(connectionTimeout, responseTimeout, receiver, client, registry);
        }

        /// <summary>
        /// Sends an  <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="connectionTimeout">The time-out value, in milliseconds for how long to wait for a connection</param>
        /// <param name="responseTimeout">The time-out value, in milliseconds for how long to wait for a response. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="client">The DTLS client</param>
        /// <param name="registry">The user registry.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetSecureResponse(this ISnmpMessage request, int connectionTimeout, int responseTimeout, IPEndPoint receiver, Client client, UserRegistry registry)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var requestCode = request.TypeCode();
            if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", requestCode));
            }

            var bytes = request.ToBytes();
            await client.ConnectToServerWithTimeoutAsync(receiver, connectionTimeout);
            var reply = await client.SendAndGetResponseWithTimeoutAsync(bytes, responseTimeout);

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid an issue if parsing >1 response).
            var response = MessageFactory.ParseMessages(reply, 0, reply.Length, registry)[0];
            var responseCode = response.TypeCode();
            if (responseCode == SnmpType.ResponsePdu || responseCode == SnmpType.ReportPdu)
            {
                var requestId = request.MessageId();
                var responseId = response.MessageId();
                if (responseId != requestId)
                {
                    throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", requestId, responseId), receiver.Address);
                }

                return response;
            }

            throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", responseCode), receiver.Address);
        }
    }
}
