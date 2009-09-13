/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 10:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.IO;
using NUnit.Framework;
#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestByteTool
    {
        [Test]
        public void TestConvertDecimal()
        {
            byte[] b = ByteTool.ConvertDecimal(" 16 18 ");
            Assert.AreEqual(new byte[] { 0x10, 0x12 }, b);
        }

        [Test]
        public void TestReadShortLength()
        {
            MemoryStream m = new MemoryStream();
            m.WriteByte(0x66);
            m.Flush();
            m.Position = 0;
            Assert.AreEqual(102, ByteTool.ReadPayloadLength(m));
        }
        
        [Test]
        public void TestWriteShortLength()
        {
            int length = 102;
            byte expect = 0x66;
            MemoryStream m = new MemoryStream();
            ByteTool.WritePayloadLength(m, length);
            byte[] array = m.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(expect, array[0]);
        }
        
        [Test]
        public void TestReadLongLength()
        {
            byte[] expected = new byte[] {0x83, 0x73, 0x59, 0xB5};
            MemoryStream m = new MemoryStream();
            m.Write(expected, 0, 4);
            m.Flush();
            m.Position = 0;
            Assert.AreEqual(7559605, ByteTool.ReadPayloadLength(m));
        }
        
        [Test]
        public void TestWriteLongLength()
        {
            int length = 7559605;
            byte[] expected = new byte[] {0x83, 0x73, 0x59, 0xB5};
            MemoryStream m = new MemoryStream();
            ByteTool.WritePayloadLength(m, length);
            Assert.AreEqual(expected, m.ToArray());
        }
    }
}
#pragma warning restore 1591, 0618