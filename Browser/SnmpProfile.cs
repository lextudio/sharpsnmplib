using System;
using System.Collections.Generic;
using System.Text;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;

namespace Browser
{
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

        internal void Get(IDefinition def)
        {
            if (def.Type == DefinitionType.Scalar)
            {

            }
        }

        internal static void Initiate(Manager manager, string getCommunity, string setCommunity, VersionCode version, string ip)
        {
            lock (typeof(SnmpProfile))
            {
                if (instance == null)
                {
                    instance = new SnmpProfile(manager, getCommunity, setCommunity, version, ip);
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

        internal void Set(IDefinition def)
        {
            throw new NotImplementedException();
        }
    }
}
