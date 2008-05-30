/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 17:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// MIB file class.
	/// </summary>
	public class MibModule
	{
		string _name;
		/// <summary>
		/// Creates a <see cref="MibModule"/> with a specific <see cref="Lexer"/>.
		/// </summary>
		/// <param name="lexer">Lexer</param>
		public MibModule(string name, Lexer lexer)
		{
			_name = name;
			Symbol temp = lexer.NextSymbol;
			if (temp != Symbol.Definitions)
			{
				throw SharpMibException.Create(temp);
			}
			temp = lexer.NextSymbol;
			if (temp != Symbol.Assign) {
				throw SharpMibException.Create(temp);
			}
			temp = lexer.NextSymbol;
			if (temp != Symbol.Begin) {
				throw SharpMibException.Create(temp);
			}
            do
            {
                temp = lexer.NextSymbol;
            } while (temp == Symbol.EOL);
            if (temp == Symbol.Imports)
            {
                ParseDependents(_dependents, lexer);
            }
			ParseEntities(_tokens, temp, _name, lexer);
		}
		
		static void ParseDependents(IList<string> dependents, Lexer lexer)
		{
			Symbol temp;
			while ((temp = lexer.NextSymbol) != null)
			{
				if (temp == Symbol.From)
				{
					dependents.Add(lexer.NextSymbol.ToString());
				}
				else if (temp == Symbol.Semicolon) 
				{
					return;
				}				
			}
		}
		
		IList<IAsn> _tokens = new List<IAsn>();
		IList<string> _dependents = new List<string>();
		
		void ParseEntities(IList<IAsn> tokens, Symbol last, string module, Lexer lexer)
		{
			Symbol temp = last;
			
			IList<Symbol> buffer = new List<Symbol>();
			IList<Symbol> next = new List<Symbol>();// symbol that belongs to next token.
            do
            {
                if (temp == Symbol.Imports || temp == Symbol.EOL)
                {
                    continue;
                }
                buffer.Add(temp);
                if (temp == Symbol.Assign)
                {
                    ParseEntity(tokens, module, buffer, lexer, ref next);
                    buffer.Clear();
                }
            }
            while ((temp = lexer.NextSymbol) != Symbol.End);
		}
		
		static void ParseEntity(IList<IAsn> tokens, string module, IList<Symbol> buffer, Lexer lexer, ref IList<Symbol> next)
		{	
            next.Clear();
            if (buffer.Count == 1)
            {
                throw SharpMibException.Create(buffer[0]);
            } 
            if (buffer.Count == 2)
            {
                // others
                tokens.Add(ParseOthers(module, buffer[0], lexer));
            }
            else if (buffer[1] == Symbol.Object)
            {
                // object identifier
                tokens.Add(ParseObjectIdentifier(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Module_Identity)
            {
                // module identity
                tokens.Add(ParseModuleIdentity(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Object_Type)
            {
                tokens.Add(ParseObjectType(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Object_Group) 
            {
            	tokens.Add(ParseObjectGroup(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Notification_Group) {
            	tokens.Add(ParseNotificationGroup(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Module_Compliance) {
            	tokens.Add(ParseModuleCompliance(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Notification_Type)
            {
                tokens.Add(ParseNotificationType(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Object_Identity)
            {
                tokens.Add(ParseObjectIdentity(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Macro)
            {
                tokens.Add(ParseMacro(module, buffer, lexer));
            }
		}

        private static IAsn ParseMacro(string module, IList<Symbol> header, Lexer lexer)
        {
            return new MacroNode(module, header, lexer);
        }

        static IEntity ParseObjectIdentity(string module, IList<Symbol> header, Lexer lexer)
        {
            return new ObjectIdentityNode(module, header, lexer);
        }

        static IEntity ParseNotificationType(string module, IList<Symbol> header, Lexer lexer)
        {
            return new NotificationTypeNode(module, header, lexer);
        }
		
		static IEntity ParseModuleCompliance(string module, IList<Symbol> header, Lexer lexer)
		{
            return new ModuleComplianceNode(module, header, lexer);
		}
		
		static IEntity ParseNotificationGroup(string module, IList<Symbol> header, Lexer lexer)
		{
            return new NotificationGroupNode(module, header, lexer);
		}
		
		static IEntity ParseObjectGroup(string module, IList<Symbol> header, Lexer lexer)
		{
            return new ObjectGroupNode(module, header, lexer);
		}

        static IEntity ParseObjectType(string module, IList<Symbol> header, Lexer lexer)
        {           
            return new ObjectTypeNode(module, header, lexer);
        }

        static IEntity ParseModuleIdentity(string module, IList<Symbol> header, Lexer lexer)
        {
            return new ModuleIdentityNode(module, header, lexer);
        }
		
		static IEntity ParseObjectIdentifier(string module, IList<Symbol> header, Lexer lexer)
		{
            if (header.Count != 4)
            {
                throw SharpMibException.Create(header[0]);
            }
			if (header[2] != Symbol.Identifier) {
				throw SharpMibException.Create(header[2]);
			}
			return new ObjectIdentifierNode(module, header[0].ToString(), lexer);
		}
		
		static IAsn ParseOthers(string module, Symbol name, Lexer lexer)
		{
			Symbol current = lexer.NextSymbol;
			if (current == Symbol.Sequence) {
				return new SequenceNode(module, name, lexer);
			}
			return new AliasNode(module, name, current, lexer);
		}
		/// <summary>
		/// Module name.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}
		/// <summary>
		/// OID nodes.
		/// </summary>
		internal IList<IEntity> EntityNodes
		{
			get
			{
				IList<IEntity> result = new List<IEntity>();
				foreach (IAsn e in _tokens)
				{
					if (e is IEntity) {
						result.Add((IEntity)e);
					}
				}
				return result;
			}
		}
		
		internal IList<string> Dependents
		{
			get
			{
				return _dependents;
			}
		}
		
		internal static bool AllDependentsAvailable(MibModule module, IDictionary<string, MibModule> modules)
		{
			foreach (string dependent in module.Dependents) {
            	if (!modules.ContainsKey(dependent)) {
            		return false;
            	}
            }
            return true;
		}
	}
}
