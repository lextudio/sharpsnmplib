namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Segment interface for v12 compatibility.
    /// </summary>
    public interface ISegment
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        ISnmpData? GetData(VersionCode version);

        /// <summary>
        /// Converts to a <see cref="Sequence"/> object.
        /// </summary>
        Sequence ToSequence();
    }
}
