using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Browser
{
    class SecureAgentProfile : AgentProfile
    {
        public SecureAgentProfile(Guid id, VersionCode version, IPEndPoint agent, string agentName, string authenticationPassphrase, string privacyPassphrase, int authenticationMethod, int privacyMethod, string userName)
            : base(id, version, agent, agentName, userName)
        {
            AuthenticationPassphrase = authenticationPassphrase;
            PrivacyPassphrase = privacyPassphrase;
            AuthenticationMethod = authenticationMethod;
            PrivacyMethod = privacyMethod;
        }

        internal string AuthenticationPassphrase { get; set; }
        internal string PrivacyPassphrase { get; set; }
        internal int AuthenticationMethod { get; set; }
        internal int PrivacyMethod { get; set; }

        internal override void Get(Manager manager, Variable variable)
        {
            TraceSource source = new TraceSource("Browser");
            // source.TraceInformation(manager.GetSingle(Agent, GetCommunity, result).ToString(manager.Objects));
            if (string.IsNullOrEmpty(UserName))
            {
                source.TraceInformation("User name need to be specified for v3.");
                source.Flush();
                source.Close();
                return;
            }

            //IAuthenticationProvider auth = (level & Levels.Authentication) == Levels.Authentication
            //                                   ? GetAuthenticationProviderByName(authentication, authPhrase)
            //                                   : DefaultAuthenticationProvider.Instance;

            //IPrivacyProvider priv = (level & Levels.Privacy) == Levels.Privacy
            //                            ? new DESPrivacyProvider(new OctetString(privPhrase), auth)
            //                            : DefaultPrivacyProvider.Instance;

            //Discovery discovery = new Discovery(1, 101);
            //ReportMessage report = discovery.GetResponse(timeout, receiver);

            //ProviderPair record = new ProviderPair(auth, priv);
            //GetRequestMessage request = new GetRequestMessage(VersionCode.V3, 100, 0, new OctetString(user), vList, record, report);

            //ISnmpMessage response = request.GetResponse(timeout, receiver);
            //if (response.Pdu.ErrorStatus.ToInt32() != 0) // != ErrorCode.NoError
            //{
            //    throw ErrorException.Create(
            //        "error in response",
            //        receiver.Address,
            //        response);
            //}

            //foreach (Variable v in response.Pdu.Variables)
            //{
            //    Console.WriteLine(v);
            //}
            source.Flush();
            source.Close();
        }

        internal override string GetValue(Manager manager, Variable variable)
        {
            // Variable result = manager.Objects.CreateVariable(textual);
            return null; // manager.GetSingle(Agent, GetCommunity, result).Data.ToString();
        }

        internal override void GetNext(Manager manager, Variable variable)
        {
            // Variable result = manager.Objects.CreateVariable(textual);
            TraceSource source = new TraceSource("Browser");
            //GetNextRequestMessage message = new GetNextRequestMessage(Messenger.NextRequestId, VersionCode, new OctetString(GetCommunity),
            //                                                          new List<Variable> {result});
            //ISnmpMessage response = message.GetResponse(manager.Timeout, Agent);
            //if (response.Pdu.ErrorStatus.ToInt32() != 0)
            //{
            //    throw ErrorException.Create(
            //        "error in response",
            //        Agent.Address,
            //        response);
            //}

            //source.TraceInformation(response.Pdu.Variables[0].ToString(manager.Objects));
            source.Flush();
            source.Close();
        }

        internal override void Set(Manager manager, Variable variable)
        {
            TraceSource source = new TraceSource("Browser");
            // source.TraceInformation(manager.SetSingle(Agent, GetCommunity, manager.Objects.CreateVariable(textual, data)).ToString(manager.Objects));
            source.Flush();
            source.Close();
        }

        internal override void GetTable(Manager manager, IDefinition def)
        {
            //IList<Variable> list = new List<Variable>();
            //int rows = Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity), new ObjectIdentifier(def.GetNumericalForm()), list, manager.Timeout, WalkMode.WithinSubtree);
			
            //// 
            //// How many rows are there?
            ////
            //if (rows > 0)
            //{
            //    FormTable newTable = new FormTable(def);
            //    newTable.SetRows(rows);
            //    newTable.PopulateGrid(list);
            //    newTable.Show();
            //}
            //else
            //{
            //    TraceSource source = new TraceSource("Browser");
            //    foreach (Variable t in list)
            //    {
            //        source.TraceInformation(t.ToString());
            //    }

            //    source.Flush();
            //    source.Close();
            //}
        }

        public override void Walk(Manager manager, IDefinition definition)
        {
            TraceSource source = new TraceSource("Browser");
            IList<Variable> list = new List<Variable>();
            //if (VersionCode == VersionCode.V1)
            //{
            //    Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity),
            //                   new ObjectIdentifier(definition.GetNumericalForm()), list, manager.Timeout,
            //                   WalkMode.WithinSubtree);
            //}
            //else
            //{
            //    Messenger.BulkWalk(VersionCode, Agent, new OctetString(GetCommunity),
            //                       new ObjectIdentifier(definition.GetNumericalForm()), list, manager.Timeout, 10,
            //                       WalkMode.WithinSubtree);
            //}

            //foreach (Variable v in list)
            //{
            //    source.TraceInformation(v.ToString(manager.Objects));
            //}

            source.Flush();
            source.Close();
        }
    }
}
