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
using System.Text.RegularExpressions;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// MIB module class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mib")]
    internal sealed class MibModule : IModule
    {
        private readonly string _name;
        private readonly Imports _imports;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Exports _exports;
        private readonly List<IConstruct> _tokens = new List<IConstruct>();
        
        internal MibModule(string name, IEnumerable<string> dependents)
        {
            _name = name;
            _imports = new Imports(dependents);
        }
        
        /// <summary>
        /// Creates a <see cref="MibModule"/> with a specific <see cref="Lexer"/>.
        /// </summary>
        /// <param name="name">Module name</param>
        /// <param name="lexer">Lexer</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lexer")]
        public MibModule(string name, Lexer lexer)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            
            if (lexer == null)
            {
                throw new ArgumentNullException("lexer");
            }
            
            _name = name.ToUpperInvariant(); // all module name are uppercase.
            Symbol temp = lexer.NextNonEOLSymbol;
            ConstructHelper.Expect(temp, Symbol.Definitions);
            temp = lexer.NextNonEOLSymbol;
            ConstructHelper.Expect(temp, Symbol.Assign);
            temp = lexer.NextSymbol;
            ConstructHelper.Expect(temp, Symbol.Begin);
            temp = lexer.NextNonEOLSymbol;
            if (temp == Symbol.Imports)
            {
                _imports = ParseDependents(lexer);
            }
            else if (temp == Symbol.Exports)
            {
                _exports = ParseExports(lexer);
            }
            
            ParseEntities(_tokens, temp, _name, lexer);
        }
        
        private static Exports ParseExports(Lexer lexer)
        {
            return new Exports(lexer);
        }
        
        private static Imports ParseDependents(Lexer lexer)
        {
            return new Imports(lexer);
        }
        
        private static void ParseEntities(IList<IConstruct> tokens, Symbol last, string module, Lexer lexer)
        {
            Symbol temp = last;
            
            IList<Symbol> buffer = new List<Symbol>();
            IList<Symbol> next = new List<Symbol>(); // symbol that belongs to next token.
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
                    foreach (Symbol s in next)
                    {
                        if (s == Symbol.End)
                        {
                            return;
                        }
                        
                        buffer.Add(s);
                    }
                    
                    next.Clear();
                }
            }
            while ((temp = lexer.NextSymbol) != Symbol.End);
        }
        
        private static void ParseEntity(IList<IConstruct> tokens, string module, IList<Symbol> buffer, Lexer lexer, ref IList<Symbol> next)
        {
            next.Clear();
            ConstructHelper.Validate(buffer[0], buffer.Count == 1, "unexpected symbol");
            ConstructHelper.ValidateIdentifier(buffer[0]);
            if (buffer.Count == 2)
            {
                // others
                tokens.Add(ParseOthers(module, buffer, lexer, ref next));
            }
            else if (buffer[1] == Symbol.Object)
            {
                // object identifier
                tokens.Add(ParseObjectIdentifier(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.ModuleIdentity)
            {
                // module identity
                tokens.Add(new ModuleIdentity(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.ObjectType)
            {
                tokens.Add(new ObjectType(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.ObjectGroup)
            {
                tokens.Add(new ObjectGroup(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.NotificationGroup)
            {
                tokens.Add(new NotificationGroup(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.ModuleCompliance)
            {
                tokens.Add(new ModuleCompliance(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.NotificationType)
            {
                tokens.Add(new NotificationType(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.ObjectIdentity)
            {
                tokens.Add(new ObjectIdentity(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.Macro)
            {
                tokens.Add(new Macro(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.TrapType)
            {
                tokens.Add(new TrapType(module, buffer, lexer));
            }
            else if (buffer[1] == Symbol.AgentCapabilities)
            {
                tokens.Add(new AgentCapabilities(module, buffer, lexer));
            }
        }
        
        private static IEntity ParseObjectIdentifier(string module, IList<Symbol> header, Lexer lexer)
        {
            ConstructHelper.Validate(header[0], header.Count != 4, "invalid OID value assignment");
            ConstructHelper.Expect(header[2], Symbol.Identifier);
            return new OidValueAssignment(module, header[0].ToString(), lexer);
        }

        private static IConstruct ParseOthers(string module, IList<Symbol> header, Lexer lexer, ref IList<Symbol> next)
        {
            Symbol current = lexer.NextNonEOLSymbol;
            
            if (current == Symbol.Sequence)
            {
                return new Sequence(module, header[0].ToString(), lexer);
            }
            else if (current == Symbol.Choice)
            {
                return new Choice(module, header[0].ToString(), lexer);
            }
            else if (current == Symbol.Integer)
            {
                return new Integer(module, header[0].ToString(), lexer);
            }
            else if (current == Symbol.TextualConvention)
            {
                return new TextualConvention(module, header[0].ToString(), lexer);
            }
            
            TypeAssignment result = new TypeAssignment(module, header[0].ToString(), current, lexer);
            next.Add(result.Left);
            return result;
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
        public IList<IEntity> Entities
        {
            get
            {
                IList<IEntity> result = new List<IEntity>();
                foreach (IConstruct e in _tokens)
                {
                    IEntity entity = e as IEntity;
                    if (entity != null)
                    {
                        result.Add(entity);
                    }
                }
                
                return result;
            }
        }
        
        /// <summary>
        /// OID nodes.
        /// </summary>
        public IList<IEntity> Objects
        {
            get
            {
                IList<IEntity> result = new List<IEntity>();
                foreach (IConstruct e in _tokens)
                {
                    ObjectType entity = e as ObjectType;
                    if (entity != null)
                    {
                        result.Add(entity);
                    }
                }
                
                return result;
            }
        }
        
        public IList<string> Dependents
        {
            get
            {
                return (_imports == null) ? new List<string>() : _imports.Dependents;
            }
        }
        
        internal static bool AllDependentsAvailable(MibModule module, IDictionary<string, MibModule> modules)
        {
            foreach (string dependent in module.Dependents)
            {
                if (!DependentFound(dependent, modules))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        private static bool DependentFound(string dependent, IDictionary<string, MibModule> modules)
        {
            const string Pattern = "-V[0-9]+$";
            if (!Regex.IsMatch(dependent, Pattern))
            {
                return modules.ContainsKey(dependent);
            }

            if (modules.ContainsKey(dependent))
            {
                return true;
            }
            
            string dependentNonVersion = Regex.Replace(dependent, Pattern, string.Empty);
            return modules.ContainsKey(dependentNonVersion);
        }
    }
}