using System.Globalization;
using System.Net;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Client;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security.Privacy;
using DotNetSnmp.Transport;
using DotNetSnmp.Transport.Targets;
using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Messenger class contains all static helper methods you need to send out SNMP messages.
/// Static methods in Manager or Agent class will be removed in the future.
/// </summary>
public static partial class Messenger
{
    private static readonly Lazy<NumberGenerator> RequestCounterFullRange = new(() => new NumberGenerator(int.MinValue, int.MaxValue));
    private static readonly Lazy<NumberGenerator> RequestCounterPositive = new(() => new NumberGenerator(0, int.MaxValue));
    private static NumberGenerator? _requestCounter;

    private static readonly NumberGenerator MessageCounter = new(0, int.MaxValue);
    private static readonly ObjectIdentifier IdUnsupportedSecurityLevel = new("1.3.6.1.6.3.15.1.1.1.0");
    private static readonly ObjectIdentifier IdNotInTimeWindow = new("1.3.6.1.6.3.15.1.1.2.0");
    private static readonly ObjectIdentifier IdUnknownSecurityName = new("1.3.6.1.6.3.15.1.1.3.0");
    private static readonly ObjectIdentifier IdUnknownEngineId = new("1.3.6.1.6.3.15.1.1.4.0");
    private static readonly ObjectIdentifier IdAuthenticationFailure = new("1.3.6.1.6.3.15.1.1.5.0");
    private static readonly ObjectIdentifier IdDecryptionError = new("1.3.6.1.6.3.15.1.1.6.0");

    /// <summary>
    /// The universal counter for request IDs and other IDs.
    /// </summary>
    public static NumberGenerator RequestCounter
    {
        get
        {
            return _requestCounter ??= UseFullRange ? RequestCounterFullRange.Value : RequestCounterPositive.Value;
        }
        set
        {
            _requestCounter = value;
        }
    }

    /// <summary>
    /// A flag to control request ID range.
    /// </summary>
    public static bool UseFullRange { get; set; } = true;

    /// <summary>
    /// Represents next Id.
    /// </summary>
    public static int NextRequestId => RequestCounter.NextId;

    /// <summary>
    /// Represents next Id.
    /// </summary>
    public static int NextMessageId => MessageCounter.NextId;

    /// <summary>
    /// Max message size used in #SNMP. 
    /// </summary>
    public static int MaxMessageSize { get; set; } = 0xFFE3;

    /// <summary>
    /// If the privacy module returns failure, then the message can
    /// not be processed, so the usmStatsDecryptionErrors counter is
    /// incremented and an error indication (decryptionError) together
    /// with the OID and value of the incremented counter is returned
    /// to the calling module.
    /// </summary>
    public static ObjectIdentifier DecryptionError => IdDecryptionError;

    /// <summary>
    /// If the authentication module returns failure, then the message
    /// cannot be trusted, so the usmStatsWrongDigests counter is
    /// incremented and an error indication (authenticationFailure)
    /// together with the OID and value of the incremented counter is
    /// returned to the calling module.
    /// </summary>
    public static ObjectIdentifier AuthenticationFailure => IdAuthenticationFailure;

    /// <summary>
    /// If the value of the msgAuthoritativeEngineID field in the
    /// securityParameters is unknown then
    /// the usmStatsUnknownEngineIDs counter is incremented, and an
    /// error indication (unknownEngineID) together with the OID and
    /// value of the incremented counter is returned to the calling
    /// module.
    /// </summary>
    public static ObjectIdentifier UnknownEngineId => IdUnknownEngineId;

    /// <summary>
    /// Information about the value of the msgUserName and
    /// msgAuthoritativeEngineID fields is extracted from the Local
    /// Configuration Datastore (LCD, usmUserTable).  If no information
    /// is available for the user, then the usmStatsUnknownUserNames
    /// counter is incremented and an error indication
    /// (unknownSecurityName) together with the OID and value of the
    /// incremented counter is returned to the calling module.
    /// </summary>
    public static ObjectIdentifier UnknownSecurityName => IdUnknownSecurityName;

    /// <summary>
    /// If the message is considered to be outside of the Time Window
    /// then the usmStatsNotInTimeWindows counter is incremented and
    /// an error indication (notInTimeWindow) together with the OID,
    /// the value of the incremented counter, and an indication that
    /// the error must be reported with a securityLevel of authNoPriv,
    /// is returned to the calling module
    /// </summary>
    public static ObjectIdentifier NotInTimeWindow => IdNotInTimeWindow;

    /// <summary>
    /// If the information about the user indicates that it does not
    /// support the securityLevel requested by the caller, then the
    /// usmStatsUnsupportedSecLevels counter is incremented and an error
    /// indication (unsupportedSecurityLevel) together with the OID and
    /// value of the incremented counter is returned to the calling
    /// module.
    /// </summary>
    public static ObjectIdentifier UnsupportedSecurityLevel => IdUnsupportedSecurityLevel;

    /// <summary>
    /// Returns a new discovery request.
    /// </summary>
    public static Discovery GetNextDiscovery(SnmpType type)
    {
        return new Discovery(NextMessageId, NextRequestId, MaxMessageSize, type);
    }

    /// <summary>
    /// Returns error message for the specific <see cref="ObjectIdentifier"/>.
    /// </summary>
    public static string GetErrorMessage(this ObjectIdentifier id)
    {
        if (id == IdUnsupportedSecurityLevel)
        {
            return "unsupported security level";
        }

        if (id == IdNotInTimeWindow)
        {
            return "not in time window";
        }

        if (id == IdUnknownSecurityName)
        {
            return "unknown security name";
        }

        if (id == IdUnknownEngineId)
        {
            return "unknown engine ID";
        }

        if (id == IdAuthenticationFailure)
        {
            return "authentication failure";
        }

        if (id == IdDecryptionError)
        {
            return "decryption error";
        }

        return "unknown error";
    }

    /// <summary>
    /// Performs asynchronous retrieval.
    /// </summary>
    public static async Task<IList<Variable>> GetAsync(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables)
    {
        var dispatcher = new SnmpDispatcher();
        ISnmpTarget target;

        if (version == VersionCode.V3)
        {
            throw new ArgumentException("V3 requests require a username and security parameters. Use GetV3Async instead.");
        }
        else
        {
            target = new CommunityTarget(version, community);
        }

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            target,
            endpoint,
            new GetRequestPdu()
            {
                VariableBindings = new VarBindList(variables.ToArray())
            }
        ) as ResponsePdu;

        return response!.VariableBindings!.ToList();
    }

    /// <summary>
    /// Gets v3 Async.
    /// </summary>
    public static Task<IList<Variable>> GetV3Async(IPEndPoint endpoint, string username, IList<Variable> variables)
    {
        return GetV3Async(endpoint, username, new DefaultPrivacyProvider(), variables);
    }


    /// <summary>
    /// Gets v3 Async.
    /// </summary>
    public static async Task<IList<Variable>> GetV3Async(IPEndPoint endpoint, string username, IPrivacyProvider privacyProvider, IList<Variable> variables, string contextName = "")
    {
        var dispatcher = new SnmpDispatcher();

        var target = new UserTarget(new OctetString(username), privacyProvider);

        var scopedPdu = new Scope
        {
            ContextName = contextName,
            Pdu = new GetRequestPdu
            {
                VariableBindings = new VarBindList(variables.ToArray())
            }
        };

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            target,
            endpoint,
            scopedPdu
        ) as Scope;

        return response!.VariableBindings!.ToList();
    }

    /// <summary>
    /// Performs asynchronous update.
    /// </summary>
    public static async Task<IList<Variable>> SetAsync(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables)
    {
        if (version == VersionCode.V3)
        {
            throw new ArgumentException("V3 requests require a username and security parameters. Use SetV3Async instead.");
        }

        var dispatcher = new SnmpDispatcher();

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            new CommunityTarget(version, community),
            endpoint,
            new SetRequestPdu()
            {
                VariableBindings = new VarBindList(variables.ToArray())
            }
        ) as ResponsePdu;

        return response!.VariableBindings!.ToList();
    }

    /// <summary>
    /// Sets v3 Async.
    /// </summary>
    public static async Task<IList<Variable>> SetV3Async(IPEndPoint endpoint, string username, IPrivacyProvider privacyProvider,
        IList<Variable> variables, string contextName = "")
    {
        var dispatcher = new SnmpDispatcher();

        var target = new UserTarget(new OctetString(username), privacyProvider);

        var scopedPdu = new Scope
        {
            ContextName = contextName,
            Pdu = new SetRequestPdu
            {
                VariableBindings = new VarBindList(variables.ToArray())
            }
        };

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            target,
            endpoint,
            scopedPdu
        ) as Scope;

        return response!.VariableBindings!.ToList();
    }

    /// <summary>
    /// Performs an asynchronous walk operation.
    /// </summary>
    public static async Task<int> WalkAsync(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, WalkMode mode)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var result = 0;
        var tableV = new Variable(table);
        Variable seed;
        var next = tableV;
        var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
        var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
        Tuple<bool, Variable?> data = new(false, next);
        do
        {
            seed = data.Item2!.Value;

            if (version == VersionCode.V2 && seed.Data is EndOfMibView)
            {
                break;
            }

            if (seed == tableV)
            {
                data = await HasNextAsync(version, endpoint, community, seed).ConfigureAwait(false);
                continue;
            }

            if (mode == WalkMode.WithinSubtree && !seed.Id.ToString().StartsWith(subTreeMask, StringComparison.Ordinal))
            {
                // not in sub tree
                break;
            }

            list.Add(seed);
            if (seed.Id.ToString().StartsWith(rowMask, StringComparison.Ordinal))
            {
                result++;
            }

            data = await HasNextAsync(version, endpoint, community, seed).ConfigureAwait(false);
        }
        while (data.Item1);
        return result;
    }

    private static async Task<Tuple<bool, Variable?>> HasNextAsync(VersionCode version, IPEndPoint endpoint, OctetString community, Variable seed)
    {
        var variables = new List<Variable> { new(seed.Id) };
        var dispatcher = new SnmpDispatcher();

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            new CommunityTarget(version, community),
            endpoint,
            new GetNextRequestPdu()
            {
                VariableBindings = new VarBindList(variables.ToArray())
            }
        ) as ResponsePdu;

        var errorFound = response!.ErrorStatus == ErrorCode.NoSuchName;
        return new Tuple<bool, Variable?>(!errorFound, errorFound ? null : response.VariableBindings!.ToArray()[0]);
    }

    /// <summary>
    /// Performs an asynchronous bulk walk operation.
    /// </summary>
    public static async Task<int> BulkWalkAsync(VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, ObjectIdentifier table, IList<Variable> list, int maxRepetitions, WalkMode mode)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var tableV = new Variable(table);
        var seed = tableV;
        var result = 0;
        var data = await BulkHasNextAsync(version, endpoint, community, contextName, seed, maxRepetitions).ConfigureAwait(false);
        var next = data.Item2;
        while (data.Item1)
        {
            var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
            var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);
            foreach (var v in next)
            {
                var id = v.Id.ToString();
                if (v.Data is EndOfMibView)
                {
                    goto end;
                }

                if (mode == WalkMode.WithinSubtree && !id.StartsWith(subTreeMask, StringComparison.Ordinal))
                {
                    // not in sub tree
                    goto end;
                }

                list.Add(v);
                if (id.StartsWith(rowMask, StringComparison.Ordinal))
                {
                    result++;
                }
            }

            seed = next[next.Count - 1];
            data = await BulkHasNextAsync(version, endpoint, community, contextName, seed, maxRepetitions).ConfigureAwait(false);
            next = data.Item2;
        }

    end:
        return result;
    }

    private static async Task<Tuple<bool, IList<Variable>, Pdu?>> BulkHasNextAsync(VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, Variable seed, int maxRepetitions)
    {
        // TODO: report should be updated with latest message from agent.
        if (version == VersionCode.V1 || version == VersionCode.V3)
        {
            throw new NotSupportedException("SNMP v1 and v3 are not supported");
        }

        var variables = new List<Variable> { new(seed.Id) };
        var dispatcher = new SnmpDispatcher();

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            new CommunityTarget(version, community),
            endpoint,
            new GetBulkRequestPdu()
            {
                VariableBindings = new VarBindList(variables.ToArray()),
                NonRepeaters = 0,
                MaxRepetitions = maxRepetitions
            }
        ) as ResponsePdu;

        var next = response!.VariableBindings;
        return new Tuple<bool, IList<Variable>, Pdu?>(!next!.IsEmpty, next.ToArray(), null);
    }

    /// <summary>
    /// Performs an asynchronous bulk walk operation.
    /// </summary>
    public static Task<int> BulkWalkAsync(
        VersionCode version,
        IPEndPoint endpoint,
        OctetString community,
        OctetString contextName,
        ObjectIdentifier table,
        IList<Variable> list,
        int maxRepetitions,
        WalkMode mode,
        IPrivacyProvider privacy,
        ISnmpMessage report)
    {
        return BulkWalkAsync(version, endpoint, community, contextName, table, list, maxRepetitions, mode);
    }

    /// <summary>
    /// Retrieves data.
    /// </summary>
    public static IList<Variable> Get(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
    {
        return ExecuteWithTimeout(() => GetAsync(version, endpoint, community, variables), timeout);
    }

    /// <summary>
    /// Sends updated data.
    /// </summary>
    public static IList<Variable> Set(VersionCode version, IPEndPoint endpoint, OctetString community, IList<Variable> variables, int timeout)
    {
        return ExecuteWithTimeout(() => SetAsync(version, endpoint, community, variables), timeout);
    }

    /// <summary>
    /// Performs a walk operation.
    /// </summary>
    public static int Walk(VersionCode version, IPEndPoint endpoint, OctetString community, ObjectIdentifier table, IList<Variable> list, int timeout, WalkMode mode)
    {
        return ExecuteWithTimeout(() => WalkAsync(version, endpoint, community, table, list, mode), timeout);
    }

    /// <summary>
    /// Performs a bulk walk operation.
    /// </summary>
    public static int BulkWalk(
        VersionCode version,
        IPEndPoint endpoint,
        OctetString community,
        OctetString contextName,
        ObjectIdentifier table,
        IList<Variable> list,
        int timeout,
        int maxRepetitions,
        WalkMode mode,
        IPrivacyProvider? privacy,
        ISnmpMessage? report)
    {
        return ExecuteWithTimeout(
            () => BulkWalkAsync(version, endpoint, community, contextName, table, list, maxRepetitions, mode),
            timeout);
    }

    /// <summary>
    /// Gets table.
    /// </summary>
    [Obsolete("This method only works for a few scenarios. Might be replaced by new methods in the future. If it does not work for you, parse WALK result on your own.")]
    public static Variable[,] GetTable(
        VersionCode version,
        IPEndPoint endpoint,
        OctetString community,
        ObjectIdentifier table,
        int timeout,
        int maxRepetitions)
    {
        if (version == VersionCode.V3)
        {
            throw new NotSupportedException("SNMP v3 is not supported");
        }

        IList<Variable> list = new List<Variable>();
        var rows = version == VersionCode.V1
            ? Walk(version, endpoint, community, table, list, timeout, WalkMode.WithinSubtree)
            : BulkWalk(version, endpoint, community, OctetString.Empty, table, list, timeout, maxRepetitions, WalkMode.WithinSubtree, null, null);

        if (rows == 0)
        {
            return new Variable[0, 0];
        }

        var cols = list.Count / rows;
        var k = 0;
        var result = new Variable[rows, cols];
        for (var j = 0; j < cols; j++)
        {
            for (var i = 0; i < rows; i++)
            {
                result[i, j] = list[k];
                k++;
            }
        }

        return result;
    }

    /// <summary>
    /// Sends inform Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static async Task SendInformAsync(int requestId, VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
    {
        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        var pdu = new InformRequestPdu
        {
            RequestId = requestId,
            Enterprise = enterprise,
            TimeStamp = timestamp,
            VariableBindings = new VarBindList(variables.ToArray())
        };

        var dispatcher = new SnmpDispatcher();
        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            new CommunityTarget(version, community),
            endpoint,
            pdu
        ) as ResponsePdu;

        if (response!.ErrorStatus != ErrorCode.NoError)
        {
            throw new InvalidOperationException($"Error in response: {response.ErrorStatus}");
        }
    }

    /// <summary>
    /// Sends inform Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static Task SendInformAsync(
        int requestId,
        VersionCode version,
        IPEndPoint endpoint,
        OctetString community,
        OctetString contextName,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        ISnmpMessage report)
    {
        return SendInformAsync(requestId, version, endpoint, community, contextName, enterprise, timestamp, variables);
    }

    /// <summary>
    /// Sends trap V1 Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static async Task SendTrapV1Async(
        EndPoint receiver,
        IPAddress agent,
        OctetString community,
        ObjectIdentifier enterprise,
        GenericCode generic,
        int specific,
        uint timestamp,
        IList<Variable> variables)
    {
        if (receiver is not IPEndPoint endpoint)
        {
            throw new ArgumentException("receiver must be an IPEndPoint instance.", nameof(receiver));
        }

        if (agent == null)
        {
            throw new ArgumentNullException(nameof(agent));
        }

        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        var pdu = new TrapPdu
        {
            Enterprise = enterprise,
            AgentAddress = agent,
            GenericTrap = (int)generic,
            SpecificTrap = specific,
            TimeStamp = timestamp,
            VariableBindings = new VarBindList(variables.ToArray())
        };

        var dispatcher = new SnmpDispatcher();
        await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            new CommunityTarget(VersionCode.V1, community),
            endpoint,
            pdu,
            false).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends trap V1.
    /// </summary>
    [System.CLSCompliant(false)]
    public static void SendTrapV1(
        EndPoint receiver,
        IPAddress agent,
        OctetString community,
        ObjectIdentifier enterprise,
        GenericCode generic,
        int specific,
        uint timestamp,
        IList<Variable> variables)
    {
        SendTrapV1Async(receiver, agent, community, enterprise, generic, specific, timestamp, variables)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Sends trap V2 Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static async Task SendTrapV2Async(int requestId, VersionCode version, IPEndPoint endpoint, OctetString community, OctetString contextName, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
    {
        if (version != VersionCode.V2)
        {
            throw new NotSupportedException("Only SNMP v2c is supported");
        }

        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        var pdu = new TrapV2Pdu
        {
            RequestId = requestId,
            Enterprise = enterprise,
            TimeStamp = timestamp,
            VariableBindings = new VarBindList(variables.ToArray())
        };

        var dispatcher = new SnmpDispatcher();
        await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            new CommunityTarget(version, community),
            endpoint,
            pdu,
            false
        );
    }

    /// <summary>
    /// Sends trap V2 Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static Task SendTrapV2Async(int requestId, VersionCode version, EndPoint receiver, OctetString community, ObjectIdentifier enterprise, uint timestamp, IList<Variable> variables)
    {
        if (receiver is not IPEndPoint endpoint)
        {
            throw new ArgumentException("receiver must be an IPEndPoint instance.", nameof(receiver));
        }

        return SendTrapV2Async(requestId, version, endpoint, community, OctetString.Empty, enterprise, timestamp, variables);
    }

    /// <summary>
    /// Sends inform.
    /// </summary>
    [System.CLSCompliant(false)]
    public static void SendInform(
        int requestId,
        VersionCode version,
        IPEndPoint receiver,
        OctetString community,
        OctetString contextName,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables,
        int timeout,
        IPrivacyProvider privacy,
        ISnmpMessage report)
    {
        ExecuteWithTimeout(
            () => SendInformAsync(requestId, version, receiver, community, contextName, enterprise, timestamp, variables, privacy, report),
            timeout);
    }

    /// <summary>
    /// Sends trap V2.
    /// </summary>
    [System.CLSCompliant(false)]
    public static void SendTrapV2(
        int requestId,
        VersionCode version,
        EndPoint receiver,
        OctetString community,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables)
    {
        if (receiver is not IPEndPoint endpoint)
        {
            throw new ArgumentException("receiver must be an IPEndPoint instance.", nameof(receiver));
        }

        SendTrapV2Async(requestId, version, endpoint, community, OctetString.Empty, enterprise, timestamp, variables)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Walks a subtree with GetBulk requests over SNMPv3 and appends results to <paramref name="list"/>.
    /// </summary>
    public static async Task<int> BulkWalkV3Async(
        IPEndPoint endpoint,
        string username,
        IPrivacyProvider privacyProvider,
        ObjectIdentifier table,
        IList<Variable> list,
        int maxRepetitions,
        WalkMode mode,
        string contextName = "")
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var dispatcher = new SnmpDispatcher();
        var target = new UserTarget(new OctetString(username), privacyProvider);

        var tableV = new Variable(table);
        var seed = tableV;
        var result = 0;

        // Initial request - get first set of results using GetBulkRequest
        var initialPdu = new Scope
        {
            ContextName = contextName,
            Pdu = new GetBulkRequestPdu
            {
                NonRepeaters = 0,
                MaxRepetitions = maxRepetitions,
                VariableBindings = new VarBindList(seed)
            }
        };

        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            target,
            endpoint,
            initialPdu
        ) as Scope;

        var next = response!.VariableBindings!.ToArray();
        var moreData = next.Length > 0;

        while (moreData && next.Length > 0)
        {
            var subTreeMask = string.Format(CultureInfo.InvariantCulture, "{0}.", table);
            var rowMask = string.Format(CultureInfo.InvariantCulture, "{0}.1.1.", table);

            // Process all variables in the current response
            var continueWalk = true;
            foreach (var v in next)
            {
                var id = v.Id.ToString();

                // Stop if we reach end of MIB view
                if (v.Data is EndOfMibView)
                {
                    continueWalk = false;
                    break;
                }

                // Check if we're still within the subtree if that mode is requested
                if (mode == WalkMode.WithinSubtree && !id.StartsWith(subTreeMask, StringComparison.Ordinal))
                {
                    // not in sub tree
                    continueWalk = false;
                    break;
                }

                list.Add(v);
                if (id.StartsWith(rowMask, StringComparison.Ordinal))
                {
                    result++;
                }
            }

            if (!continueWalk || next.Length == 0)
            {
                break;
            }

            // Use the last variable from current response as seed for next request
            seed = next[next.Length - 1];

            // Get next batch of variables
            var bulkPdu = new Scope
            {
                ContextName = contextName,
                Pdu = new GetBulkRequestPdu
                {
                    NonRepeaters = 0,
                    MaxRepetitions = maxRepetitions,
                    VariableBindings = new VarBindList(seed)
                }
            };

            response = await dispatcher.SendPdu(
                new BasicUdpTransport(endpoint),
                target,
                endpoint,
                bulkPdu
            ) as Scope;

            next = response!.VariableBindings!.ToArray();
            moreData = next.Length > 0;
        }

        return result;
    }

    /// <summary>
    /// Sends inform V3 Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static async Task SendInformV3Async(
        IPEndPoint endpoint,
        string username,
        IPrivacyProvider privacyProvider,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables,
        string contextName = "")
    {
        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        var dispatcher = new SnmpDispatcher();
        var target = new UserTarget(
            new OctetString(username), privacyProvider);

        // Create inform PDU with the proper structure
        var informPdu = new InformRequestPdu
        {
            RequestId = Random.Shared.Next(),
            Enterprise = enterprise,
            TimeStamp = timestamp,
            VariableBindings = new VarBindList(variables.ToArray())
        };

        // Wrap in a scoped PDU for SNMPv3
        var scopedPdu = new Scope
        {
            ContextName = contextName,
            Pdu = informPdu
        };

        // Send and await the response
        var response = await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            target,
            endpoint,
            scopedPdu
        ) as Scope;

        // Process response
        var responsePdu = response!.Pdu as ResponsePdu;
        if (responsePdu!.ErrorStatus != ErrorCode.NoError)
        {
            throw new InvalidOperationException($"Error in response: {responsePdu.ErrorStatus}");
        }
    }

    /// <summary>
    /// Sends trap V2 V3 Async.
    /// </summary>
    [System.CLSCompliant(false)]
    public static async Task SendTrapV2V3Async(
        IPEndPoint endpoint,
        string username,
        IPrivacyProvider privacyProvider,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables,
        string contextName = "")
    {
        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        var dispatcher = new SnmpDispatcher();
        var target = new UserTarget(new OctetString(username), privacyProvider);

        var scopedPdu = new Scope
        {
            ContextName = contextName,
            Pdu = new TrapV2Pdu
            {
                RequestId = Random.Shared.Next(),
                Enterprise = enterprise,
                TimeStamp = timestamp,
                VariableBindings = new VarBindList(variables.ToArray())
            }
        };

        await dispatcher.SendPdu(
            new BasicUdpTransport(endpoint),
            target,
            endpoint,
            scopedPdu,
            false // Don't expect a response for TRAPv2
        );
    }

    private static T ExecuteWithTimeout<T>(Func<Task<T>> action, int timeout)
    {
        var timeoutSpan = ToTimeout(timeout);
        try
        {
            return action().WaitAsync(timeoutSpan).GetAwaiter().GetResult();
        }
        catch (TaskCanceledException ex) when (timeout != Timeout.Infinite)
        {
            throw new TimeoutException($"Operation timed out after {timeout} milliseconds.", ex);
        }
    }

    private static void ExecuteWithTimeout(Func<Task> action, int timeout)
    {
        var timeoutSpan = ToTimeout(timeout);
        try
        {
            action().WaitAsync(timeoutSpan).GetAwaiter().GetResult();
        }
        catch (TaskCanceledException ex) when (timeout != Timeout.Infinite)
        {
            throw new TimeoutException($"Operation timed out after {timeout} milliseconds.", ex);
        }
    }

    private static TimeSpan ToTimeout(int timeout)
    {
        if (timeout < Timeout.Infinite)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        return timeout == Timeout.Infinite ? Timeout.InfiniteTimeSpan : TimeSpan.FromMilliseconds(timeout);
    }
}
