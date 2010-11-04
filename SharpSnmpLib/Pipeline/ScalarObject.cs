// Scalar object class.
// Copyright (C) 2009-2010 Lex Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System.Globalization;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Scalar object interface.
    /// </summary>
    public abstract class ScalarObject : SnmpObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarObject"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        protected ScalarObject(ObjectIdentifier id)
        {
            Id = id;
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
            get { return new Variable(Id, Data); }
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
        private ObjectIdentifier Id { get; set; }

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
