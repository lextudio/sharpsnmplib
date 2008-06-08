/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/16
 * Time: 20:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Object registry.
	/// </summary>
	public class ObjectRegistry
	{
		ObjectTree _tree = new ObjectTree();
		static ObjectRegistry instance;
		
		private ObjectRegistry() {}
		/// <summary>
		/// Registry instance.
		/// </summary>
		public static ObjectRegistry Instance
		{
			get
			{
				lock(typeof(ObjectRegistry))
				{
					if (instance == null) {
						instance = new ObjectRegistry();
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMPv2_SMI)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMPv2_CONF)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMPv2_TC)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMPv2_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMPv2_TM)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.AGENTX_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_EVENT_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_EXPRESSION_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_NSLOOKUP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_PING_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_SCHEDULE_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_SCRIPT_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.DISMAN_TRACEROUTE_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.EtherLike_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.HCNUM_TC)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.HOST_RESOURCES_TYPES)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IANA_ADDRESS_FAMILY_NUMBERS_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IANA_LANGUAGE_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IANA_RTPROTO_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IANAifType_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IF_INVERTED_STACK_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IF_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.INET_ADDRESS_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IP_FORWARD_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IPV6_FLOW_LABEL_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IPV6_ICMP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IPV6_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IPV6_TC)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IPV6_TCP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.IPV6_UDP_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.LM_SENSORS_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.MTA_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_AGENT_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXAMPLES_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_EXTEND_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_MONITOR_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_SYSTEM_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_TC)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NET_SNMP_VACM_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NETWORK_SERVICES_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.NOTIFICATION_LOG_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.RMON_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SMUX_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_COMMUNITY_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_FRAMEWORK_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_MPD_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_NOTIFICATION_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_PROXY_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_TARGET_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_USER_BASED_SM_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_USM_AES_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_USM_DH_OBJECTS_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.SNMP_VIEW_BASED_ACM_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.TCP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.TRANSPORT_ADDRESS_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.TUNNEL_MIB)));
						
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_DEMO_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_DISKIO_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_DLMOD_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_IPFILTER_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_IPFWACC_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UCD_SNMP_MIB_OLD)));
						instance.LoadFile(new StreamReader(new MemoryStream(Resource.UDP_MIB)));						
					}
				}
				return instance;
			}
		}
		/// <summary>
		/// Indicates that if the specific OID is a table.
		/// </summary>
		/// <param name="id">OID</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public bool IsTableId(uint[] id)
		{
			string name = GetTextualFrom(id);
			return name.EndsWith("Table", StringComparison.Ordinal);
		}
		/// <summary>
		/// Gets numercial form from textual form.
		/// </summary>
		/// <param name="textual">Textual</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public uint[] GetNumericalFrom(string textual)
		{
			if (textual == null) {
				throw new ArgumentNullException("textual");
			}
			if (textual.Length == 0) {
				throw new ArgumentException("textual cannot be empty");
			}
			string[] content = textual.Split(new string[] {"::"}, StringSplitOptions.None);
			if (content.Length != 2)
			{
				throw new ArgumentException("textual format must be '<module>::<name>'");
			}
			return GetNumericalFrom(content[0], content[1]);
		}
		/// <summary>
		/// Gets numerical form from textual form.
		/// </summary>
		/// <param name="module">Module name</param>
		/// <param name="name">Object name</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public uint[] GetNumericalFrom(string module, string name)
		{
			if (module == null) {
				throw new ArgumentNullException("module");
			}
			if (module.Length == 0) {
				throw new ArgumentException("module cannot be empty");
			}
			if (name == null) {
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0) {
				throw new ArgumentException("name cannot be empty");
			}
			if (!name.Contains(".")) {
				return _tree.Find(module, name).NumericalForm;
			}
			string[] content = name.Split('.');
			if (content.Length != 2) {
				throw new ArgumentException("name can only contain one dot");
			}
			int value;
			bool succeeded = int.TryParse(content[1], out value);
			if (!succeeded) {
				throw new ArgumentException("not a decimal after dot");
			}
			uint[] oid = _tree.Find(module, content[0]).NumericalForm;
			return Definition.AppendTo(oid, (uint)value);
		}
		/// <summary>
		/// Gets textual form from numerical form.
		/// </summary>
		/// <param name="numerical">Numerical form</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public string GetTextualFrom(uint[] numerical)
		{
			if (numerical == null) {
				throw new ArgumentNullException("numerical");
			}			
			try {
				return _tree.Find(numerical).TextualForm;
			} catch (ArgumentOutOfRangeException) {
			}
			return _tree.Find(GetParent(numerical)).TextualForm + "." + numerical[numerical.Length - 1];
		}
		
		static uint[] GetParent(uint[] id)
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
		public void LoadFolder(string folder, string pattern)
		{
			if (folder == null) {
				throw new ArgumentNullException("folder");
			}
			if (folder.Length == 0) {
				throw new ArgumentException("folder cannot be empty");
			}
			if (!Directory.Exists(folder)) {
				throw new ArgumentException("folder does not exist: " + folder);
			}
			if (pattern == null) {
				throw new ArgumentNullException("pattern");
			}
			if (pattern.Length == 0) {
				throw new ArgumentException("pattern cannot be empty");
			}
			foreach (string file in Directory.GetFiles(folder, pattern))
			{
				LoadFile(file);
			}
		}
		/// <summary>
		/// Loads a MIB file.
		/// </summary>
		/// <param name="fileName">File name</param>
		public void LoadFile(string fileName)
		{
			if (fileName == null) {
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0) {
				throw new ArgumentException("fileName cannot be empty");
			}
			if (!File.Exists(fileName)) {
				throw new ArgumentException("file does not exist: " + fileName);
			}
			LoadFile(File.OpenText(fileName));
		}

		internal void LoadFile(TextReader stream)
		{
			try {
				_tree.Parse(stream);
			}
			finally
			{
				stream.Close();
			}
		}
	}
}
