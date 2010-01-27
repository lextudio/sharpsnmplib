namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Scalar object interface.
    /// </summary>
    internal abstract class ScalarObject : SnmpObject
    {
        private readonly ObjectIdentifier _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarObject"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        protected ScalarObject(ObjectIdentifier id)
        {
            _id = id;
        }

        public Variable Variable
        {
            get { return new Variable(_id, Data); }
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        protected internal abstract ISnmpData Data
        {
            get; set;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        private ObjectIdentifier Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Matches the GET NEXT criteria.
        /// </summary>
        /// <param name="id">The ID in GET NEXT message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public override ScalarObject MatchGetNext(ObjectIdentifier id)
        {
            return Id > id ? this : null;
        }

        /// <summary>
        /// Matches the GET criteria.
        /// </summary>
        /// <param name="id">The ID in GET message.</param>
        /// <returns><c>null</c> if it does not match.</returns>
        public override ScalarObject MatchGet(ObjectIdentifier id)
        {
            return Id == id ? this : null;
        }
    }
}
