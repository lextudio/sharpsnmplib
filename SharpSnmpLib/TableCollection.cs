using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SNMP table collection.
    /// </summary>
    public class TableCollection : Collection<Variable>
    {
        private readonly ObjectIdentifier _root;
        private readonly int _count;
        private readonly IDictionary<ObjectIdentifier, Variable> _table = new Dictionary<ObjectIdentifier, Variable>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCollection"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="list">The list.</param>
        public TableCollection(ObjectIdentifier root, IList<Variable> list)
        {
            _root = root;
            if (list.Count == 0)
            {
                _count = 0;
                return;
            }

            _count = (list[0].Id.ToNumerical().Length - root.ToNumerical().Length) / 2;
            foreach (Variable v in list)
            {
                _table.Add(v.Id, v);
            }
        }

        /// <summary>
        /// Gets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int IndexCount
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets the variable at.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public Variable GetVariableAt(params int[] position)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }

            if (position.Length == 0)
            {
                throw new ArgumentException("position cannot be empty", "position");
            }

//// ReSharper disable RedundantToStringCall
            StringBuilder key = new StringBuilder(_root.ToString() + ".1.");
//// ReSharper restore RedundantToStringCall
            foreach (int index in position)
            {
                key.Append("." + (index + 1));
            }

            ObjectIdentifier keyId = new ObjectIdentifier(key.ToString());
            if (_table.ContainsKey(keyId))
            {
                return _table[keyId];
            }

            return new Variable(keyId, new Null());
        }
    }
}
