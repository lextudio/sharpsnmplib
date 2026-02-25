namespace DotNetSnmp.Protocol.V3.Security
{
    /// <summary>
    /// Defines values for MsgFlags.
    /// </summary>
    [Flags]
    public enum MsgFlags : byte
    {
        /// <summary>
        /// Represents the NoAuthNoPriv value.
        /// </summary>
        NoAuthNoPriv = 0,
        /// <summary>
        /// Represents the Auth value.
        /// </summary>
        Auth = 1,
        /// <summary>
        /// Represents the Priv value.
        /// </summary>
        Priv = 2,
        /// <summary>
        /// Represents the Reportable value.
        /// </summary>
        Reportable = 4,
    }
}
