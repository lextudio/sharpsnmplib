using System;
using System.Collections.Generic;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP version 3 membership provider. Not yet implemented.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class Version3MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V3;
        private readonly OctetString _get;
        private readonly OctetString _set;
        private readonly OctetString _engineId = new OctetString(new byte[] { 4, 13, 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244, 73 });
        private uint _counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version3MembershipProvider"/> class.
        /// </summary>
        /// <param name="getCommunity">The get community.</param>
        /// <param name="setCommunity">The set community.</param>
        public Version3MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
            // TODO: implement v3 checking.
            _get = getCommunity;
            _set = setCommunity;
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(SnmpContext context)
        {
            ISnmpMessage message = context.Request;
            if (message.Version != Version)
            {
                return false;
            }
            
            if (message.Parameters.UserName == OctetString.Empty)
            {
                _counter++;
                // discovery message received.
                context.Response = new ReportMessage(
                    VersionCode.V3,
                    new Header(
                        new Integer32(message.MessageId),
                        new Integer32(0xFFE3),
                        new OctetString(new[] {(byte) Levels.Reportable}),
                        new Integer32(3)),
                    new SecurityParameters(
                        _engineId,
                        new Integer32(0),
                        new Integer32(Environment.TickCount),
                        OctetString.Empty,
                        OctetString.Empty,
                        OctetString.Empty),
                    new Scope(
                        OctetString.Empty,
                        OctetString.Empty,
                        new ReportPdu(
                            message.RequestId,
                            ErrorCode.NoError,
                            0,
                            new List<Variable>(1)
                                {
                                    new Variable(
                                        new ObjectIdentifier("1.3.6.1.6.3.15.1.1.4.0"),
                                        new Counter32(_counter)
                                        )
                                })),
                    ProviderPair.Default);
                return true;                
            }

            if (context.Request.Pdu.TypeCode == SnmpType.SetRequestPdu)
            {
                return context.Request.Parameters.UserName == _set;
            }

            return context.Request.Parameters.UserName == _get;
        }
    }
}
