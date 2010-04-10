using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Nomral SNMP context class. It is v1 and v2c specific.
    /// </summary>
    internal class NormalSnmpContext : SnmpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalSnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="binding">The binding.</param>
        public NormalSnmpContext(ISnmpMessage request, IPEndPoint sender, Listener listener, ListenerBinding binding) 
            : base(request, sender, listener, null, binding)
        {
        }

        /// <summary>
        /// Authenticates the message.
        /// </summary>
        protected override void AuthenticateMessage()
        {
            // v1 and v2c do not need this.
        }

        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        internal override void HandleAuthenticationFailure()
        {
            // TODO: implement this later according to v1 and v2c RFC.
            Response = null;
        }

        /// <summary>
        /// Handles the membership.
        /// </summary>
        /// <returns>Always returns <code>false</code>.</returns>
        public override bool HandleMembership()
        {
            return false;
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="data">The data.</param>
        internal override void GenerateResponse(ResponseData data)
        {
            GetResponseMessage response;
            if (data.ErrorStatus == ErrorCode.NoError)
            {
                response = new GetResponseMessage(
                    Request.RequestId,
                    Request.Version,
                    Request.Parameters.UserName,
                    ErrorCode.NoError,
                    0,
                    data.Variables);
            }
            else
            {
                response = new GetResponseMessage(
                    Request.RequestId,
                    Request.Version,
                    Request.Parameters.UserName,
                    data.ErrorStatus,
                    data.ErrorIndex,
                    Request.Pdu.Variables);
            }

            if (response.ToBytes().Length > MaxResponseSize)
            {
                response = new GetResponseMessage(
                    Request.RequestId,
                    Request.Version,
                    Request.Parameters.UserName,
                    ErrorCode.TooBig,
                    0,
                    Request.Pdu.Variables);
            }

            Response = response;
        }
    }
}