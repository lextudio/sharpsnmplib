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
        private readonly AgentObjects _objects;
        private readonly UserRegistry _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version3MembershipProvider"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="objects">The objects.</param>
        public Version3MembershipProvider(UserRegistry users, AgentObjects objects)
        {
            _users = users;
            _objects = objects;
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
                _objects.ReportCount++;
                // discovery message received.
                context.Response = new ReportMessage(
                    VersionCode.V3,
                    new Header(
                        new Integer32(message.MessageId),
                        new Integer32(0xFFE3),
                        new OctetString(new[] {(byte) Levels.Reportable}),
                        new Integer32(3)),
                    new SecurityParameters(
                        _objects.EngineId,
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
                                        new Counter32(_objects.ReportCount)
                                        )
                                })),
                    ProviderPair.Default);
                return true;                
            }

            if (message.Parameters.EngineId != _objects.EngineId)
            {
                // not from this engine.
                return false;
            }

            if (message.Parameters.EngineBoots.ToInt32() != _objects.EngineBoots)
            {
                // does not match boot count.
                return false;
            }

            if (message.Parameters.EngineTime.ToInt32() > _objects.EngineTime + 500)
            {
                // timeout.
                return false;
            }

            // TODO: verify authentication here.
            OctetString embedded = message.Parameters.AuthenticationParameters;
            ProviderPair providers = _users.Find(message.Parameters.UserName);
            message.Parameters.AuthenticationParameters = providers.Authentication.CleanDigest;
            OctetString calculated = providers.Authentication.ComputeHash(message);
            // other checking were performed in MessageFactory when decrypting message body.
            return embedded == calculated;
        }
    }
}
