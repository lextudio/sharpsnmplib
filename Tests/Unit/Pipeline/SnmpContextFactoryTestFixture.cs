using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Moq;
using Xunit;

namespace Lextm.SharpSnmpLib.Pipeline.Tests
{
    public class SnmpContextFactoryTestFixture
    {
        [Fact]
        public void Test()
        {
            var messageMock = new Mock<ISnmpMessage>();
            messageMock.Setup(foo => foo.Version).Returns(VersionCode.V3);
            var bindingMock = new Mock<IListenerBinding>();
            var context = SnmpContextFactory.Create(messageMock.Object, new IPEndPoint(IPAddress.Loopback, 0), new UserRegistry(),
                                      new EngineGroup(),
                                      bindingMock.Object);
            context.SendResponse();
            bindingMock.Verify(foo => foo.SendResponse(It.IsAny<ISnmpMessage>(), It.IsAny<EndPoint>()), Times.AtMostOnce);
        }
    }
}
