using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace Lextm.SharpSnmpLib.Pipeline.Tests
{
    public class InformRequestMessageReceivedEventArgsTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new InformRequestMessageReceivedEventArgs(null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new InformRequestMessageReceivedEventArgs(new IPEndPoint(IPAddress.Any, 0), null, null));
            IList<Variable> v = new List<Variable>();
            Assert.Throws<ArgumentNullException>(
                () =>
                new InformRequestMessageReceivedEventArgs(new IPEndPoint(IPAddress.Any, 0),
                                                   new InformRequestMessage(0,
                                                                     VersionCode.V2,
                                                                     new OctetString("community"),
                                                                     new ObjectIdentifier("1.3.6"),
                                                                     0,
                                                                     v),
                                                   null));
        }
    }
}
