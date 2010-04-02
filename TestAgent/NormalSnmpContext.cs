using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class NormalSnmpContext : SnmpContext
    {
        public NormalSnmpContext(ISnmpMessage request, IPEndPoint sender, Listener listener) 
            : base(request, sender, listener, null)
        {
        }

        protected override void AuthenticateMessage()
        {
        }

        internal override void HandleAuthenticationFailure()
        {
            Response = null;
        }

        public override bool HandleMembership()
        {
            return false;
        }

        internal override void GenerateResponse(ResponseData data)
        {
            GetResponseMessage response;
            if (data.ErrorStatus == ErrorCode.NoError)
            {
                // for v1 and v2 reply.
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
                    // TODO: check RFC to see what should be returned here.
                    new List<Variable>());
            }

            Response = response;
        }
    }
}