using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Segment interface.
    /// </summary>
    public interface ISegment
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        ISnmpData GetData(VersionCode version);

        /// <summary>
        /// Converts to <see cref="Sequence"/> object.
        /// </summary>
        /// <returns></returns>
        Sequence ToSequence();
    }
}
