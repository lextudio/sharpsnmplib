using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Net;

namespace SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestTrapMessage
    {
        [Test]
        public void TestParseNoVarbind()
        {
            byte[] buffer = Resource.novarbind;
            TrapMessage message = new TrapMessage(buffer);
            Assert.AreEqual(6, message.GenericId);
            Assert.AreEqual(12, message.SpecificId);
            Assert.AreEqual("public", message.Community);
            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), message.AgentAddress);
            Assert.AreEqual(new uint[] { 1,3 }, message.Enterprise);
            Assert.AreEqual(16352, message.TimeStamp);
            Assert.AreEqual(0, message.Variables.Count);            
        }
        
        [Test]
        public void TestParseOneVarbind()
        {
        	byte[] buffer = Resource.onevarbind;
        	TrapMessage message = new TrapMessage(buffer);
        	Assert.AreEqual(1, message.Variables.Count);
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1000, 2 }, message.Enterprise);
        	Assert.AreEqual("TrapTest", message.Variables[0].Data.Value.ToString());
        	Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, (uint[])message.Variables[0].Id.Value);
        }
        
        [Test]
        public void TestParseTwoVarbinds()
        {
        	byte[] buffer = Resource.twovarbinds;
        	TrapMessage message = new TrapMessage(buffer);
        	Assert.AreEqual(2, message.Variables.Count);
        	Assert.AreEqual("TrapTest", message.Variables[0].Data.Value.ToString());
        	Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, (uint[])message.Variables[0].Id.Value);
        	Assert.AreEqual("TrapTest", message.Variables[1].Data.Value.ToString());
        	Assert.AreEqual(new uint[] {1,3,6,1,4,1,2162,1001,22,0}, (uint[])message.Variables[1].Id.Value);
        }

        [Test]
        public void TestParseFiveVarbinds()
        {
            byte[] buffer = Resource.fivevarbinds;
            TrapMessage message = new TrapMessage(buffer);
            Assert.AreEqual(5, message.Variables.Count);
            Assert.AreEqual("TrapTest5", message.Variables[4].Data.Value.ToString());
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1001, 25, 0 }, (uint[])message.Variables[4].Id.Value);
        }
    }
}
