/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Lextm.SharpSnmpLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    
    /// <summary>
    /// Variable bind.
    /// </summary>
    /// <remarks>
    /// <para>Represents SNMP variable bind.</para>
    /// </remarks>
    public class Variable
    {
        private ObjectIdentifier _oid;
        private ISnmpData _data;
        
        /// <summary>
        /// Creates a <see cref="Variable"/> instance with a specific <see cref="ObjectIdentifier"/>.
        /// </summary>
        /// <param name="oid">Object identifier</param>
        public Variable(ObjectIdentifier oid) : this(oid, null)     
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="Variable"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="textual">Textual IID string</param>
        public Variable(string textual) : this(new ObjectIdentifier(Mib.ObjectRegistry.Instance.GetNumericalFrom(textual)), null)
        {  
        }
        
        /// <summary>
        /// Creates a <see cref="Variable"/> instance with a specific object identifier.
        /// </summary>
        /// <param name="oid">Object identifier</param>
        [CLSCompliant(false)]
        public Variable(uint[] oid) : this(new ObjectIdentifier(oid)) 
        {
        }
        
        /// <summary>
        /// Creates a <see cref="Variable"/> instance with a specific <see cref="String"/> and <see cref="ISnmpData"/>.
        /// </summary>
        /// <param name="textual"></param>
        /// <param name="data"></param>
        public Variable(string textual, ISnmpData data)
            : this(new ObjectIdentifier(Mib.ObjectRegistry.Instance.GetNumericalFrom(textual)), data)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="Variable"/> instance with a specific <see cref="ObjectIdentifier"/> and <see cref="ISnmpData"/>.
        /// </summary>
        /// <param name="oid">Object identifier</param>
        /// <param name="data">Data</param>
        /// <remarks>If you set <c>null</c> to <paramref name="data"/>, you get a <see cref="Variable"/> instance whose <see cref="Data"/> is a <see cref="Null"/> instance.</remarks>
        [CLSCompliant(false)]
        public Variable(uint[] oid, ISnmpData data) : this(new ObjectIdentifier(oid), data) 
        { 
        }
        
        /// <summary>
        /// Creates a <see cref="Variable"/> instance with a specific object identifier and data.
        /// </summary>
        /// <param name="oid">Object identifier</param>
        /// <param name="data">Data</param>
        /// <remarks>If you set <c>null</c> to <paramref name="data"/>, you get a <see cref="Variable"/> instance whose <see cref="Data"/> is a <see cref="Null"/> instance.</remarks>
        public Variable(ObjectIdentifier oid, ISnmpData data)
        {
            _oid = oid;
            _data = data ?? new Null();
        }
        
        /// <summary>
        /// Variable object identifier.
        /// </summary>
        public ObjectIdentifier Id
        {
            get
            {
                return _oid;
            }
        }
        
        /// <summary>
        /// Variable data.
        /// </summary>
        public ISnmpData Data
        {
            get
            {
                return _data;
            }
        }
        
        /// <summary>
        /// Converts varbind section to variable binds list.
        /// </summary>
        /// <param name="varbindSection"></param>
        /// <returns></returns>
        internal static IList<Variable> Transform(Sequence varbindSection)
        {
            IList<Variable> result = new List<Variable>(varbindSection.Items.Count);
            foreach (ISnmpData item in varbindSection.Items)
            {
                if (item.TypeCode != SnmpType.Sequence)
                {
                    throw new ArgumentException("wrong varbind section data");
                }
                
                Sequence varbind = item as Sequence;
                if (null != varbind)
                {
                    if (varbind.Items.Count != 2 || varbind.Items[0].TypeCode != SnmpType.ObjectIdentifier)
                    {
                        throw new ArgumentException("wrong varbind data");
                    }
                    
                    result.Add(new Variable((ObjectIdentifier)varbind.Items[0], varbind.Items[1]));
                }
                else
                {
                    Variable v = item as Variable;
                    if (null != v) 
                    {
                        result.Add(v);
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Converts variable binds to varbind section.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        internal static Sequence Transform(IList<Variable> variables)
        {
            IList<ISnmpData> varbinds = new List<ISnmpData>(2 * variables.Count);
            foreach (Variable v in variables)
            {
                varbinds.Add(new Sequence(v.Id, v.Data));
            }
            
            Sequence result = new Sequence(varbinds);
            return result;
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Variable"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Variable: Id: " + Id + "; Data: " + Data;
        }
    }
}