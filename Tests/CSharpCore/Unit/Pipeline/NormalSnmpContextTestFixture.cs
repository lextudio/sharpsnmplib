using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;
using Moq;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Pipeline
{
    public class NormalSnmpContextTestFixture
    {
        [Fact]
        public void Test()
        {
            var message = new GetRequestMessage(0, VersionCode.V1, new OctetString("public"), new List<Variable>());
            var bindingMock = new Mock<IListenerBinding>();
            bindingMock.Setup(foo => foo.SendResponse(It.IsAny<ISnmpMessage>(), It.IsAny<EndPoint>()));
            var context = new NormalSnmpContext(message, new IPEndPoint(IPAddress.Loopback, 0),
                                                new UserRegistry(), bindingMock.Object);
            context.GenerateResponse(new List<Variable>());
            Assert.NotNull(context.Response);
            context.SendResponse();
            Assert.False(context.HandleMembership());

            var list = new List<Variable>();
            for (int i = 0; i < 5000; i++)
            {
                list.Add(new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.4.0"), new OctetString("test")));
            }

            context.GenerateResponse(list);
            Assert.Equal(ErrorCode.TooBig, context.Response.Pdu().ErrorStatus.ToErrorCode());
            bindingMock.Verify(foo => foo.SendResponse(It.IsAny<ISnmpMessage>(), It.IsAny<EndPoint>()), Times.AtMostOnce);
        }
    }
}
