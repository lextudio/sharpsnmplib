/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 10:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    /// <summary>
    /// Description of TestInteger32.
    /// </summary>
    [TestFixture]
    public class TestInteger32
    {
        [Test]
        public void TestConstructor()
        {
            Integer32 test = new Integer32(100);
            Assert.AreEqual(100, test.ToInt32());
            
            Integer32 test2 = new Integer32(new byte[] {0x00});
            Assert.AreEqual(0, test2.ToInt32());
            
            Integer32 test3 = new Integer32(new byte[] {0xFF});
            Assert.AreEqual(-1, test3.ToInt32());
        }
        
        [Test]
        public void TestToInt32()
        {                     
            int result = -26955;
            byte[] expected = new byte[] {0x96, 0xB5};
            Integer32 test = new Integer32(expected);
            Assert.AreEqual(result, test.ToInt32());
            
            Assert.AreEqual(255, new Integer32(new byte[] {0x00, 0xFF}).ToInt32());
        }
        
        [Test]
        public void TestToBytes()
        {
            byte[] bytes = new byte[] {0x02, 0x02, 0x96, 0xB5};
            Integer32 test = new Integer32(-26955);
            Assert.AreEqual(bytes, test.ToBytes());
            
            Assert.AreEqual(new byte[] {0x02, 0x02, 0x00, 0xFF}, new Integer32(255).ToBytes());
            
            Assert.AreEqual(6, new Integer32(2147483647).ToBytes().Length);
        }
        
        [Test]
        public void TestToBytes2()
        {
            Integer32 i = new Integer32(-1);
            Assert.AreEqual(new byte[] {0x02, 0x01, 0xFF}, i.ToBytes());
        }
    }
}
#pragma warning restore 1591