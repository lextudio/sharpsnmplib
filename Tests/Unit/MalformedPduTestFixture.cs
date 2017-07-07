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
            Assert.Equal(0, pdu.Variables.Count);
            Assert.Equal(Integer32.Zero, pdu.RequestId);
            Assert.Equal(SnmpType.Unknown, pdu.TypeCode);
            Assert.Equal("Malformed PDU", pdu.ToString());
        }
    }
}
