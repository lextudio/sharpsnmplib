/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/21
 * Time: 19:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Description of ObjectGroupNode.
	/// </summary>
	sealed class ObjectGroup : IEntity
	{
        string _module;
        string _parent;
        int _value;
        string _name;

        public ObjectGroup(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
            ConstructHelper.ParseOidValue(lexer, out _parent, out _value);
        }


        public string Module
        {
            get { return _module; }
        }

        public string Parent
        {
            get { return _parent; }
        }

        public int Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }
	}
}
