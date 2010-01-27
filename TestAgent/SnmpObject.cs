namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP basic object.
    /// </summary>
    internal abstract class SnmpObject: ISnmpObject
    {
        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public abstract ScalarObject MatchGetNext(ObjectIdentifier id);

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public abstract ScalarObject MatchGet(ObjectIdentifier id);
   }
}