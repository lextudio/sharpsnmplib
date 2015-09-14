// $Header: $

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Pipeline.Tests
{
	[TestFixture]
	public class SecureSnmpContextTestFixture
	{
		/// <summary>
		/// Sets up objects for testing.
		/// </summary>
		/// <param name="trapMessage">Outputs the created trap message.</param>
		/// <param name="context">Outputs the created <see cref="SecureSnmpContext"/>.</param>
		private void setup1(out TrapV2Message trapMessage, out SecureSnmpContext context)
		{
			var privacyProvider = new DESPrivacyProvider(new OctetString("privacykey"), new MD5AuthenticationProvider(new OctetString("authenticationkey")));
			var users = new UserRegistry(new[] { new User(new OctetString("private"), privacyProvider) });
			var engineGroup = new EngineGroup();

			trapMessage = new TrapV2Message(
				VersionCode.V3,
				123,
				456,
				new OctetString("private"),
				new ObjectIdentifier(new byte[] { 1, 3, 6, 1, 1, 0 }),
				789,
				new List<Variable> { },
				privacyProvider,
				Messenger.MaxMessageSize,
				engineGroup.EngineId,
				engineGroup.EngineTimeData[0],
				engineGroup.EngineTimeData[1]);

			context = new SecureSnmpContext(trapMessage, new System.Net.IPEndPoint(IPAddress.Loopback, 162), users, engineGroup, null);
		}

		[Test]
		public void ReceivedSNMPv3TrapWithReportableFlagInMessageHeader_Should_BeProcessedIfReportableFlagNotSetInMessageHeader()
		{
			TrapV2Message trapMessage;
			SecureSnmpContext context;
			this.setup1(out trapMessage, out context);

			// Enforce Reportable flag.
			// TODO Replace string literal by nameof(...) as soon as switched to a more recent version of C#.
			typeof(Header).GetProperty("SecurityLevel", BindingFlags.Public | BindingFlags.Instance).SetValue(trapMessage.Header, trapMessage.Header.SecurityLevel | Levels.Reportable);

			// Accept either true as return value or a "not in time window" reponse.
			var result = context.HandleMembership();
			Assert.IsTrue(result);
			Assert.IsTrue(result || ((context.Response.Variables().Count == 1) && (context.Response.Variables().Single().Id == (new EngineGroup()).NotInTimeWindow.Id)));
		}

		[Test]
		public void ReceivedSNMPv3TrapWithoutReportableFlagInMessageHeader_Should_BeProcessedIfReportableFlagNotSetInMessageHeader()
		{
			TrapV2Message trapMessage;
			SecureSnmpContext context;
			this.setup1(out trapMessage, out context);

			Assert.IsTrue(context.HandleMembership());
		}
	}
}
