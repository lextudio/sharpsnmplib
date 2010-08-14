using System.Collections.Generic;
using System.Linq;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Object that represents a table.
    /// </summary>
    public abstract class TableObject : SnmpObject
    {
        /// <summary>
        /// Gets the objects in the table.
        /// </summary>
        /// <value>The objects.</value>
        protected abstract IEnumerable<ScalarObject> Objects { get; }

        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public override ScalarObject MatchGetNext(ObjectIdentifier id)
        {
            return Objects.Select(o => o.MatchGetNext(id)).FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public override ScalarObject MatchGet(ObjectIdentifier id)
        {
            return Objects.Select(o => o.MatchGet(id)).FirstOrDefault(result => result != null);
        }
    }
}