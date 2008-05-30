using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object identifier node.
    /// </summary>
    public class ObjectIdentityNode : IEntity
    {
        string _module;
        string _name;
        string _parent;
        int _value;
        /// <summary>
        /// Creates a <see cref="ObjectIdentifierNode"/>.
        /// </summary>
        /// <param name="module">Module name</param>
        /// <param name="header">Header</param>
        /// <param name="lexer">Lexer</param>
        public ObjectIdentityNode(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
            Symbol temp = lexer.NextSymbol;
            if (temp != Symbol.OpenBracket)
            {
                throw SharpMibException.Create(temp);
            }
            _parent = lexer.NextSymbol.ToString();
            temp = lexer.NextSymbol;
            bool succeeded = int.TryParse(temp.ToString(), out _value);
            if (!succeeded)
            {
                throw SharpMibException.Create(temp);
            }
            temp = lexer.NextSymbol;
            if (temp != Symbol.CloseBracket)
            {
                throw SharpMibException.Create(temp);
            }
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

