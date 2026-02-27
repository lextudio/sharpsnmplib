namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    public enum PrivacyProtocol
    {
        /// <summary>
        /// usmNoPrivProtocol
        /// </summary>
        None = 0,

        /// <summary>
        /// usmDESPrivProtocol
        /// </summary>
        Des,

        /// <summary>
        /// usmAesCfb128Protocol
        /// </summary>
        Aes,

        /// <summary>
        /// usmAesCfb192Protocol
        /// </summary>
        Aes192,

        /// <summary>
        /// usmAesCfb256Protocol
        /// </summary>
        Aes256,
        TripleDes
    }
}
