using System.Globalization;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Scalar object interface.
    /// </summary>
    public abstract class ScalarObject : SnmpObject
    {
        private readonly ObjectIdentifier _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarObject"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        protected ScalarObject(ObjectIdentifier id)
        {
            _id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarObject"/> class.
        /// </summary>
        /// <param name="dots">The ID string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        protected ScalarObject(string dots)
            : this(new ObjectIdentifier(dots))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarObject"/> class.
        /// </summary>
        /// <param name="dots">The ID string.</param>
        /// <param name="index">The index.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        protected ScalarObject(string dots, int index)
            : this(string.Format(CultureInfo.InvariantCulture, dots, index))
        {
        }

        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <value>The variable.</value>
        public Variable Variable
        {
            get { return new Variable(_id, Data); }
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
