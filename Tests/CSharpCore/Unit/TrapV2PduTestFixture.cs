﻿using System;
using System.Collections.Generic;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class TrapV2PduTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new TrapV2Pdu(null, null));
            Assert.Throws<ArgumentNullException>(() => new TrapV2Pdu(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));
            Assert.Throws<ArgumentNullException>(() => new TrapV2Pdu(0, null, 0, null));
            Assert.Throws<ArgumentNullException>(() => new TrapV2Pdu(0, new ObjectIdentifier("1.3.6"), 0, null));
            var pdu = new TrapV2Pdu(0, new ObjectIdentifier("1.3"), 0, new List<Variable>());
            Assert.Equal(Integer32.Zero, pdu.ErrorIndex);
            Assert.Equal(Integer32.Zero, pdu.ErrorStatus);
            Assert.Throws<ArgumentNullException>(() => pdu.AppendBytesTo(null));

            Assert.Equal("TRAP v2 PDU: request ID: 0; enterprise: 1.3; time stamp: 00:00:00; variable count: 0", pdu.ToString());
        }
    }
}
