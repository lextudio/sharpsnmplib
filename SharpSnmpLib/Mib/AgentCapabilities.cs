/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/31
 * Time: 13:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// The AGENT-CAPABILITIES construct is used to specify implementation characteristics of an SNMP agent sub-system with respect to object types and events.
	/// </summary>
	sealed class AgentCapabilities : IEntity
	{
		string _module;
		string _name;
		string _parent;
		int _value;
		/// <summary>
		/// Creates an <see cref="AgentCapabilities"/> instance.
		/// </summary>
		/// <param name="module"></param>
		/// <param name="header"></param>
		/// <param name="lexer"></param>
		public AgentCapabilities(string module, IList<Symbol> header, Lexer lexer)
		{
			_module = module;
			_name = header[0].ToString();
            ConstructHelper.ParseOidValue(lexer, out _parent, out _value);
		}
		/// <summary>
        /// Module name.
        /// </summary>
        public string Module
        {
            get
            {
                return _module;
            }
        }
        /// <summary>
        /// Name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// Parent name.
        /// </summary>
        public string Parent
        {
            get
            {
                return _parent;
            }
        }
        /// <summary>
        /// Value.
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }
        }
	}
}
