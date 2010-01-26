namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP object.
    /// </summary>
    internal interface ISnmpObject
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        ISnmpData Data { get; set; }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        ObjectIdentifier Id { get; }

        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns></returns>
        bool MatchGetNext(ObjectIdentifier id);

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns></returns>
        bool MatchGet(ObjectIdentifier id);
    }
}
