/*
 * Created by SharpDevelop.
 * User: Lex
 * Date: 8/4/2012
 * Time: 9:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Pipeline
{
    /// <summary>
    /// Description of EngineGroupTestFixture.
    /// </summary>
    public class EngineGroupTestFixture
    {
        [Fact]
        public void TestIsInTime()
        {
            Assert.True(EngineGroup.IsInTime(new[] { 0, 0 }, 0, -4));
            Assert.False(EngineGroup.IsInTime(new[] { 0, 0 }, 0, -151));
            Assert.True(EngineGroup.IsInTime(new[] { 0, 0 }, 0, 4));
            Assert.False(EngineGroup.IsInTime(new[] { 0, 0 }, 0, 151));

            Assert.True(EngineGroup.IsInTime(new[] { 0, int.MinValue + 1 }, 0, int.MaxValue - 1));
            Assert.False(EngineGroup.IsInTime(new[] { 0, int.MinValue + 152 }, 0, int.MaxValue));
            Assert.True(EngineGroup.IsInTime(new[] { 0, int.MaxValue - 1 }, 0, int.MinValue + 1));
            Assert.False(EngineGroup.IsInTime(new[] { 0, int.MaxValue }, 0, int.MinValue + 152));
        }
    }
}
