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
					}
				}
				return instance;
			}
		}
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
		
		uint[] GetParent(uint[] id)
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
