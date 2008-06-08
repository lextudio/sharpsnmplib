/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/31
 * Time: 12:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    sealed class TrapType : IConstruct
    {
        string _module;
        int _value;
        string _name;

        public TrapType(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
            Symbol temp;
            while ((temp = lexer.NextSymbol) == Symbol.EOL)
            { }
            bool succeeded = int.TryParse(temp.ToString(), out _value);
            ConstructHelper.Validate(temp, !succeeded, "not a decimal");
        }

        public string Module
        {
            get { return _module; }
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
