using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Browser
{
    internal class NormalAgentProfile : AgentProfile
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger("Lextm.SharpSnmpLib.Browser");

        public NormalAgentProfile(Guid id, VersionCode version, IPEndPoint agent, string getCommunity, string setCommunity, string agentName, string userName)
            : base(id, version, agent, agentName, userName)
        {
            GetCommunity = getCommunity;
            SetCommunity = setCommunity;
        }

        internal string GetCommunity { get; set; }
        internal string SetCommunity { get; set; }

        internal override void Get(Manager manager, Variable variable)
        {
            Logger.Info(manager.GetSingle(Agent, GetCommunity, variable).ToString(manager.Objects));
        }

        internal override string GetValue(Manager manager, Variable variable)
        {
            return manager.GetSingle(Agent, GetCommunity, variable).Data.ToString();
        }

        internal override void GetNext(Manager manager, Variable variable)
        {
            GetNextRequestMessage message = new GetNextRequestMessage(Messenger.NextRequestId, VersionCode, new OctetString(GetCommunity),
                                                                      new List<Variable> {variable});
            ISnmpMessage response = message.GetResponse(manager.Timeout, Agent);
            if (response.Pdu.ErrorStatus.ToInt32() != 0)
            {
                throw ErrorException.Create(
                    "error in response",
                    Agent.Address,
                    response);
            }

            Logger.Info(response.Pdu.Variables[0].ToString(manager.Objects));
        }

        internal override void Set(Manager manager, Variable variable)
        {
            Logger.Info(manager.SetSingle(Agent, SetCommunity, variable).ToString(manager.Objects));
        }

        internal override void GetTable(Manager manager, IDefinition def)
        {
            IList<Variable> list = new List<Variable>();
            int rows = Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity), new ObjectIdentifier(def.GetNumericalForm()), list, manager.Timeout, WalkMode.WithinSubtree);
			
            // 
            // How many rows are there?
            //
            if (rows > 0)
            {
                FormTable newTable = new FormTable(def);
                newTable.SetRows(rows);
                newTable.PopulateGrid(list);
                newTable.Show();
            }
            else
            {
                foreach (Variable t in list)
                {
                    Logger.Info(t.ToString());
                }
            }
        }

        public override void Walk(Manager manager, IDefinition definition)
        {
            IList<Variable> list = new List<Variable>();
            if (VersionCode == VersionCode.V1)
            {
                Messenger.Walk(VersionCode, Agent, new OctetString(GetCommunity),
                               new ObjectIdentifier(definition.GetNumericalForm()), list, manager.Timeout,
                               WalkMode.WithinSubtree);
            }
            else
            {
                Messenger.BulkWalk(VersionCode, Agent, new OctetString(GetCommunity),
                                   new ObjectIdentifier(definition.GetNumericalForm()), list, manager.Timeout, 10,
                                   WalkMode.WithinSubtree);
            }

            foreach (Variable v in list)
            {
                Logger.Info(v.ToString(manager.Objects));
            }
        }
    }
}