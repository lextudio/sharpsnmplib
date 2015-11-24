/*
 * Created by SharpDevelop.
 * User: Lex
 * Date: 8/4/2012
 * Time: 9:32 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Linq;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Pipeline.Tests
{
	/// <summary>
	/// Description of EngineGroupTestFixture.
	/// </summary>
	[TestFixture]
	[Category("Default")]
	public class EngineGroupTestFixture
	{
		[Test]
		public void TestIsInTime()
		{
			Assert.IsTrue(EngineGroup.IsInTime(new[] { 0, 0 }, 0, -499));
			Assert.IsFalse(EngineGroup.IsInTime(new[] { 0, 0 }, 0, -150001));

			Assert.IsTrue(EngineGroup.IsInTime(new[] { 0, int.MinValue + 1, }, 0, int.MaxValue - 1));
			Assert.IsFalse(EngineGroup.IsInTime(new[] { 0, int.MinValue + 150002 }, 0, int.MaxValue));
		}

		[Test]
		public void InstantiateWithoutArguments_Should_ShowDefaultBehavior()
		{
			var eg = new EngineGroup();
			Assert.IsTrue(eg.EngineId.GetRaw().SequenceEqual(EngineGroup.EngineIdDefault.GetRaw()));
			Assert.IsTrue(eg.ContextName.Equals(OctetString.Empty));
			Assert.AreEqual(0, eg.EngineTimeData[0]);
		}

		[Test]
		public void InstantiateWithEngineBootsOnly_Should_UseEngineBootsButDefaultEngineId()
		{
			var eg = new EngineGroup(42);
			Assert.IsTrue(eg.EngineId.GetRaw().SequenceEqual(EngineGroup.EngineIdDefault.GetRaw()));
			Assert.AreEqual(OctetString.Empty, eg.ContextName);
			Assert.AreEqual(42, eg.EngineTimeData[0]);
		}

		[Test]
		public void InstantiateWithNegativeEngineBoots_Should_ThrowArgumentOutOfRangeException()
		{
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => { var eg = new EngineGroup(-1); });
		}

		[Test]
		public void InstantiateWithEngineIdAndEngineBoots_Should_UseValues()
		{
			var engineId = new byte[] { 0x80, 0x00, 0x1f, 0x88, 0x80, 0xaf, 0xbc, 0x29, 0x10, 0xfc, 0x64, 0x12, 0x56, 0x00, 0x00, 0x00, 0x00 };
			var eg = new EngineGroup(new OctetString(engineId), new OctetString("TheContext"), 42);
			Assert.IsTrue(eg.EngineId.GetRaw().SequenceEqual(engineId));
			Assert.AreEqual(new OctetString("TheContext"), eg.ContextName);
			Assert.AreEqual(42, eg.EngineTimeData[0]);
		}

		[Test]
		public void InstantiateWithTooShortEngineId_Should_ThrowArgumentException()
		{
			var engineId = new byte[] { 0x01, 0x02, 0x03, 0x04 };
			Assert.Throws(typeof(ArgumentException), () => { new EngineGroup(new OctetString(engineId), OctetString.Empty, 0); });
		}

		[Test]
		public void InstantiateWithTooLongEngineId_Should_ThrowArgumentException()
		{
			var engineId = new byte[] {
				0x80, 0x00, 0x1f, 0x88, 0x80, 0xaf, 0xbc, 0x29,
				0x10, 0xfc, 0x64, 0x12, 0x56, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00 };
			Assert.Throws(typeof(ArgumentException), () => { new EngineGroup(new OctetString(engineId), OctetString.Empty, 0); });
		}

		[Test]
		public void InstantiateWithEngineIdNull_Should_ThrowArgumentNullException()
		{
			Assert.Throws(typeof(ArgumentNullException), () => { new EngineGroup(null, OctetString.Empty, 0); });
		}

		[Test]
		public void InstantiateWithEngineIdNotBeginningWithOneBit_Should_ThrowArgumentException()
		{
			var engineId = new byte[] { 0x00, 0x00, 0x1f, 0x88, 0x80, 0xaf, 0xbc, 0x29, 0x10, 0xfc, 0x64, 0x12, 0x56, 0x00, 0x00, 0x00, 0x00 };
			Assert.Throws(typeof(ArgumentException), () => { new EngineGroup(new OctetString(engineId), OctetString.Empty, 0); });
		}

		[Test]
		public void InstantiateWithContextNameNull_Should_ThrowArgumentNullException()
		{
			var engineId = new byte[] { 0x80, 0x00, 0x1f, 0x88, 0x80, 0xaf, 0xbc, 0x29, 0x10, 0xfc, 0x64, 0x12, 0x56, 0x00, 0x00, 0x00, 0x00 };
			Assert.Throws(typeof(ArgumentNullException), () => { new EngineGroup(new OctetString(engineId), null, 0); });
		}
	}
}
