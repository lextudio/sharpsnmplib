/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 20:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object identifier node.
    /// </summary>
    internal sealed class OidValueAssignment : IEntity
    {
        private string _module;
        private string _name;
        private string _parent;
        private uint _value;
        
        /// <summary>
        /// Creates an <see cref="OidValueAssignment"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        public OidValueAssignment(string module, string name, string parent, uint value)
        {
            _module = module;
            _name = name;
            _parent = parent;
            _value = value;
        }
        
        /// <summary>
        /// Creates a <see cref="OidValueAssignment"/>.
        /// </summary>
        /// <param name="module">Module name</param>
        /// <param name="name">Name</param>
        /// <param name="lexer">Lexer</param>
        public OidValueAssignment(string module, string name, Lexer lexer)
        {            
            _module = module;
            _name = name;
            ConstructHelper.ParseOidValue(lexer, out _parent, out _value);
        }
        
        /// <summary>
        /// Module name.
        /// </summary>
        public string Module
        {
            get { return _module; }
        }
        
        /// <summary>
        /// Name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        
        /// <summary>
        /// Parent name.
        /// </summary>
        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        
        /// <summary>
        /// Value.
        /// </summary>
        public uint Value
        {
            get { return _value; }
        }
    }
}