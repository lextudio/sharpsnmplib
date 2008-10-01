/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/16
 * Time: 20:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object registry.
    /// </summary>
    public class ObjectRegistry
    {
        private ObjectTree _tree = new ObjectTree();
        private static ObjectRegistry instance;
        
        private ObjectRegistry()
        {
        }

        /// <summary>
        /// This event occurs when new documents are loaded.
        /// </summary>
        public event EventHandler OnChanged;
        
        /// <summary>
        /// Object tree.
        /// </summary>
        [CLSCompliant(false)]
        public IObjectTree Tree
        {
            get
            {
                return _tree;
            }
        }
        
        /// <summary>
        /// Registry
        /// </summary>
        public static ObjectRegistry Instance
        {
            get
            {
                lock (typeof(ObjectRegistry))
                {
                    if (instance == null)
                    {
                        instance = new ObjectRegistry();
                        instance.LoadDefaultDocuments();
                    }
                }
                
                return instance;
            }
        }
        
        private void LoadDefaultDocuments()
        {
            Compiler compiler = new Compiler();
            // SMI v2
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_SMI)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_CONF)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_TC)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_TM)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.HCNUM_TC)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IANA_ADDRESS_FAMILY_NUMBERS_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IANA_LANGUAGE_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IANA_RTPROTO_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IANAifType_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IF_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.EtherLike_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_TYPES)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IF_INVERTED_STACK_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.INET_ADDRESS_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IP_FORWARD_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_FLOW_LABEL_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_TC)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_ICMP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_TCP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_UDP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MONITOR_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_TC)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_SYSTEM_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.RMON_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_FRAMEWORK_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.AGENTX_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_EXPRESSION_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_NSLOOKUP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_PING_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_SCHEDULE_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_SCRIPT_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_TRACEROUTE_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXAMPLES_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_AGENT_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXTEND_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NETWORK_SERVICES_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NOTIFICATION_LOG_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.MTA_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_MPD_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_TARGET_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_PROXY_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_COMMUNITY_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_EVENT_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_NOTIFICATION_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USER_BASED_SM_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USM_AES_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USM_DH_OBJECTS_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_VIEW_BASED_ACM_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_VACM_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.TCP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.TRANSPORT_ADDRESS_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.TUNNEL_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_IPFWACC_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_IPFILTER_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_DLMOD_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_DISKIO_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.LM_SENSORS_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_DEMO_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB_OLD)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.UDP_MIB)));
            
            // SMI v1
            compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC_1215)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC1155_SMI)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC_1212)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.SMUX_MIB)));
            compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC1213_MIB)));
            
            Import(compiler.Modules);
            Refresh();
        }
        
        /// <summary>
        /// Indicates that if the specific OID is a table.
        /// </summary>
        /// <param name="id">OID</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public bool IsTableId(uint[] id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            
            string name = Translate(id);
            return name.EndsWith("Table", StringComparison.Ordinal);
        }
        
        internal static bool ValidateTable(ObjectIdentifier table)
        {
            try
            {
                return Mib.ObjectRegistry.Instance.IsTableId(table.ToNumerical());
            }
            catch (ArgumentOutOfRangeException)
            {
                // if no matching definition found, continue.
                return true;
            }
        }
        
        /// <summary>
        /// Gets numercial form from textual form.
        /// </summary>
        /// <param name="textual">Textual</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public uint[] Translate(string textual)
        {
            if (textual == null)
            {
                throw new ArgumentNullException("textual");
            }
            
            if (textual.Length == 0)
            {
                throw new ArgumentException("textual cannot be empty");
            }
            
            string[] content = textual.Split(new string[] { "::" }, StringSplitOptions.None);
            if (content.Length != 2)
            {
                throw new ArgumentException("textual format must be '<module>::<name>'");
            }
            
            return Translate(content[0], content[1]);
        }
        
        /// <summary>
        /// Gets numerical form from textual form.
        /// </summary>
        /// <param name="module">Module name</param>
        /// <param name="name">Object name</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public uint[] Translate(string module, string name)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            
            if (module.Length == 0)
            {
                throw new ArgumentException("module cannot be empty");
            }
            
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            
            if (name.Length == 0)
            {
                throw new ArgumentException("name cannot be empty");
            }
            
            if (!name.Contains("."))
            {
                return _tree.Find(module, name).GetNumericalForm();
            }
            
            string[] content = name.Split('.');
            if (content.Length != 2)
            {
                throw new ArgumentException("name can only contain one dot");
            }
            
            int value;
            bool succeeded = int.TryParse(content[1], out value);
            if (!succeeded)
            {
                throw new ArgumentException("not a decimal after dot");
            }
            
            uint[] oid = _tree.Find(module, content[0]).GetNumericalForm();
            return Definition.AppendTo(oid, (uint)value);
        }
        
        /// <summary>
        /// Gets textual form from numerical form.
        /// </summary>
        /// <param name="numerical">Numerical form</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public string Translate(uint[] numerical)
        {
            if (numerical == null)
            {
                throw new ArgumentNullException("numerical");
            }
            
            try
            {
                return _tree.Find(numerical).TextualForm;
            }
            catch (ArgumentOutOfRangeException)
            {
                // no definition matches numerical.
            }
            
            return _tree.Find(GetParent(numerical)).TextualForm + "." + numerical[numerical.Length - 1].ToString(CultureInfo.InvariantCulture);
        }
        
        private static uint[] GetParent(uint[] id)
        {
            uint[] result = new uint[id.Length - 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = id[i];
            }
            
            return result;
        }
        /// <summary>
        /// Loads a folder of MIB files.
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="pattern">MIB file pattern</param>
        public void CompileFolder(string folder, string pattern)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            
            if (folder.Length == 0)
            {
                throw new ArgumentException("folder cannot be empty");
            }
            
            if (!Directory.Exists(folder))
            {
                throw new ArgumentException("folder does not exist: " + folder);
            }
            
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            
            if (pattern.Length == 0)
            {
                throw new ArgumentException("pattern cannot be empty");
            }
            
            CompileFiles(Directory.GetFiles(folder, pattern));
        }

        /// <summary>
        /// Loads MIB files.
        /// </summary>
        /// <param name="fileNames">File names.</param>
        public void CompileFiles(IEnumerable<string> fileNames)
        {
            Compiler compiler = new Compiler();
            foreach (string fileName in fileNames)
            {
                compiler.Compile(fileName);
            }
            Import(compiler.Modules);
            Refresh();
        }
        
        /// <summary>
        /// Loads a MIB file.
        /// </summary>
        /// <param name="fileName">File name</param>
        public void Compile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            
            if (fileName.Length == 0)
            {
                throw new ArgumentException("fileName cannot be empty");
            }
            
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("file does not exist: " + fileName);
            }
            
            Compiler compiler = new Compiler();
            compiler.Compile(fileName);
            Import(compiler.Modules);
            Refresh();
        }
     
        internal void Import(IEnumerable<MibModule> modules)
        {
            _tree.Import(modules);
        }
        
        internal void Refresh()
        {
            _tree.Refresh();
            if (OnChanged != null) 
            {
                OnChanged(this, EventArgs.Empty);
            }
        }
    }
}