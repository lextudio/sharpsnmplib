using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class MalformedPduTestFixture
    {
        [Fact]
        public void Test()
        {
            var pdu = new MalformedPdu();
            Assert.Throws<NotSupportedException>(() => { var test = pdu.ErrorIndex; });
            Assert.Throws<NotSupportedException>(() => { var test = pdu.ErrorStatus; });
            Assert.Throws<NotSupportedException>(() => pdu.AppendBytesTo(null));
            Assert.Empty(pdu.Variables);
            Assert.Equal(Integer32.Zero, pdu.RequestId);
            Assert.Equal(SnmpType.Unknown, pdu.TypeCode);
            Assert.Equal("Malformed PDU", pdu.ToString());
        }
    }
}
