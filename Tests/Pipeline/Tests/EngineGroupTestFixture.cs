/*
 * Created by SharpDevelop.
 * User: Lex
 * Date: 8/4/2012
 * Time: 9:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Pipeline.Tests
{
    /// <summary>
    /// Description of EngineGroupTestFixture.
    /// </summary>
    [TestFixture]
    public class EngineGroupTestFixture
    {
        [Test]
        public void TestIsInTime()
        {
            Assert.IsTrue(EngineGroup.IsInTime(new[] { 0, 0 }, -499, 0));
            Assert.IsFalse(EngineGroup.IsInTime(new[] { 0, 0 }, -501, 0));
            
            Assert.IsTrue(EngineGroup.IsInTime(new[] { int.MinValue + 1, 0 }, int.MaxValue - 1, 0));
            Assert.IsFalse(EngineGroup.IsInTime(new[] { int.MinValue + 502, 0}, int.MaxValue, 0));
        }
    }
}
