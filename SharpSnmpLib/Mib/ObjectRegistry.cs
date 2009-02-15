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
		private readonly ObjectTree _tree;
		private static volatile IObjectRegistry _default;
		private static readonly object locker = new object();
		private readonly string _path;
		private const string DefaultPath = "modules";

		private ObjectRegistry()
		{
			_path = DefaultPath;
			_tree = new ObjectTree(LoadDefaultModules());
		}
		
		private IList<ModuleLoader> LoadDefaultModules()
		{
			IList<ModuleLoader> result = new List<ModuleLoader>(5);
			result.Add(LoadSingle(Resource.SNMPv2_SMI, "SNMPV2-SMI"));
			result.Add(LoadSingle(Resource.SNMPv2_CONF, "SNMPV2-CONF"));
			result.Add(LoadSingle(Resource.SNMPv2_TC, "SNMPV2-TC"));
			result.Add(LoadSingle(Resource.SNMPv2_MIB, "SNMPV2-MIB"));
			result.Add(LoadSingle(Resource.SNMPv2_TM, "SNMPV2-TM"));
			return result;
		}
		
		private ModuleLoader LoadSingle(string mibFileContent, string name)
		{
			ModuleLoader result;
			using (TextReader reader = new StringReader(mibFileContent))
			{
				result = new ModuleLoader(reader, name);
				//reader.Close();	// Not needed: we're in a 'using' statement.
			}
			
			return result;
		}

		
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectRegistry"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public ObjectRegistry(string path)
		{
		    if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
			{
				_path = DefaultPath;
			}
			else
			{
				_path = path;
			}

			if (Directory.Exists(_path))
			{
				_tree = new ObjectTree(Directory.GetFiles(_path, "*.module"));
			}
			else
			{
				_tree = new ObjectTree();
			}
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
		[CLSCompliant(false)]
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
							_default = new ObjectRegistry();
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

        //private void LoadDefaultDocuments()
        //{
        //    // SMI v2
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_SMI))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_CONF))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_TC))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMPv2_TM))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.HCNUM_TC))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IANA_ADDRESS_FAMILY_NUMBERS_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IANA_LANGUAGE_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IANA_RTPROTO_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IANAifType_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IF_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.EtherLike_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_TYPES))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IF_INVERTED_STACK_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.INET_ADDRESS_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IP_FORWARD_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IPV6_FLOW_LABEL_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IPV6_TC))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IPV6_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IPV6_ICMP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IPV6_TCP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.IPV6_UDP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MONITOR_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_TC))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_SYSTEM_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.RMON_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_FRAMEWORK_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.AGENTX_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_EXPRESSION_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_NSLOOKUP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_PING_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_SCHEDULE_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_SCRIPT_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_TRACEROUTE_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXAMPLES_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_AGENT_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXTEND_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NETWORK_SERVICES_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NOTIFICATION_LOG_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.MTA_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_MPD_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_TARGET_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_PROXY_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_COMMUNITY_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.DISMAN_EVENT_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_NOTIFICATION_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USER_BASED_SM_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USM_AES_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_USM_DH_OBJECTS_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SNMP_VIEW_BASED_ACM_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.NET_SNMP_VACM_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.TCP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.TRANSPORT_ADDRESS_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.TUNNEL_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_IPFWACC_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_IPFILTER_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_DLMOD_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_DISKIO_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.LM_SENSORS_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_DEMO_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB_OLD))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.UDP_MIB))));
			
        //    // SMI v1
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.RFC_1215))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.RFC1155_SMI))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.RFC_1212))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.SMUX_MIB))));
        //    Import(Parser.Compile(new StreamReader(new MemoryStream(Resource.RFC1213_MIB))));

        //    Refresh();
        //}
		
		/// <summary>
		/// Indicates that if the specific OID is a table.
		/// </summary>
		/// <param name="id">OID</param>
		/// <returns></returns>
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

		/// <summary>
		/// Validates if an <see cref="ObjectIdentifier"/> is a table.
		/// </summary>
		/// <param name="table">The object identifier.</param>
		/// <returns></returns>
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
			Array.Copy(id, result, id.Length - 1);
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
				Import(Parser.Compile(fileName));
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
			
			Import(Parser.Compile(fileName));
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
		/// Refreshes.
		/// </summary>
		/// <remarks>This method raises an <see cref="OnChanged"/> event. </remarks>
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
