using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Nomral SNMP context class. It is v1 and v2c specific.
    /// </summary>
    internal sealed class NormalSnmpContext : SnmpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalSnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="users">The users.</param>
        /// <param name="binding">The binding.</param>
        public NormalSnmpContext(ISnmpMessage request, IPEndPoint sender, UserRegistry users, IListenerBinding binding) 
            : base(request, sender, users, null, binding)
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
            if (!data.HasResponse)
            {
                return;
            }

            ResponseMessage response;
            if (data.ErrorStatus == ErrorCode.NoError)
            {
                response = new ResponseMessage(
                    Request.RequestId,
                    Request.Version,
                    Request.Parameters.UserName,
                    ErrorCode.NoError,
                    0,
                    data.Variables);
            }
            else
            {
                response = new ResponseMessage(
                    Request.RequestId,
                    Request.Version,
                    Request.Parameters.UserName,
                    data.ErrorStatus,
                    data.ErrorIndex,
                    Request.Pdu.Variables);
            }

            if (response.ToBytes().Length > MaxResponseSize)
            {
                response = new ResponseMessage(
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