namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP object.
    /// </summary>
    public interface ISnmpObject
    {
        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        ScalarObject MatchGetNext(ObjectIdentifier id);

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        ScalarObject MatchGet(ObjectIdentifier id);
    }
}
