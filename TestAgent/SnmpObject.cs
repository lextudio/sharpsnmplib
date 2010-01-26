namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP basic object.
    /// </summary>
    internal abstract class SnmpObject: ISnmpObject
    {
        private readonly ObjectIdentifier _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpObject"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        protected SnmpObject(ObjectIdentifier id)
        {
            _id = id;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public abstract ISnmpData Data
        {
            get; set;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public ObjectIdentifier Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns></returns>
        public abstract bool MatchGetNext(ObjectIdentifier id);

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns></returns>
        public abstract bool MatchGet(ObjectIdentifier id);
    }
}