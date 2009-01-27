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
    public class ObjectRegistry : IObjectRegistry
    {
        private readonly ObjectTree _tree = new ObjectTree();
        private static volatile IObjectRegistry _default;
        private static readonly object locker = new object();
        private readonly string _path;
        private const string DefaultPath = "mibs";

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectRegistry"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ObjectRegistry(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                _path = DefaultPath;
                LoadDefaultDocuments();
            }
            else
            {
                _path = path;
            }
            
            LoadDocuments(_path);
        }

        private void LoadDocuments(string folder)
        {
            // TODO: load .module files here instead of MIB documents.
            foreach (string file in Directory.GetFiles(folder))
            {
                Import(Compiler.Compile(file));
            }

            Refresh();
        }

        /// <summary>
        /// This event occurs when new documents are loaded.
        /// </summary>
        public event EventHandler<EventArgs> OnChanged;
        
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
        public static IObjectRegistry Default
        {
            get
            {
                if (_default == null)
                {
                    lock (locker)
                    {
                        if (_default == null)
                        {
                            _default = new ObjectRegistry(string.Empty);
                        }
                    }
                }
                
                return _default;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return _path; }
        }

        private void LoadDefaultDocuments()
        {          
            // SMI v2
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_SMI))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_CONF))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_TC))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_TM))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.HCNUM_TC))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IANA_ADDRESS_FAMILY_NUMBERS_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IANA_LANGUAGE_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IANA_RTPROTO_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IANAifType_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IF_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.EtherLike_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_TYPES))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IF_INVERTED_STACK_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.INET_ADDRESS_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IP_FORWARD_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_FLOW_LABEL_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_TC))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_ICMP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_TCP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.IPV6_UDP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MONITOR_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_TC))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_SYSTEM_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.RMON_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_FRAMEWORK_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.AGENTX_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_EXPRESSION_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_NSLOOKUP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_PING_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_SCHEDULE_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_SCRIPT_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_TRACEROUTE_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXAMPLES_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_AGENT_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXTEND_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NETWORK_SERVICES_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NOTIFICATION_LOG_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.MTA_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_MPD_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_TARGET_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_PROXY_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_COMMUNITY_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_EVENT_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_NOTIFICATION_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USER_BASED_SM_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USM_AES_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USM_DH_OBJECTS_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SNMP_VIEW_BASED_ACM_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_VACM_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.TCP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.TRANSPORT_ADDRESS_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.TUNNEL_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_IPFWACC_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_IPFILTER_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_DLMOD_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_DISKIO_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.LM_SENSORS_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_DEMO_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB_OLD))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.UDP_MIB))));
            
            // SMI v1
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC_1215))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC1155_SMI))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC_1212))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.SMUX_MIB))));
            Import(Compiler.Compile(new StreamReader(new MemoryStream(Resource.RFC1213_MIB))));

            Refresh();
        }
        
        /// <summary>
        /// Indicates that if the specific OID is a table.
        /// </summary>
        /// <param name="id">OID</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        internal bool IsTableId(uint[] id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            
            // TODO: enhance checking here.
            string name = Translate(id);
            return name.EndsWith("Table", StringComparison.Ordinal);
        }

        public bool ValidateTable(ObjectIdentifier table)
        {
            try
            {
                return IsTableId(table.ToNumerical());
            }
            catch (ArgumentOutOfRangeException)
            {
                // if no matching definition found, refuse to continue.
                return false;
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
            if (fileNames == null)
            {
                throw new ArgumentNullException("fileNames");
            }

            foreach (string fileName in fileNames)
            {
                Import(Compiler.Compile(fileName));
            }
            
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
            
            Import(Compiler.Compile(fileName));
            Refresh();
        }
     
        /// <summary>
        /// Imports instances of <see cref="MibModule"/>.
        /// </summary>
        /// <param name="modules">Modules.</param>
        public void Import(IEnumerable<MibModule> modules)
        {
            _tree.Import(modules);
        }

        /// <summary>
        /// Imports a .module file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        internal void Import(string fileName)
        {
            // TODO:
        }

        public void Refresh()
        {
            _tree.Refresh();
            EventHandler<EventArgs> handler = OnChanged;
            if (handler != null) 
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}