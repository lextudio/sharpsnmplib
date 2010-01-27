using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
    internal abstract class VectorObject : SnmpObject
    {
        protected abstract IEnumerable<ScalarObject> Objects
        { get; }

        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public override ScalarObject MatchGetNext(ObjectIdentifier id)
        {
            foreach (ScalarObject o in Objects)
            {
                ScalarObject result = o.MatchGetNext(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public override ScalarObject MatchGet(ObjectIdentifier id)
        {
            foreach (ScalarObject o in Objects)
            {
                ScalarObject result = o.MatchGet(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}