using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Default object registry.
    /// </summary>
    public sealed class DefaultObjectRegistry : ObjectRegistryBase
    {
        private static volatile DefaultObjectRegistry _instance;
        private static readonly object locker = new object();
        
        private DefaultObjectRegistry()
        {
            Tree = new ObjectTree(LoadDefaultModules());
        }

        /// <summary>
        /// Default instance.
        /// </summary>
        [CLSCompliant(false)]
        public static DefaultObjectRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DefaultObjectRegistry();
                        }
                    }
                }
                
                return _instance;
            }
        }

        private static IList<ModuleLoader> LoadDefaultModules()
        {
            IList<ModuleLoader> result = new List<ModuleLoader>(5);
            result.Add(LoadSingle(Resource.SNMPv2_SMI, "SNMPV2-SMI"));
            result.Add(LoadSingle(Resource.SNMPv2_CONF, "SNMPV2-CONF"));
            result.Add(LoadSingle(Resource.SNMPv2_TC, "SNMPV2-TC"));
            result.Add(LoadSingle(Resource.SNMPv2_MIB, "SNMPV2-MIB"));
            result.Add(LoadSingle(Resource.SNMPv2_TM, "SNMPV2-TM"));
            return result;
        }

        private static ModuleLoader LoadSingle(string mibFileContent, string name)
        {
            ModuleLoader result;
            using (TextReader reader = new StringReader(mibFileContent))
            {
                result = new ModuleLoader(reader, name);
            }
            
            return result;
        }
    }
}