/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 17:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// MIB file class.
	/// </summary>
	public class MibModule
	{
		string _name;
		Imports _imports;
		Exports _exports;
		IList<IConstruct> _tokens = new List<IConstruct>();
		/// <summary>
		/// Creates a <see cref="MibModule"/> with a specific <see cref="Lexer"/>.
		/// </summary>
		/// <param name="name">Module name</param>
		/// <param name="lexer">Lexer</param>
		public MibModule(string name, Lexer lexer)
		{
			_name = name;
			Symbol temp = lexer.NextSymbol;
			ConstructHelper.Expect(temp, Symbol.Definitions);
			temp = lexer.NextSymbol;
			ConstructHelper.Expect(temp, Symbol.Assign);
			temp = lexer.NextSymbol;
			ConstructHelper.Expect(temp, Symbol.Begin);
            temp = ConstructHelper.IgnoreEOL(lexer);
            if (temp == Symbol.Imports)
            {
                _imports = ParseDependents(lexer);
            }
            else if (temp == Symbol.Exports) {
            	_exports = ParseExports(lexer);
            }
			ParseEntities(_tokens, temp, _name, lexer);
		}
		
		Exports ParseExports(Lexer lexer)
		{
			return new Exports(lexer);
		}
		
		Imports ParseDependents(Lexer lexer)
		{
			return new Imports(lexer);
		}
		
		void ParseEntities(IList<IConstruct> tokens, Symbol last, string module, Lexer lexer)
		{
			Symbol temp = last;
			
			IList<Symbol> buffer = new List<Symbol>();
			IList<Symbol> next = new List<Symbol>();// symbol that belongs to next token.
            do
            {
                if (temp == Symbol.Imports || temp == Symbol.Exports || temp == Symbol.EOL)
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
		
		static void ParseEntity(IList<IConstruct> tokens, string module, IList<Symbol> buffer, Lexer lexer, ref IList<Symbol> next)
		{	
            next.Clear();
            ConstructHelper.Validate(buffer[0], buffer.Count == 1, "unexpected symbol");
            ConstructHelper.ValidateIdentifier(buffer[0]);
            if (buffer.Count == 2)
            {
                // others
                tokens.Add(ParseOthers(module, buffer, lexer));
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
            else if (buffer[1] == Symbol.Trap_Type) 
            {
            	tokens.Add(new TrapType(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Agent_Capabilities) {
            	tokens.Add(new AgentCapabilities(module, buffer, lexer));
            }
		}

        private static IConstruct ParseMacro(string module, IList<Symbol> header, Lexer lexer)
        {
            return new Macro(module, header, lexer);
        }

        static IEntity ParseObjectIdentity(string module, IList<Symbol> header, Lexer lexer)
        {
            return new ObjectIdentity(module, header, lexer);
        }

        static IEntity ParseNotificationType(string module, IList<Symbol> header, Lexer lexer)
        {
            return new NotificationType(module, header, lexer);
        }
		
		static IEntity ParseModuleCompliance(string module, IList<Symbol> header, Lexer lexer)
		{
            return new ModuleCompliance(module, header, lexer);
		}
		
		static IEntity ParseNotificationGroup(string module, IList<Symbol> header, Lexer lexer)
		{
            return new NotificationGroup(module, header, lexer);
		}
		
		static IEntity ParseObjectGroup(string module, IList<Symbol> header, Lexer lexer)
		{
            return new ObjectGroup(module, header, lexer);
		}

        static IEntity ParseObjectType(string module, IList<Symbol> header, Lexer lexer)
        {           
            return new ObjectType(module, header, lexer);
        }

        static IEntity ParseModuleIdentity(string module, IList<Symbol> header, Lexer lexer)
        {
            return new ModuleIdentity(module, header, lexer);
        }
		
		static IEntity ParseObjectIdentifier(string module, IList<Symbol> header, Lexer lexer)
		{
			ConstructHelper.Validate(header[0], header.Count != 4, "invalid OID value assignment");
			ConstructHelper.Expect(header[2], Symbol.Identifier);
			return new OidValueAssignment(module, header[0].ToString(), lexer);
		}
		
		static IConstruct ParseOthers(string module, IList<Symbol> header, Lexer lexer)
		{
			Symbol current;
            while ((current = lexer.NextSymbol) == Symbol.EOL) { }
            if (current == Symbol.Sequence) {
				return new Sequence(module, header[0].ToString(), lexer);
			} 
			else if (current == Symbol.Choice) {
				return new Choice(module, header[0].ToString(), lexer);
			}
            else if (current == Symbol.Textual_Convention)
            {
                return new TextualConvention(module, header[0].ToString(), lexer);
            }
			return new TypeAssignment(module, header[0].ToString(), current, lexer);
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
		internal IList<IEntity> Entities
		{
			get
			{
				IList<IEntity> result = new List<IEntity>();
				foreach (IConstruct e in _tokens)
				{
					if (e is IEntity) {
						result.Add((IEntity)e);
					}
				}
				return result;
			}
		}		
		/// <summary>
		/// OID nodes.
		/// </summary>
		internal IList<ObjectType> Objects
		{
			get
			{
				IList<ObjectType> result = new List<ObjectType>();
				foreach (IConstruct e in _tokens)
				{
					if (e is ObjectType) {
						result.Add((ObjectType)e);
					}
				}
				return result;
			}
		}
		
		internal IList<string> Dependents
		{
			get
			{
				return (_imports == null) ? new List<string>() : _imports.Dependents;
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
