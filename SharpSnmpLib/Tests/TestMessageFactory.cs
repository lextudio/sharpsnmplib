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
        [Test]
        public void TestMethod()
        {
            MemoryStream m = new MemoryStream();
            m.Write(Resource.getresponse, 0, Resource.getresponse.Length);
            m.Write(Resource.getresponse, 0, Resource.getresponse.Length);
            m.Flush();
            m.Position = 0;
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(m);
            Assert.AreEqual(2, messages.Count);
        }
    }
}
#pragma warning restore 1591