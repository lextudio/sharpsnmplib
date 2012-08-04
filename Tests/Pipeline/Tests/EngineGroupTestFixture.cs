/*
 * Created by SharpDevelop.
 * User: Lex
 * Date: 8/4/2012
 * Time: 9:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
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
			Assert.IsTrue(EngineGroup.IsInTime(0, -499));
			Assert.IsFalse(EngineGroup.IsInTime(0, -501));
			
			Assert.IsTrue(EngineGroup.IsInTime(Int32.MinValue + 1, Int32.MaxValue - 1));
			Assert.IsFalse(EngineGroup.IsInTime(Int32.MinValue + 502, Int32.MaxValue));
		}
	}
}
