/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/8
 * Time: 19:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestMessageFactory
    {
//        [Test]
//        public void TestMethod()
//        {
//            MemoryStream m = new MemoryStream();
//            m.Write(Resource.getresponse, 0, Resource.getresponse.Length);
//            m.Write(Resource.getresponse, 0, Resource.getresponse.Length);
//            m.Flush();
//            m.Position = 0;
//            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(m);
//            Assert.AreEqual(2, messages.Count);
//        }
		
		[Test]
		public void TestInform()
		{
		byte[] data = new byte[] { 0x30, 0x5d, 0x02, 0x01, 0x01, 0x04, 0x06, 0x70, 0x75, 0x62, 0x6c, 0x69, 0x63, 0xa6, 0x50, 0x02, 0x01, 0x01, 0x02, 0x01,
0x00, 0x02, 0x01, 0x00, 0x30, 0x45, 0x30, 0x0e, 0x06, 0x08, 0x2b, 0x06, 0x01, 0x02, 0x01, 0x01, 0x03, 0x00, 0x43, 0x02,
0x3f, 0xe0, 0x30, 0x18, 0x06, 0x0a, 0x2b, 0x06, 0x01, 0x06, 0x03, 0x01, 0x01, 0x04, 0x01, 0x00, 0x06, 0x0a, 0x2b, 0x06,
0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02, 0x30, 0x19, 0x06, 0x0b, 0x2b, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87,
0x69, 0x15, 0x00, 0x04, 0x0a, 0x49, 0x6e, 0x66, 0x6f, 0x72, 0x6d, 0x54, 0x65, 0x73, 0x74 };

			IList<ISnmpMessage> messages = MessageFactory.ParseMessages(new MemoryStream(data));
			Assert.AreEqual(SnmpType.InformRequestPdu, messages[0].TypeCode);
			//Assert.AreEqual(4, messages[0].TimeStamp);
		}
		
		[Test]
		public void TestString()
		{
		    string bytes = "30 29 02 01 00 04 06 70 75 62 6c 69 63 a0 1c 02 04 4f 89 fb dd" + Environment.NewLine +
            "02 01 00 02 01 00 30 0e 30 0c 06 08 2b 06 01 02 01 01 05 00 05 00";
		    IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes);
		    Assert.AreEqual(1, messages.Count);
		    GetRequestMessage m = (GetRequestMessage)messages[0];
		    Variable v = m.Variables[0];
		    string i = v.Id.ToString();
		}
    }
}
#pragma warning restore 1591