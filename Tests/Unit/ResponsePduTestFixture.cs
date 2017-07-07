using System;
using System.Collections.Generic;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class ResponsePduTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new ResponsePdu(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));
            Assert.Throws<ArgumentNullException>(() => new ResponsePdu(0, ErrorCode.NoError, 0, null));
            var pdu = new ResponsePdu(0, ErrorCode.NoError, 0, new List<Variable>());
            Assert.Throws<ArgumentNullException>(() => pdu.AppendBytesTo(null));
            Assert.Equal("Response PDU: seq: 0; status: 0; index: 0; variable count: 0", pdu.ToString());
        }
    }
}
