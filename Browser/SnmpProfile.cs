using System;
using System.Collections.Generic;
using System.Text;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using System.Net;

namespace Browser
{
    delegate void ReportMessage(string message);

    class SnmpProfile
    {
        private Manager _manager;
        private string _get;
        private string _set;
        private VersionCode _version;
        private string _ip;
        private static SnmpProfile instance;

        private SnmpProfile(Manager manager, string getCommunity, string setCommunity, VersionCode version, string ip)
        {
            _manager = manager;
            _get = getCommunity;
            _set = setCommunity;
            _version = version;
            _ip = ip;
        }

        event ReportMessage OnOperationCompleted;

        internal static void Initiate(Manager manager, string getCommunity, string setCommunity, VersionCode version, string ip, ReportMessage handler)
        {
            lock (typeof(SnmpProfile))
            {
                if (instance == null)
                {
                    instance = new SnmpProfile(manager, getCommunity, setCommunity, version, ip);
                    instance.OnOperationCompleted += handler;
                }
            }            
        }

        internal static SnmpProfile Instance
        {
            get
            {
                return instance;
            }
        }


        internal void Get(IDefinition def)
        {
            IPAddress ip = ValidateIP();
            if (def.Type == DefinitionType.Scalar)
            {
                Report(_manager.Get(ip, _get, new Variable(def.TextualForm + ".0")));
            }
            else
            {
                //TODO: get index
                int index = 1;
                Variable result = _manager.Get(ip, _get, new Variable(def.TextualForm + "." + index));
            }
        }

        private void Report(Variable variable)
        {
            if (OnOperationCompleted != null)
            {
                OnOperationCompleted(variable.ToString());
            }
        }

        internal void Set(IDefinition def)
        {
            IPAddress ip = ValidateIP();
            //TODO: get type
            if (def.Type == DefinitionType.Scalar)
            {
                _manager.Set(ip, _get, new Variable(def.TextualForm + ".0"));
            }
            else
            {
                //TODO: get index
                int index = 1;
                _manager.Set(ip, _get, new Variable(def.TextualForm + "." + index));
            }
        }

        private IPAddress ValidateIP()
        {
            IPAddress ip;
            bool succeeded = IPAddress.TryParse(_ip, out ip);
            if (!succeeded)
            {
                throw new MibBrowserException("Invalid IP address: " + _ip);
            }
            return ip;
        }

        internal void Walk(IDefinition def)
        {
            IPAddress ip = ValidateIP();
        }
    }
}
